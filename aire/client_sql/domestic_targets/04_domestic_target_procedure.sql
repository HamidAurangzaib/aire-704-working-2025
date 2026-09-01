

-- ------------------------------------------------------------
-- B1. UpdateIsFoundStatusForGFDomesticAirline
--     Step 1: IsTargetFound + IsOldTarget (reset month/deal flags)
--     Step 2: IsMonthTarget
--     Step 3: IsTargetDeal
--
--     Join keys are the same as the previous version
--     (From, To, Aircode, Dates BETWEEN FromDate/ToDate, Days) EXCEPT that
--     Days is now compared on the leading number only. comprGOOGLCOPY stores
--     "7 Nights" while the target upload screen writes "7 day", so the plain
--     c.[Days] = t.[Days] test could never match and NO domestic row was ever
--     flagged as a target. Comparing "7" to "7" makes it work for both
--     spellings, old rows included.
-- ------------------------------------------------------------
ALTER PROCEDURE [dbo].[UpdateIsFoundStatusForGFDomesticAirline]
AS
BEGIN
    -- Step 1: IsTargetFound / IsOldTarget; reset month + deal flags
    UPDATE c
    SET c.IsTargetFound = CASE
            WHEN c.[New_price] <= t.[Price] AND c.[Difference] <= -5 THEN 1
            ELSE 0
        END,
        c.IsOldTarget = CASE
            WHEN c.[New_price] <= t.[Price] AND c.[Difference] > -5 AND c.[Difference] <= 0 THEN 1
            ELSE 0
        END,
        c.IsMonthTarget   = 0,
        c.IsTargetDeal    = 0,
        c.IsTargetDealOld = 0
    FROM dbo.comprGOOGLCOPY c
    LEFT JOIN dbo.tblGfDomesticTarget t
        ON  c.[From]    = t.[From]
        AND c.[To]      = t.[To]
        AND c.[Aircode] = t.[Aircode]
        AND c.[Dates]   BETWEEN t.[FromDate] AND t.[ToDate]
        AND LEFT(c.[Days], CHARINDEX(' ', c.[Days] + ' ') - 1)
          = LEFT(t.[Days], CHARINDEX(' ', t.[Days] + ' ') - 1);

    -- Step 2: IsMonthTarget - Blue rows that have a Yellow row with the same
    --         (From, To, Airline, Stops, Cabin) but a DIFFERENT month, and no
    --         cheaper Blue row in the SAME month.
    UPDATE c
    SET c.IsMonthTarget = 1
    FROM dbo.comprGOOGLCOPY c
    WHERE c.IsTargetFound = 1
      AND EXISTS (
          SELECT 1
          FROM dbo.comprGOOGLCOPY y
          WHERE y.IsOldTarget = 1
            AND y.[From]   = c.[From]
            AND y.[To]     = c.[To]
            AND y.Airline  = c.Airline
            AND y.Stops    = c.Stops
            AND y.Cabin    = c.Cabin
            AND MONTH(y.Dates) <> MONTH(c.Dates)
      )
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.comprGOOGLCOPY cheaper
          WHERE cheaper.IsTargetFound = 1
            AND cheaper.[From]   = c.[From]
            AND cheaper.[To]     = c.[To]
            AND cheaper.Airline  = c.Airline
            AND cheaper.Stops    = c.Stops
            AND cheaper.Cabin    = c.Cabin
            AND MONTH(cheaper.Dates) = MONTH(c.Dates)
            AND YEAR(cheaper.Dates)  = YEAR(c.Dates)
            AND cheaper.New_price    < c.New_price
      );

    -- Step 3: IsTargetDeal - Blue rows whose New_price is strictly lower than
    --         every other flagged row in the same
    --         (From, To, Airline, Stops, Cabin, Aircode) group.
    UPDATE c
    SET c.IsTargetDeal = 1
    FROM dbo.comprGOOGLCOPY c
    WHERE c.IsTargetFound = 1
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.comprGOOGLCOPY o
          WHERE (o.IsTargetFound = 1 OR o.IsOldTarget = 1)
            AND o.[From]   = c.[From]
            AND o.[To]     = c.[To]
            AND o.Airline  = c.Airline
            AND o.Stops    = c.Stops
            AND o.Cabin    = c.Cabin
            AND o.Aircode  = c.Aircode
            AND o.New_price < c.New_price
      );
END;

