-- ============================================================
-- domestic_target_flags_changes.sql
-- Run this in SSMS against DB_A61545_andycom
--
-- PART A: "new deal" fix (Olde_price = 0 must give Difference = 0
--          so the grid paints the Difference cell ORANGE, not RED)
-- PART B: full target categorisation (Blue / Yellow / Purple / Green)
--          for the COPY (domestic) side - comprGOOGLCOPY
-- ============================================================


-- ============================================================
-- PART A - "NEW DEAL" FIX  (comprGOOGLAirline)
-- ============================================================
-- Step 1 of upd_cmprgoogleAirline used to be an unconditional
--     UPDATE comprGOOGLAirline SET [Difference] = New_price - Olde_price;
-- so a route that only exists in the NEW file (Olde_price = 0) got
-- Difference = New_price, i.e. a big POSITIVE number, and the grid
-- painted it RED ("gone up") instead of ORANGE ("new deal").
--
-- The domestic side never had this problem because upd_cmprgoogleCOPY
-- forces Difference = 0 when Olde_price = 0. This makes the Airline
-- side behave the same way.
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
    --         is nothing to compare against: keep Difference = 0 -> ORANGE
    --         ("new deal") instead of a positive number -> RED ("gone up").
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

GO

-- One-off repair of the rows that are already wrong in the table, so you do
-- not have to wait for the next upload to see the ORANGE "new deal" rows.
UPDATE dbo.comprGOOGLAirline
SET [Difference] = 0
WHERE ISNULL(Olde_price, 0) = 0
  AND [Difference] <> 0;

GO


-- ============================================================
-- PART B - TARGET PRICES FOR COPY (DOMESTIC)
-- ============================================================
-- comprGOOGLCOPY already has IsOldTarget / IsMonthTarget / IsTargetDeal
-- columns, but nothing was filling them and the search procs did not
-- return them. This is the domestic mirror of target_flags_changes.sql.
--
--   Blue   (IsTargetFound) : New_price <= target AND Difference <= -5
--   Yellow (IsOldTarget)   : New_price <= target AND -5 < Difference <= 0
--   Purple (IsMonthTarget) : Blue row that has a Yellow row in another month
--   Green  (IsTargetDeal)  : Blue row that is the cheapest of its group
--
-- Note: comprGOOGLCOPY / tblGfDomesticTarget have no OtaDiscount / OtaTotal
-- columns, so the OTA part of the Airline procedure is not ported.
-- ------------------------------------------------------------

-- Safety net: create the flag columns if this database has not got them yet.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsOldTarget')
    ALTER TABLE comprGOOGLCOPY ADD IsOldTarget bit NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsMonthTarget')
    ALTER TABLE comprGOOGLCOPY ADD IsMonthTarget bit NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsTargetDeal')
    ALTER TABLE comprGOOGLCOPY ADD IsTargetDeal bit NULL;
GO


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
        c.IsMonthTarget = 0,
        c.IsTargetDeal  = 0
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

GO


-- ------------------------------------------------------------
-- B2. serchWithoutFromToGOOGleDomestic
--     Adds IsOldTarget / IsMonthTarget / IsTargetDeal after ai.photo
--     (result columns 18, 19, 20 - the C# reads them by index).
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchWithoutFromToGOOGleDomestic]
@From NVARCHAR(MAX) = '', @To NVARCHAR(MAX) = '', @IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@ChkNewPrice bit = 0,
@ChkDiffPrice bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@EverywhereFrom bit = 0,
@EverywhereTo bit = 0,
@GreenDiff bit = 0,
@RedDiff bit = 0
as begin

With TheDates AS (
               SELECT @Fromdate AS TheDate
               UNION ALL
               SELECT DATEADD(DAY, 7, TheDate)
               FROM TheDates
               WHERE DATEADD(DAY, 7, TheDate) <= @Todate
            )

