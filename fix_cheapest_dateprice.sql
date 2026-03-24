-- ============================================================
-- fix_cheapest_dateprice.sql
-- Run this in SSMS against DB_A61545_andycom
-- THIS IS THE PERMANENT FIX for Cheapest and DateNewPriceChanged
-- ============================================================

-- ============================================================
-- PART A: Create persistent PriceHistory table (run once)
-- This table survives row deletions in comprGOOGLAirline
-- It is the single source of truth for all-time cheapest price
-- and the date a price last changed per route/date/cabin etc.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'comprGOOGLAirline_PriceHistory')
BEGIN
    CREATE TABLE dbo.comprGOOGLAirline_PriceHistory (
        [From]               NVARCHAR(10)  NOT NULL,
        [To]                 NVARCHAR(10)  NOT NULL,
        Aircode              NVARCHAR(10)  NOT NULL,
        Cabin                NVARCHAR(50)  NOT NULL,
        [Days]               NVARCHAR(20)  NOT NULL,
        Stops                NVARCHAR(10)  NOT NULL,
        Dates                DATE          NOT NULL,
        MinPrice             FLOAT         NOT NULL,
        DatePriceLastChanged DATETIME      NULL,
        CONSTRAINT PK_comprGOOGLAirline_PriceHistory
            PRIMARY KEY ([From],[To],Aircode,Cabin,[Days],Stops,Dates)
    );
    PRINT 'Created comprGOOGLAirline_PriceHistory table';
END
ELSE
BEGIN
    PRINT 'comprGOOGLAirline_PriceHistory table already exists';
END

GO

-- ============================================================
-- PART B: Seed history table with current data
-- Only inserts rows not already in history
-- MinPrice = MIN(Olde_price, New_price) per row
-- DatePriceLastChanged = existing DateNewPriceChanged or NewUploadDate
-- ============================================================
INSERT INTO dbo.comprGOOGLAirline_PriceHistory
    ([From],[To],Aircode,Cabin,[Days],Stops,Dates,MinPrice,DatePriceLastChanged)
SELECT
    c.[From], c.[To], c.Aircode, c.Cabin, c.[Days], c.Stops, c.Dates,
    CASE
        WHEN c.Olde_price > 0 AND c.New_price > 0
            THEN CASE WHEN c.Olde_price < c.New_price THEN c.Olde_price ELSE c.New_price END
        WHEN c.New_price  > 0 THEN c.New_price
        WHEN c.Olde_price > 0 THEN c.Olde_price
        ELSE 0
    END,
    ISNULL(c.DateNewPriceChanged, c.NewUploadDate)
FROM dbo.comprGOOGLAirline c
WHERE (c.New_price > 0 OR c.Olde_price > 0)
  AND NOT EXISTS (
      SELECT 1 FROM dbo.comprGOOGLAirline_PriceHistory h
      WHERE h.[From]   = c.[From]   AND h.[To]     = c.[To]
        AND h.Aircode  = c.Aircode  AND h.Cabin    = c.Cabin
        AND h.[Days]   = c.[Days]   AND h.Stops    = c.Stops
        AND h.Dates    = c.Dates
  );

PRINT 'Seeded ' + CAST(@@ROWCOUNT AS VARCHAR) + ' rows into history table';

GO

