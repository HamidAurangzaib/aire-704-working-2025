-- ============================================================
-- domestic_target_flags_changes.sql
-- Run this in SSMS against DB_A61545_andycom
--
-- PART A: "new deal" fix - a route with no old price must not be reported
--          as a price rise (Difference must be 0, not New_price)
-- PART B: full target categorisation (Blue / Yellow / Purple / Green / Orange)
--          for the COPY (domestic) side - comprGOOGLCOPY
-- PART C: refresh both target procedures for the data already loaded
-- ============================================================


-- ============================================================
-- PART A - "NEW DEAL" FIX  (comprGOOGLAirline)
-- ============================================================
-- Step 1 of upd_cmprgoogleAirline used to be an unconditional
--     UPDATE comprGOOGLAirline SET [Difference] = New_price - Olde_price;
-- so a route that only exists in the NEW file (Olde_price = 0) got
-- Difference = New_price, i.e. a big POSITIVE number, which reads as a
-- price rise ("gone up") when it is really a brand new deal.
--
-- That wrong figure is not only a colour problem: the target rules
-- (Difference <= -5) and the "Red Diff" search filter (Difference > 0)
-- both read it.
--
-- The domestic side never had this problem because upd_cmprgoogleCOPY
-- forces Difference = 0 when Olde_price = 0. This makes the Airline
-- side behave the same way. In the application these rows are shown in
-- CYAN ("new deal"), separate from GREY ("route gone").
-- ------------------------------------------------------------
ALTER PROCEDURE [dbo].[upd_cmprgoogleAirline]
AS
BEGIN
    SET NOCOUNT ON;

    -- Step 0: Detect duplicate (route+date) combos and SKIP them entirely.
    -- Duplicates usually mean a wrong URL / bad source row, so we never display
    -- them on the website. The skipped routes are logged so you can investigate.
    IF OBJECT_ID('tempdb..#DupKeys') IS NOT NULL DROP TABLE #DupKeys;
    SELECT [From],[To],Aircode,Cabin,[Days],Stops,Dates, COUNT(*) AS dup_count
    INTO #DupKeys
    FROM dbo.comprGOOGLAirline
    GROUP BY [From],[To],Aircode,Cabin,[Days],Stops,Dates
    HAVING COUNT(*) > 1;

    DECLARE @SkippedCount INT = (SELECT COUNT(*) FROM #DupKeys);

    IF @SkippedCount > 0
    BEGIN
        -- Log the skipped routes (Name not known here so left NULL)
        INSERT INTO dbo.SkippedDuplicateRoutes_Log
            ([From],[To],Aircode,Cabin,[Days],Stops,Dates,DuplicateCount,Note)
        SELECT [From],[To],Aircode,Cabin,[Days],Stops,Dates,dup_count,
               'Skipped during upd_cmprgoogleAirline'
        FROM #DupKeys;

        -- Delete ALL rows for these combos (not just the extras) so the website
        -- never shows inconsistent prices for the same flight
        DELETE c
        FROM dbo.comprGOOGLAirline c
        INNER JOIN #DupKeys d
            ON c.[From]  = d.[From]  AND c.[To]    = d.[To]
           AND c.Aircode = d.Aircode AND c.Cabin   = d.Cabin
           AND c.[Days]  = d.[Days]  AND c.Stops   = d.Stops
           AND c.Dates   = d.Dates;

        PRINT 'WARNING: ' + CAST(@SkippedCount AS NVARCHAR)
            + ' route+date combos had duplicates and were SKIPPED. '
            + 'See dbo.SkippedDuplicateRoutes_Log for details.';
    END

    DROP TABLE #DupKeys;

    -- Step 1: Recalculate Difference.
    --         Olde_price = 0 means the route is only in the NEW file, so there
    --         is nothing to compare against: keep Difference = 0 rather than
    --         a positive number that would read as a price rise.
    UPDATE comprGOOGLAirline
    SET [Difference] = CASE
                          WHEN ISNULL(Olde_price, 0) = 0 THEN 0
                          ELSE New_price - Olde_price
                       END;

    -- Step 2: Update existing history rows
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