select m.[From],m.[To],m.citys,m.Dates,m.Olde_price,m.New_price,m.[Difference],m.Cheapest,m.Airline,m.Aircode,m.Cabin,m.[Days],m.Stops,m.web,m.IsTargetFound,m.NewUploadDate, m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
From comprGOOGLCOPY m left join airlinex ai on m.Airline = ai.Airline WHERE (m.Aircode = @Aircode OR @Aircode = '') AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
AND (m.Stops = @Stops OR @Stops = '')
AND (m.Cabin = @Cabin OR @Cabin = '') AND (m.Airline = @Airline OR @Airline = '')
AND (
    (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
    (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
   (@Shortstays = 1 AND Dates IN (select TheDate from TheDates))
  )
AND (
      (
         (@ChkNewPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.New_price Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkNewPrice = 0)
)
AND (
      (
         (@ChkDiffPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.[Difference] Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.[Difference] > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.[Difference] < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkDiffPrice = 0)
)
AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
AND (
        CASE
            WHEN @IsTargetOnly = 1 THEN m.IsTargetFound
            ELSE 1
        END = 1
    );
end

GO


-- ------------------------------------------------------------
-- B3. serchFromMultiGroupCityGOOGleDomesticEverywhere
--     New columns added to the CTE and the final SELECT is now an
--     explicit column list (was SELECT m.*) so the column order is fixed.
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchFromMultiGroupCityGOOGleDomesticEverywhere]
@From NVARCHAR(MAX) = '', @IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@ChkNewPrice bit = 0,
@ChkDiffPrice bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@EverywhereFrom bit = 0,
@EverywhereTo bit = 0,
@GreenDiff bit = 0,
@RedDiff bit = 0
as begin

CREATE TABLE #CitiesStatus (
    MatchedFromCityGroup NVARCHAR(MAX),
    MatchedFromCity NVARCHAR(MAX),
    MatchedToCityGroup NVARCHAR(MAX),
    MatchedToCity NVARCHAR(MAX)
);

INSERT INTO #CitiesStatus (MatchedFromCityGroup)
SELECT RTRIM(LTRIM(FromCity.value))
FROM dbo.SplitString(@From, ',') AS FromCity
WHERE (
   @EverywhereFrom = 0
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(FromCity.value)))
   )
   OR (
   @EverywhereFrom = 1
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(FromCity.value, '-', ''))))
   );

INSERT INTO #CitiesStatus (MatchedFromCity)
SELECT RTRIM(LTRIM(FromCity.value))
FROM dbo.SplitString(@From, ',') AS FromCity
WHERE (
   @EverywhereFrom = 0
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(FromCity.value)))
   )
   OR (
   @EverywhereFrom = 1
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(FromCity.value, '-', ''))))
   );

