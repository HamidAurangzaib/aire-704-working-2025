CREATE PROCEDURE dbo.PublishStagingToLive
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @stagingRows INT;
    SELECT @stagingRows = COUNT(*) FROM dbo.comprGOOGLAirline_Staging;
    IF @stagingRows = 0
    BEGIN
        RAISERROR('Staging table is empty - publish aborted to prevent data loss. Run Transfer first.', 16, 1);
        RETURN;
    END
    PRINT 'Safety check passed: ' + CAST(@stagingRows AS NVARCHAR) + ' rows in staging.';

    PRINT 'Phase 1: Building indexes on staging...';

    DECLARE @liveObjId   INT = OBJECT_ID('dbo.comprGOOGLAirline');
    DECLARE @idxName     NVARCHAR(128);
    DECLARE @isUnique    BIT;
    DECLARE @isClustered BIT;
    DECLARE @keyCols     NVARCHAR(MAX);
    DECLARE @inclCols    NVARCHAR(MAX);
    DECLARE @sql         NVARCHAR(MAX);

    DECLARE idx_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT i.name, i.is_unique,
               CASE i.type WHEN 1 THEN 1 ELSE 0 END AS is_clustered
        FROM sys.indexes i
        WHERE i.object_id = @liveObjId
          AND i.type IN (1, 2)
          AND i.is_primary_key = 0
          AND i.is_unique_constraint = 0
        ORDER BY i.type;

    OPEN idx_cursor;
    FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS (
            SELECT 1 FROM sys.indexes
            WHERE object_id = OBJECT_ID('dbo.comprGOOGLAirline_Staging')
              AND name = @idxName
        )
        BEGIN
            FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;
            CONTINUE;
        END

        SET @keyCols = '';
        SELECT @keyCols = @keyCols +
            QUOTENAME(c.name) +
            CASE ic.is_descending_key WHEN 1 THEN ' DESC' ELSE ' ASC' END + ','
        FROM sys.index_columns ic
        JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
        WHERE ic.object_id = @liveObjId
          AND ic.index_id  = (SELECT index_id FROM sys.indexes WHERE object_id = @liveObjId AND name = @idxName)
          AND ic.key_ordinal > 0
        ORDER BY ic.key_ordinal;

        IF LEN(@keyCols) > 0
            SET @keyCols = LEFT(@keyCols, LEN(@keyCols) - 1);

        SET @inclCols = '';
        SELECT @inclCols = @inclCols + QUOTENAME(c.name) + ','
        FROM sys.index_columns ic
        JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
        WHERE ic.object_id        = @liveObjId
          AND ic.index_id         = (SELECT index_id FROM sys.indexes WHERE object_id = @liveObjId AND name = @idxName)
          AND ic.is_included_column = 1
        ORDER BY ic.index_column_id;

        IF LEN(@inclCols) > 0
            SET @inclCols = LEFT(@inclCols, LEN(@inclCols) - 1);

        SET @sql =
            'CREATE ' +
            CASE WHEN @isUnique    = 1 THEN 'UNIQUE '    ELSE '' END +
            CASE WHEN @isClustered = 1 THEN 'CLUSTERED ' ELSE 'NONCLUSTERED ' END +
            'INDEX ' + QUOTENAME(@idxName) +
            ' ON dbo.comprGOOGLAirline_Staging (' + @keyCols + ')' +
            CASE WHEN LEN(@inclCols) > 0 THEN ' INCLUDE (' + @inclCols + ')' ELSE '' END;

        EXEC sp_executesql @sql;
        FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;
    END

    CLOSE idx_cursor;
    DEALLOCATE idx_cursor;
    PRINT 'Phase 1 complete.';

    PRINT 'Phase 2: Swapping staging to live...';

    IF OBJECT_ID('dbo.comprGOOGLAirline_Old', 'U') IS NOT NULL
        DROP TABLE dbo.comprGOOGLAirline_Old;

    EXEC sp_rename 'comprGOOGLAirline',         'comprGOOGLAirline_Old';
    EXEC sp_rename 'comprGOOGLAirline_Staging', 'comprGOOGLAirline';

    PRINT 'Phase 2 complete. Website now reading new data.';

    DROP TABLE dbo.comprGOOGLAirline_Old;
    PRINT 'Phase 3 complete.';

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
