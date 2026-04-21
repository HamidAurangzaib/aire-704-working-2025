-- ================================================================
-- Run this ONCE on db_a61545_bobs (SQL8010) in SSMS
-- Sets up staging table and PublishStagingToLive stored procedure
--
-- Workflow after this:
--   1. Transfer button uploads to comprGOOGLAirline_Staging  (slow, website unaffected)
--   2. Publish button calls PublishStagingToLive             (builds indexes, then instant swap)
--   3. Website reads from comprGOOGLAirline as normal
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
-- Phase 1: Build all indexes on staging while website still reads
--          the live comprGOOGLAirline table (no downtime)
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

    -- ---- Phase 1: Build indexes on staging ----
    -- Website is still reading comprGOOGLAirline (live) during this phase.
    -- This is the only step that takes a few minutes.

    PRINT 'Phase 1: Building indexes on staging table...';

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'PK_Staging')
        CREATE CLUSTERED INDEX PK_Staging
            ON dbo.comprGOOGLAirline_Staging (id);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_airline_dates')
        CREATE NONCLUSTERED INDEX idx_airline_dates
            ON dbo.comprGOOGLAirline_Staging (Airline, Dates);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_dates')
        CREATE NONCLUSTERED INDEX idx_dates
            ON dbo.comprGOOGLAirline_Staging (Dates);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_from')
        CREATE NONCLUSTERED INDEX idx_from
            ON dbo.comprGOOGLAirline_Staging ([From]);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_from_to_dates')
        CREATE NONCLUSTERED INDEX idx_from_to_dates
            ON dbo.comprGOOGLAirline_Staging ([From], [To], Dates);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_from_to_dates_airline')
        CREATE NONCLUSTERED INDEX idx_from_to_dates_airline
            ON dbo.comprGOOGLAirline_Staging ([From], [To], Dates, Airline);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_from_to_dates_airline_include')
        CREATE NONCLUSTERED INDEX idx_from_to_dates_airline_include
            ON dbo.comprGOOGLAirline_Staging ([From], [To], Dates, Airline)
            INCLUDE (New_price, Stops, Days, Cabin);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('comprGOOGLAirline_Staging') AND name = 'idx_to')
        CREATE NONCLUSTERED INDEX idx_to
            ON dbo.comprGOOGLAirline_Staging ([To]);

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