WITH MatchedFromCities AS (
    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged,
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLCOPY t1
    INNER JOIN CodeCitys f ON t1.[From] = f.code
    WHERE (
         (
            (@EverywhereFrom = 0) AND (f.city IN (SELECT MatchedFromCityGroup FROM #CitiesStatus) AND f.code not in (SELECT REPLACE(MatchedFromCity, '-', '') FROM #CitiesStatus where MatchedFromCity LIKE '-%'))
         )
         OR
         (
            (@EverywhereFrom = 1) AND (f.city NOT IN (SELECT REPLACE(MatchedFromCityGroup, '-', '') FROM #CitiesStatus WHERE MatchedFromCityGroup IS NOT NULL)
                           AND f.[code] NOT IN (SELECT REPLACE(MatchedFromCity, '-', '') FROM #CitiesStatus WHERE MatchedFromCity IS NOT NULL))
         )
      )

    UNION ALL

    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged,
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLCOPY t1
    WHERE (@EverywhereFrom = 0 AND t1.[From] IN (SELECT MatchedFromCity FROM #CitiesStatus))
)
, TheDates AS (
               SELECT @Fromdate AS TheDate
               UNION ALL
               SELECT DATEADD(DAY, 7, TheDate)
               FROM TheDates
               WHERE DATEADD(DAY, 7, TheDate) <= @Todate
            )

SELECT m.[From], m.[To], m.citys, m.Dates, m.Olde_price, m.New_price,
       m.[Difference], m.Cheapest, m.Airline, m.Aircode, m.Cabin, m.[Days],
       m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
FROM MatchedFromCities m left join airlinex ai on m.Airline = ai.Airline
WHERE (m.Aircode = @Aircode OR @Aircode = '') AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
AND (m.Stops = @Stops OR @Stops = '')
AND (m.Cabin = @Cabin OR @Cabin = '') AND (m.Airline = @Airline OR @Airline = '')
AND (
    (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
    (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
   (@Shortstays = 1 AND Dates IN (select TheDate from TheDates))
  )
AND (
      (
         (@ChkNewPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.New_price Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkNewPrice = 0)
)
AND (
      (
         (@ChkDiffPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.[Difference] Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.[Difference] > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.[Difference] < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkDiffPrice = 0)
)
AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
AND (
        CASE
            WHEN @IsTargetOnly = 1 THEN m.IsTargetFound
            ELSE 1
        END = 1
    );

DROP TABLE #CitiesStatus;
end

GO


-- ------------------------------------------------------------
-- B4. serchToMultiGroupCityGOOGleDomesticEverywhere
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchToMultiGroupCityGOOGleDomesticEverywhere]
@To NVARCHAR(MAX) = '', @IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@ChkNewPrice bit = 0,
@ChkDiffPrice bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@EverywhereFrom bit = 0,
@EverywhereTo bit = 0,
@GreenDiff bit = 0,
@RedDiff bit = 0
as begin

CREATE TABLE #CitiesStatus (
    MatchedFromCityGroup NVARCHAR(MAX),
    MatchedFromCity NVARCHAR(MAX),
    MatchedToCityGroup NVARCHAR(MAX),
    MatchedToCity NVARCHAR(MAX)
);

INSERT INTO #CitiesStatus (MatchedToCityGroup)
SELECT RTRIM(LTRIM(ToCity.value))
FROM dbo.SplitString(@To, ',') AS ToCity
WHERE (
   @EverywhereTo = 0
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(ToCity.value)))
   )
   OR (
   @EverywhereTo = 1
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(ToCity.value, '-', ''))))
   );

INSERT INTO #CitiesStatus (MatchedToCity)
SELECT RTRIM(LTRIM(ToCity.value))
FROM dbo.SplitString(@To, ',') AS ToCity
WHERE (
   @EverywhereTo = 0
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(ToCity.value)))
   )
   OR (
   @EverywhereTo = 1
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(ToCity.value, '-', ''))))
   );

