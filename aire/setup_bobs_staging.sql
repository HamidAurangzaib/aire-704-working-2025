-- ================================================================
-- Run this ONCE on db_a61545_bobs (SQL8010) in SSMS
-- Sets up staging table and PublishStagingToLive stored procedure
--
-- Workflow after this:
--   1. Transfer button uploads to comprGOOGLAirline_Staging  (slow, website unaffected)
--   2. Publish button calls PublishStagingToLive             (copies indexes from live, then instant swap)
--   3. Website reads from comprGOOGLAirline as normal
--
-- Dynamic index replication: PublishStagingToLive reads whatever indexes exist
-- on comprGOOGLAirline at the time of publish and recreates them on staging.
-- No manual updates needed when indexes are added or changed.
-- ================================================================

-- ----------------------------------------------------------------
-- PART 1: Create staging table (no indexes = fastest possible upload)
-- ----------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'comprGOOGLAirline_Staging')
BEGIN
    CREATE TABLE dbo.comprGOOGLAirline_Staging (
        id                  INT             NOT NULL,
        [From]              VARCHAR(20)     NULL,
        [To]                VARCHAR(20)     NULL,
        citys               VARCHAR(40)     NULL,
        Dates               DATE            NULL,
        Olde_price          FLOAT           NULL,
        New_price           FLOAT           NULL,
        Difference          FLOAT           NULL,
        Cheapest            FLOAT           NULL,
        Airline             VARCHAR(100)    NULL,
        Aircode             VARCHAR(3)      NULL,
        Cabin               VARCHAR(30)     NULL,
        Days                VARCHAR(30)     NULL,
        Stops               VARCHAR(10)     NULL,
        web                 VARCHAR(MAX)    NULL,
        IsTargetFound       BIT             NULL,
        Name                VARCHAR(MAX)    NULL,
        NewUploadDate       DATE            NULL,
        OtaDiscount         FLOAT           NULL,
        OtaTotal            FLOAT           NULL,
        DateNewPriceChanged DATE            NULL,
        IsOldTarget         BIT             NULL,
        IsMonthTarget       BIT             NULL,
        IsTargetDeal        BIT             NULL,
        IsTargetDealOld     BIT             NULL
    );
    PRINT 'Created comprGOOGLAirline_Staging';
END
ELSE
    PRINT 'comprGOOGLAirline_Staging already exists';

GO

-- ----------------------------------------------------------------
-- PART 2: Create the publish stored procedure
--
-- Phase 1: Dynamically read all indexes from the live comprGOOGLAirline
--          table (using sys.indexes / sys.index_columns) and build the
--          same indexes on comprGOOGLAirline_Staging while the website
--          continues to read from the live table (no downtime).
-- Phase 2: Instant rename swap — staging becomes live (metadata only,
--          takes milliseconds regardless of row count)
-- Phase 3: Drop old live table
-- Phase 4: Recreate empty staging ready for next upload
-- ----------------------------------------------------------------
IF OBJECT_ID('dbo.PublishStagingToLive', 'P') IS NOT NULL
    DROP PROCEDURE dbo.PublishStagingToLive;

GO