-- ============================================================
-- PART C: ALTER upd_cmprgoogleAirline
-- Now uses PriceHistory as the source of truth
-- ============================================================
ALTER PROCEDURE [dbo].[upd_cmprgoogleAirline]
AS
BEGIN
    SET NOCOUNT ON;

    -- Step 1: Recalculate Difference
    UPDATE comprGOOGLAirline
    SET [Difference] = New_price - Olde_price;

    -- Step 2: Update existing history rows
    --   - Lower MinPrice if today's New_price is cheaper
    --   - Update DatePriceLastChanged only if price actually changed
    UPDATE h
    SET
        h.MinPrice = CASE
                        WHEN c.New_price > 0 AND c.New_price < h.MinPrice THEN c.New_price
                        ELSE h.MinPrice
                     END,
        h.DatePriceLastChanged = CASE
                                    WHEN c.New_price <> c.Olde_price THEN c.NewUploadDate
                                    ELSE ISNULL(h.DatePriceLastChanged, c.NewUploadDate)
                                 END
    FROM dbo.comprGOOGLAirline_PriceHistory h
    INNER JOIN dbo.comprGOOGLAirline c
        ON h.[From]  = c.[From]  AND h.[To]    = c.[To]
       AND h.Aircode = c.Aircode AND h.Cabin   = c.Cabin
       AND h.[Days]  = c.[Days]  AND h.Stops   = c.Stops
       AND h.Dates   = c.Dates
    WHERE c.New_price > 0;

    -- Step 3: Insert new rows not yet in history
    INSERT INTO dbo.comprGOOGLAirline_PriceHistory
        ([From],[To],Aircode,Cabin,[Days],Stops,Dates,MinPrice,DatePriceLastChanged)
    SELECT
        c.[From], c.[To], c.Aircode, c.Cabin, c.[Days], c.Stops, c.Dates,
        CASE
            WHEN c.Olde_price > 0 AND c.New_price > 0
                THEN CASE WHEN c.Olde_price < c.New_price THEN c.Olde_price ELSE c.New_price END
            WHEN c.New_price  > 0 THEN c.New_price
            WHEN c.Olde_price > 0 THEN c.Olde_price
            ELSE 0
        END,
        c.NewUploadDate
    FROM dbo.comprGOOGLAirline c
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.comprGOOGLAirline_PriceHistory h
        WHERE h.[From]  = c.[From]  AND h.[To]    = c.[To]
          AND h.Aircode = c.Aircode AND h.Cabin   = c.Cabin
          AND h.[Days]  = c.[Days]  AND h.Stops   = c.Stops
          AND h.Dates   = c.Dates
    )
    AND (c.New_price > 0 OR c.Olde_price > 0);

    -- Step 4: Apply history values back to comprGOOGLAirline
    --   Cheapest = all-time minimum from history
    --   DateNewPriceChanged = date price last actually changed
    UPDATE c
    SET
        c.Cheapest            = CASE WHEN h.MinPrice > 0 THEN h.MinPrice ELSE c.New_price END,
        c.DateNewPriceChanged = ISNULL(h.DatePriceLastChanged, c.NewUploadDate)
    FROM dbo.comprGOOGLAirline c
    INNER JOIN dbo.comprGOOGLAirline_PriceHistory h
        ON h.[From]  = c.[From]  AND h.[To]    = c.[To]
       AND h.Aircode = c.Aircode AND h.Cabin   = c.Cabin
       AND h.[Days]  = c.[Days]  AND h.Stops   = c.Stops
       AND h.Dates   = c.Dates;

END;

GO

-- ============================================================
-- PART D: Verify - check LON-BOM BA Economy sample
-- ============================================================
SELECT TOP 15
    c.Dates, c.Olde_price, c.New_price, c.[Difference],
    c.Cheapest, h.MinPrice AS History_MinPrice,
    c.DateNewPriceChanged, h.DatePriceLastChanged AS History_DateChanged,
    c.NewUploadDate
FROM dbo.comprGOOGLAirline c
LEFT JOIN dbo.comprGOOGLAirline_PriceHistory h
    ON h.[From]=c.[From] AND h.[To]=c.[To] AND h.Aircode=c.Aircode
    AND h.Cabin=c.Cabin AND h.[Days]=c.[Days] AND h.Stops=c.Stops
    AND h.Dates=c.Dates
WHERE c.[From]='LON' AND c.[To]='BOM' AND c.Aircode='BA' AND c.Cabin='Economy'
ORDER BY c.Dates;