WITH MatchedToCities AS (
    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged,
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLCOPY t1
    INNER JOIN CodeCitys f ON t1.[To] = f.code
    WHERE (
         (
            (@EverywhereTo = 0) AND (f.city IN (SELECT MatchedToCityGroup FROM #CitiesStatus) AND f.code not in (SELECT REPLACE(MatchedToCity, '-', '') FROM #CitiesStatus where MatchedToCity LIKE '-%'))
         )
         OR
         (
            (@EverywhereTo = 1) AND (f.city NOT IN (SELECT REPLACE(MatchedToCityGroup, '-', '') FROM #CitiesStatus WHERE MatchedToCityGroup IS NOT NULL)
                           AND f.[code] NOT IN (SELECT REPLACE(MatchedToCity, '-', '') FROM #CitiesStatus WHERE MatchedToCity IS NOT NULL))
         )
      )

    UNION ALL

    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged,
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLCOPY t1
    WHERE (@EverywhereTo = 0 AND t1.[To] IN (SELECT MatchedToCity FROM #CitiesStatus))
)
, TheDates AS (
               SELECT @Fromdate AS TheDate
               UNION ALL
               SELECT DATEADD(DAY, 7, TheDate)
               FROM TheDates
               WHERE DATEADD(DAY, 7, TheDate) <= @Todate
            )

SELECT m.[From], m.[To], m.citys, m.Dates, m.Olde_price, m.New_price,
       m.[Difference], m.Cheapest, m.Airline, m.Aircode, m.Cabin, m.[Days],
       m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
from MatchedToCities m left join airlinex ai on m.Airline = ai.Airline
WHERE (m.Aircode = @Aircode OR @Aircode = '') AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
AND (m.Stops = @Stops OR @Stops = '')
AND (m.Cabin = @Cabin OR @Cabin = '') AND (m.Airline = @Airline OR @Airline = '')
AND (
    (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
    (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
   (@Shortstays = 1 AND Dates IN (select TheDate from TheDates))
  )
AND (
      (
         (@ChkNewPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.New_price Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkNewPrice = 0)
)
AND (
      (
         (@ChkDiffPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.[Difference] Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.[Difference] > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.[Difference] < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkDiffPrice = 0)
)
AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
AND (
        CASE
            WHEN @IsTargetOnly = 1 THEN m.IsTargetFound
            ELSE 1
        END = 1
    );

DROP TABLE #CitiesStatus;
end

GO


-- ------------------------------------------------------------
-- B5. serchFromToMultiGroupCityGOOGleDomesticEverywhere
--     MatchedToCities is only used as a WHERE filter, so only
--     MatchedFromCities needs the new columns.
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchFromToMultiGroupCityGOOGleDomesticEverywhere]
@From NVARCHAR(MAX) = '', @To NVARCHAR(MAX) = '', @IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@ChkNewPrice bit = 0,
@ChkDiffPrice bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@EverywhereFrom bit = 0,
@EverywhereTo bit = 0,
@GreenDiff bit = 0,
@RedDiff bit = 0
as begin

CREATE TABLE #CitiesStatus (
    MatchedFromCityGroup NVARCHAR(MAX),
    MatchedFromCity NVARCHAR(MAX),
    MatchedToCityGroup NVARCHAR(MAX),
    MatchedToCity NVARCHAR(MAX)
);

INSERT INTO #CitiesStatus (MatchedFromCityGroup)
SELECT RTRIM(LTRIM(FromCity.value))
FROM dbo.SplitString(@From, ',') AS FromCity
WHERE (
   @EverywhereFrom = 0
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(FromCity.value)))
   )
   OR (
   @EverywhereFrom = 1
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(FromCity.value, '-', ''))))
   );

INSERT INTO #CitiesStatus (MatchedFromCity)
SELECT RTRIM(LTRIM(FromCity.value))
FROM dbo.SplitString(@From, ',') AS FromCity
WHERE (
   @EverywhereFrom = 0
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(FromCity.value)))
   )
   OR (
   @EverywhereFrom = 1
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(FromCity.value, '-', ''))))
   );

INSERT INTO #CitiesStatus (MatchedToCityGroup)
SELECT RTRIM(LTRIM(ToCity.value))
FROM dbo.SplitString(@To, ',') AS ToCity
WHERE (
   @EverywhereTo = 0
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(ToCity.value)))
   )
   OR (
   @EverywhereTo = 1
   AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(ToCity.value, '-', ''))))
   );

INSERT INTO #CitiesStatus (MatchedToCity)
SELECT RTRIM(LTRIM(ToCity.value))
FROM dbo.SplitString(@To, ',') AS ToCity
WHERE (
   @EverywhereTo = 0
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(ToCity.value)))
   )
   OR (
   @EverywhereTo = 1
   AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(ToCity.value, '-', ''))))
   );