CREATE PROCEDURE dbo.PublishStagingToLive
AS
BEGIN
    SET NOCOUNT ON;

    -- ---- Phase 1: Dynamically replicate indexes from live table to staging ----
    -- Reads sys.indexes and sys.index_columns for comprGOOGLAirline and
    -- builds the identical index set on comprGOOGLAirline_Staging.
    -- Website is still reading comprGOOGLAirline (live) during this phase.

    PRINT 'Phase 1: Building indexes on staging (mirroring live table)...';

    DECLARE @liveObjId   INT = OBJECT_ID('dbo.comprGOOGLAirline');
    DECLARE @idxName     NVARCHAR(128);
    DECLARE @isUnique    BIT;
    DECLARE @isClustered BIT;
    DECLARE @keyCols     NVARCHAR(MAX);
    DECLARE @inclCols    NVARCHAR(MAX);
    DECLARE @sql         NVARCHAR(MAX);

    -- Cursor over every index on the live table (skip system stats indexes)
    DECLARE idx_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT
            i.name,
            i.is_unique,
            CASE i.type WHEN 1 THEN 1 ELSE 0 END AS is_clustered
        FROM sys.indexes i
        WHERE i.object_id = @liveObjId
          AND i.type IN (1, 2)          -- 1=Clustered, 2=NonClustered
          AND i.is_primary_key = 0      -- skip PK (we handle clustered by name)
          AND i.is_unique_constraint = 0
        ORDER BY i.type;                -- clustered first

    OPEN idx_cursor;
    FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Skip if this index already exists on staging (idempotent)
        IF EXISTS (
            SELECT 1 FROM sys.indexes
            WHERE object_id = OBJECT_ID('dbo.comprGOOGLAirline_Staging')
              AND name = @idxName
        )
        BEGIN
            PRINT '  Skipping (already exists): ' + @idxName;
            FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;
            CONTINUE;
        END

        -- Build KEY columns list  (key_ordinal > 0, ordered)
        -- Use comma-only separator (no trailing space) so LEN() trims correctly
        SET @keyCols = '';
        SELECT @keyCols = @keyCols +
            QUOTENAME(c.name) +
            CASE ic.is_descending_key WHEN 1 THEN ' DESC' ELSE ' ASC' END + ','
        FROM sys.index_columns ic
        JOIN sys.columns c
            ON c.object_id = ic.object_id
           AND c.column_id = ic.column_id
        WHERE ic.object_id  = @liveObjId
          AND ic.index_id   = (SELECT index_id FROM sys.indexes
                                WHERE object_id = @liveObjId AND name = @idxName)
          AND ic.key_ordinal > 0
        ORDER BY ic.key_ordinal;

        -- Trim trailing ","
        IF LEN(@keyCols) > 0
            SET @keyCols = LEFT(@keyCols, LEN(@keyCols) - 1);

        -- Build INCLUDE columns list  (key_ordinal = 0, is_included_column = 1)
        SET @inclCols = '';
        SELECT @inclCols = @inclCols + QUOTENAME(c.name) + ','
        FROM sys.index_columns ic
        JOIN sys.columns c
            ON c.object_id = ic.object_id
           AND c.column_id = ic.column_id
        WHERE ic.object_id         = @liveObjId
          AND ic.index_id          = (SELECT index_id FROM sys.indexes
                                       WHERE object_id = @liveObjId AND name = @idxName)
          AND ic.is_included_column = 1
        ORDER BY ic.index_column_id;

        IF LEN(@inclCols) > 0
            SET @inclCols = LEFT(@inclCols, LEN(@inclCols) - 1);

        -- Compose the CREATE INDEX statement
        SET @sql =
            'CREATE ' +
            CASE WHEN @isUnique = 1 THEN 'UNIQUE ' ELSE '' END +
            CASE WHEN @isClustered = 1 THEN 'CLUSTERED ' ELSE 'NONCLUSTERED ' END +
            'INDEX ' + QUOTENAME(@idxName) +
            ' ON dbo.comprGOOGLAirline_Staging (' + @keyCols + ')' +
            CASE WHEN LEN(@inclCols) > 0 THEN ' INCLUDE (' + @inclCols + ')' ELSE '' END;

        PRINT '  Creating: ' + @idxName;
        EXEC sp_executesql @sql;

        FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;
    END

    CLOSE idx_cursor;
    DEALLOCATE idx_cursor;

    PRINT 'Phase 1 complete. All indexes built on staging.';

    -- ---- Phase 2: Instant rename swap ----
    -- Pure metadata operation — takes milliseconds.
    -- Website is live right up until this point, then instantly serves new data.

    PRINT 'Phase 2: Swapping staging to live...';

    -- Remove any leftover backup from a previous failed run
    IF OBJECT_ID('dbo.comprGOOGLAirline_Old', 'U') IS NOT NULL
        DROP TABLE dbo.comprGOOGLAirline_Old;

    EXEC sp_rename 'comprGOOGLAirline',         'comprGOOGLAirline_Old';
    EXEC sp_rename 'comprGOOGLAirline_Staging', 'comprGOOGLAirline';

    PRINT 'Phase 2 complete. Website now reading new data.';

    -- ---- Phase 3: Drop old table ----
    DROP TABLE dbo.comprGOOGLAirline_Old;
    PRINT 'Phase 3 complete. Old table dropped.';

    -- ---- Phase 4: Recreate empty staging for next upload ----
    CREATE TABLE dbo.comprGOOGLAirline_Staging (
        id                  INT             NOT NULL,
        [From]              VARCHAR(20)     NULL,
        [To]                VARCHAR(20)     NULL,
        citys               VARCHAR(40)     NULL,
        Dates               DATE            NULL,
        Olde_price          FLOAT           NULL,
        New_price           FLOAT           NULL,
        Difference          FLOAT           NULL,
        Cheapest            FLOAT           NULL,
        Airline             VARCHAR(100)    NULL,
        Aircode             VARCHAR(3)      NULL,
        Cabin               VARCHAR(30)     NULL,
        Days                VARCHAR(30)     NULL,
        Stops               VARCHAR(10)     NULL,
        web                 VARCHAR(MAX)    NULL,
        IsTargetFound       BIT             NULL,
        Name                VARCHAR(MAX)    NULL,
        NewUploadDate       DATE            NULL,
        OtaDiscount         FLOAT           NULL,
        OtaTotal            FLOAT           NULL,
        DateNewPriceChanged DATE            NULL,
        IsOldTarget         BIT             NULL,
        IsMonthTarget       BIT             NULL,
        IsTargetDeal        BIT             NULL,
        IsTargetDealOld     BIT             NULL
    );

    PRINT 'Phase 4 complete. Fresh staging table ready for next upload.';
    PRINT 'Publish complete.';
END

GO

PRINT 'Setup complete. Run transfer button to upload to staging, then Publish to go live.';