WITH MatchedFromCities AS (
    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged,
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLCOPY t1
    INNER JOIN CodeCitys f ON t1.[From] = f.code
    WHERE (
         (
            (@EverywhereFrom = 0) AND (f.city IN (SELECT MatchedFromCityGroup FROM #CitiesStatus) AND f.code not in (SELECT REPLACE(MatchedFromCity, '-', '') FROM #CitiesStatus where MatchedFromCity LIKE '-%'))
         )
         OR
         (
            (@EverywhereFrom = 1) AND (f.city NOT IN (SELECT REPLACE(MatchedFromCityGroup, '-', '') FROM #CitiesStatus WHERE MatchedFromCityGroup IS NOT NULL)
                           AND f.[code] NOT IN (SELECT REPLACE(MatchedFromCity, '-', '') FROM #CitiesStatus WHERE MatchedFromCity IS NOT NULL))
         )
      )

    UNION ALL

    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged,
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLCOPY t1
    WHERE (@EverywhereFrom = 0 AND t1.[From] IN (SELECT MatchedFromCity FROM #CitiesStatus))
),
MatchedToCities AS (
    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged
    FROM comprGOOGLCOPY t1
    INNER JOIN CodeCitys f ON t1.[To] = f.code
    WHERE (
         (
            (@EverywhereTo = 0) AND (f.city IN (SELECT MatchedToCityGroup FROM #CitiesStatus) AND f.code not in (SELECT REPLACE(MatchedToCity, '-', '') FROM #CitiesStatus where MatchedToCity LIKE '-%'))
         )
         OR
         (
            (@EverywhereTo = 1) AND (f.city NOT IN (SELECT REPLACE(MatchedToCityGroup, '-', '') FROM #CitiesStatus WHERE MatchedToCityGroup IS NOT NULL)
                           AND f.[code] NOT IN (SELECT REPLACE(MatchedToCity, '-', '') FROM #CitiesStatus WHERE MatchedToCity IS NOT NULL))
         )
      )

    UNION ALL

    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, T1.NewUploadDate, t1.DateNewPriceChanged
    FROM comprGOOGLCOPY t1
    WHERE (@EverywhereTo = 0 AND t1.[To] IN (SELECT MatchedToCity FROM #CitiesStatus))
)

, TheDates AS (
               SELECT @Fromdate AS TheDate
               UNION ALL
               SELECT DATEADD(DAY, 7, TheDate)
               FROM TheDates
               WHERE DATEADD(DAY, 7, TheDate) <= @Todate
            )

SELECT m.[From], m.[To], m.citys, m.Dates, m.Olde_price, m.New_price,
       m.[Difference], m.Cheapest, m.Airline, m.Aircode, m.Cabin, m.[Days],
       m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
FROM MatchedFromCities m left join airlinex ai on m.Airline = ai.Airline
WHERE m.[From] in (SELECT[FROM] from MatchedToCities) AND m.[To] in (SELECT[TO] from MatchedToCities)
AND (m.Aircode = @Aircode OR @Aircode = '') AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
AND (m.Stops = @Stops OR @Stops = '')
AND (m.Cabin = @Cabin OR @Cabin = '') AND (m.Airline = @Airline OR @Airline = '')
AND (
    (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
    (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
   (@Shortstays = 1 AND Dates IN (select TheDate from TheDates))
  )
AND (
      (
         (@ChkNewPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.New_price Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkNewPrice = 0)
)
AND (
      (
         (@ChkDiffPrice = 1)
         AND (
               ((@IsBetween = 1 AND m.[Difference] Between @MinPrice And @MaxPrice) OR (@IsBetween = 0))
               AND
               ((@IsGreater = 1 AND m.[Difference] > @MinPrice) OR (@IsGreater = 0))
               AND
               ((@IsLess = 1 AND m.[Difference] < @MinPrice) OR (@IsLess = 0))
            )
      )
      OR   (@ChkDiffPrice = 0)
)
AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
AND (
        CASE
            WHEN @IsTargetOnly = 1 THEN m.IsTargetFound
            ELSE 1
        END = 1
    );

DROP TABLE #CitiesStatus;
end

GO

-- ============================================================
-- PART C - REFRESH THE FLAGS FOR THE DATA THAT IS ALREADY LOADED
-- ============================================================
-- Part A changed Difference for the "new deal" rows on the Airline side,
-- and Part B added three new flags on the domestic side, so both target
-- procedures have to be re-run once. From now on they run automatically
-- at the end of each upload (Finish button).
EXEC dbo.UpdateIsFoundStatusForGFAirline;
GO

EXEC dbo.UpdateIsFoundStatusForGFDomesticAirline;
GO
