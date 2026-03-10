-- ============================================================
-- target_flags_changes.sql
-- Run this in SSMS against DB_A61545_andycom
-- ============================================================

-- ------------------------------------------------------------
-- 1. ADD NEW COLUMNS to comprGOOGLAirline
-- ------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLAirline') AND name = 'IsOldTarget')
    ALTER TABLE comprGOOGLAirline ADD IsOldTarget bit NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLAirline') AND name = 'IsMonthTarget')
    ALTER TABLE comprGOOGLAirline ADD IsMonthTarget bit NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLAirline') AND name = 'IsTargetDeal')
    ALTER TABLE comprGOOGLAirline ADD IsTargetDeal bit NULL;

GO

-- ------------------------------------------------------------
-- 2. ALTER PROCEDURE UpdateIsFoundStatusForGFAirline
--    Step 1: Set IsTargetFound + IsOldTarget (reset IsMonthTarget/IsTargetDeal to 0)
--    Step 2: Set IsMonthTarget=1 for Blue rows that have a Yellow in a different month
--    Step 3: Set IsTargetDeal=1 for Blue rows cheapest in their group
-- ------------------------------------------------------------
ALTER PROCEDURE [dbo].[UpdateIsFoundStatusForGFAirline]
AS
BEGIN
    -- Step 1: IsTargetFound, IsOldTarget, OtaDiscount, OtaTotal; reset month/deal flags
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
        c.IsTargetDeal  = 0,
        c.OtaDiscount   = COALESCE(t.OtaDiscount, 0),
        c.OtaTotal      = (c.[New_price] - COALESCE(t.OtaDiscount, 0))
    FROM dbo.comprGOOGLAirline c
    LEFT JOIN dbo.tblTarget t
        ON  c.[From]   = t.[From]
        AND c.[To]     = t.[To]
        AND c.[Aircode] = t.[Aircode]
        AND c.[Cabin]  = t.[Cabin]
        AND c.[Dates]  BETWEEN t.[FromDate] AND t.[ToDate];

    -- Step 2: IsMonthTarget — Blue rows that have a Yellow row with same
    --         (From, To, Airline, Stops, Cabin) but a DIFFERENT month,
    --         AND no Olde_price cheaper than the blue row's New_price exists
    --         in the SAME month (if one does, this row stays blue/IsTargetFound only)
    UPDATE c
    SET c.IsMonthTarget = 1
    FROM dbo.comprGOOGLAirline c
    WHERE c.IsTargetFound = 1
      AND EXISTS (
          SELECT 1
          FROM dbo.comprGOOGLAirline y
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
          FROM dbo.comprGOOGLAirline cheaper
          WHERE cheaper.[From]   = c.[From]
            AND cheaper.[To]     = c.[To]
            AND cheaper.Airline  = c.Airline
            AND cheaper.Stops    = c.Stops
            AND cheaper.Cabin    = c.Cabin
            AND MONTH(cheaper.Dates) = MONTH(c.Dates)
            AND YEAR(cheaper.Dates)  = YEAR(c.Dates)
            AND (
                (cheaper.IsTargetFound = 1 AND cheaper.New_price < c.New_price)
                OR
                (cheaper.Olde_price > 0 AND cheaper.Olde_price < c.New_price)
            )
      );

    -- Step 3: IsTargetDeal — Blue rows whose New_price is strictly lower than
    --         every other flagged row (Blue/Yellow/Purple) in the same
    --         (From, To, Airline, Stops, Cabin, Aircode) group
    UPDATE c
    SET c.IsTargetDeal = 1
    FROM dbo.comprGOOGLAirline c
    WHERE c.IsTargetFound = 1
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.comprGOOGLAirline o
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
-- 3. ALTER PROCEDURE serchWithoutFromToGOOGleAirline
--    Add IsOldTarget, IsMonthTarget, IsTargetDeal after ai.photo
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchWithoutFromToGOOGleAirline]
@IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@GreenDiff bit = 0,
@RedDiff bit = 0
AS BEGIN

WITH TheDates AS (
    SELECT @Fromdate AS TheDate
    UNION ALL
    SELECT DATEADD(DAY, 7, TheDate)
    FROM TheDates
    WHERE DATEADD(DAY, 7, TheDate) <= @Todate
)

SELECT m.[From], m.[To], m.citys, m.Dates, m.Olde_price, m.New_price,
       m.[Difference], m.Cheapest, m.Airline, m.Aircode, m.Cabin, m.[Days],
       m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.OtaDiscount, m.OtaTotal,
       m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
FROM comprGOOGLAirline m
LEFT JOIN airlinex ai ON m.Airline = ai.Airline
WHERE (m.Aircode = @Aircode OR @Aircode = '')
  AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
  AND (m.Stops = @Stops OR @Stops = '')
  AND (m.Cabin = @Cabin OR @Cabin = '')
  AND (m.Airline = @Airline OR @Airline = '')
  AND (
      (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
      (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
      (@Shortstays = 1 AND Dates IN (SELECT TheDate FROM TheDates))
  )
  AND ((@IsBetween = 1 AND m.New_price BETWEEN @MinPrice AND @MaxPrice) OR (@IsBetween = 0))
  AND ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
  AND ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
  AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
  AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
  AND (CASE WHEN @IsTargetOnly = 1 THEN m.IsTargetFound ELSE 1 END = 1)
ORDER BY m.New_price;
END

GO

-- ------------------------------------------------------------
-- 4. ALTER PROCEDURE serchFromMultiGroupCityGOOGleAirlineEverywhere
--    Add new cols to both CTE branches; change final SELECT m.* to explicit list
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchFromMultiGroupCityGOOGleAirlineEverywhere]
@From NVARCHAR(MAX) = '', @IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@EverywhereFrom bit = 0,
@GreenDiff bit = 0,
@RedDiff bit = 0
AS BEGIN

CREATE TABLE #CitiesStatus (
    MatchedFromCity NVARCHAR(MAX),
    UnmatchedFromCity NVARCHAR(MAX)
);

INSERT INTO #CitiesStatus (MatchedFromCity)
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

INSERT INTO #CitiesStatus (UnmatchedFromCity)
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

WITH MatchedCities AS (
    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal,
           t1.DateNewPriceChanged, t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLAirline t1
    INNER JOIN CodeCitys f ON t1.[From] = f.code
    WHERE (
         (@EverywhereFrom = 0 AND f.city IN (SELECT MatchedFromCity FROM #CitiesStatus)
            AND f.code NOT IN (SELECT REPLACE(UnmatchedFromCity, '-', '') FROM #CitiesStatus WHERE UnmatchedFromCity LIKE '-%'))
         OR
         (@EverywhereFrom = 1 AND f.city NOT IN (SELECT REPLACE(MatchedFromCity, '-', '') FROM #CitiesStatus WHERE MatchedFromCity IS NOT NULL)
            AND f.[code] NOT IN (SELECT REPLACE(UnMatchedFromCity, '-', '') FROM #CitiesStatus WHERE UnMatchedFromCity IS NOT NULL))
      )

    UNION ALL

    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal,
           t1.DateNewPriceChanged, t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLAirline t1
    WHERE (@EverywhereFrom = 0 AND t1.[From] IN (SELECT UnmatchedFromCity FROM #CitiesStatus))
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
       m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.OtaDiscount, m.OtaTotal,
       m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
FROM MatchedCities m
LEFT JOIN airlinex ai ON m.Airline = ai.Airline
WHERE (m.Aircode = @Aircode OR @Aircode = '')
  AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
  AND (m.Stops = @Stops OR @Stops = '')
  AND (m.Cabin = @Cabin OR @Cabin = '')
  AND (m.Airline = @Airline OR @Airline = '')
  AND (
      (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
      (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
      (@Shortstays = 1 AND Dates IN (SELECT TheDate FROM TheDates))
  )
  AND ((@IsBetween = 1 AND m.New_price BETWEEN @MinPrice AND @MaxPrice) OR (@IsBetween = 0))
  AND ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
  AND ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
  AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
  AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
  AND (CASE WHEN @IsTargetOnly = 1 THEN m.IsTargetFound ELSE 1 END = 1)
ORDER BY m.New_price;

DROP TABLE #CitiesStatus;
END

GO

-- ------------------------------------------------------------
-- 5. ALTER PROCEDURE serchToMultiGroupCityGOOGleAirlineEverywhere
--    Add new cols to both CTE branches; change final SELECT m.* to explicit list
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchToMultiGroupCityGOOGleAirlineEverywhere]
@To NVARCHAR(MAX) = '', @IsTargetOnly bit,
@Airline NVARCHAR(100) = '',
@Aircode NVARCHAR(50) = '',
@Days NVARCHAR(10) = '',
@Cabin NVARCHAR(100) = '',
@Fromdate date = '1997-01-01',
@Todate date = '1997-01-01',
@Shortstays bit = 0,
@IsBetween bit = 0,
@IsGreater bit = 0,
@IsLess bit = 1,
@MinPrice float = 0,
@MaxPrice float = 0,
@Stops NVARCHAR(10) = '',
@EverywhereTo bit = 0,
@GreenDiff bit = 0,
@RedDiff bit = 0
AS BEGIN

CREATE TABLE #CitiesStatus (
    MatchedToCity NVARCHAR(MAX),
    UnmatchedToCity NVARCHAR(MAX)
);

INSERT INTO #CitiesStatus (MatchedToCity)
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

INSERT INTO #CitiesStatus (UnmatchedToCity)
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

WITH MatchedCities AS (
    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal,
           t1.DateNewPriceChanged, t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLAirline t1
    INNER JOIN CodeCitys f ON t1.[To] = f.code
    WHERE (
         (@EverywhereTo = 0 AND f.city IN (SELECT MatchedToCity FROM #CitiesStatus)
            AND f.code NOT IN (SELECT REPLACE(UnmatchedToCity, '-', '') FROM #CitiesStatus WHERE UnmatchedToCity LIKE '-%'))
         OR
         (@EverywhereTo = 1 AND f.city NOT IN (SELECT REPLACE(MatchedToCity, '-', '') FROM #CitiesStatus WHERE MatchedToCity IS NOT NULL)
            AND f.[code] NOT IN (SELECT REPLACE(UnMatchedToCity, '-', '') FROM #CitiesStatus WHERE UnMatchedToCity IS NOT NULL))
      )

    UNION ALL

    SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
           t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
           t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal,
           t1.DateNewPriceChanged, t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
    FROM comprGOOGLAirline t1
    WHERE (@EverywhereTo = 0 AND t1.[To] IN (SELECT UnmatchedToCity FROM #CitiesStatus))
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
       m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.OtaDiscount, m.OtaTotal,
       m.DateNewPriceChanged, ai.photo,
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
FROM MatchedCities m
LEFT JOIN airlinex ai ON m.Airline = ai.Airline
WHERE (m.Aircode = @Aircode OR @Aircode = '')
  AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
  AND (m.Stops = @Stops OR @Stops = '')
  AND (m.Cabin = @Cabin OR @Cabin = '')
  AND (m.Airline = @Airline OR @Airline = '')
  AND (
      (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
      (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
      (@Shortstays = 1 AND Dates IN (SELECT TheDate FROM TheDates))
  )
  AND ((@IsBetween = 1 AND m.New_price BETWEEN @MinPrice AND @MaxPrice) OR (@IsBetween = 0))
  AND ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
  AND ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
  AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
  AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
  AND (CASE WHEN @IsTargetOnly = 1 THEN m.IsTargetFound ELSE 1 END = 1)
ORDER BY m.New_price;

DROP TABLE #CitiesStatus;
END

GO

-- ------------------------------------------------------------
-- 6. ALTER PROCEDURE serchFromToMultiGroupCityGOOGleAirlineEverywhere
--    Add new cols to MatchedCities CTE branches; change final SELECT m.* to explicit list
--    (MatchedToCities is only used as a WHERE filter so no column change needed there)
-- ------------------------------------------------------------
ALTER PROC [dbo].[serchFromToMultiGroupCityGOOGleAirlineEverywhere]
    @From NVARCHAR(MAX) = '',
    @To NVARCHAR(MAX) = '',
    @IsTargetOnly bit,
    @Airline NVARCHAR(100) = '',
    @Aircode NVARCHAR(50) = '',
    @Days NVARCHAR(10) = '',
    @Cabin NVARCHAR(100) = '',
    @Fromdate date = '1997-01-01',
    @Todate date = '1997-01-01',
    @Shortstays bit = 0,
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
AS
BEGIN

    CREATE TABLE #CitiesStatus (
        MatchedFromCity   NVARCHAR(MAX),
        UnmatchedFromCity NVARCHAR(MAX),
        MatchedToCity     NVARCHAR(MAX),
        UnmatchedToCity   NVARCHAR(MAX)
    );

    INSERT INTO #CitiesStatus (MatchedFromCity)
    SELECT RTRIM(LTRIM(FromCity.value))
    FROM dbo.SplitString(@From, ',') AS FromCity
    WHERE (
        @EverywhereFrom = 0
        AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(FromCity.value)))
    ) OR (
        @EverywhereFrom = 1
        AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(FromCity.value, '-', ''))))
    );

    INSERT INTO #CitiesStatus (UnmatchedFromCity)
    SELECT RTRIM(LTRIM(FromCity.value))
    FROM dbo.SplitString(@From, ',') AS FromCity
    WHERE (
        @EverywhereFrom = 0
        AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(FromCity.value)))
    ) OR (
        @EverywhereFrom = 1
        AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(FromCity.value, '-', ''))))
    );

    INSERT INTO #CitiesStatus (MatchedToCity)
    SELECT RTRIM(LTRIM(ToCity.value))
    FROM dbo.SplitString(@To, ',') AS ToCity
    WHERE (
        @EverywhereTo = 0
        AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(ToCity.value)))
    ) OR (
        @EverywhereTo = 1
        AND EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(ToCity.value, '-', ''))))
    );

    INSERT INTO #CitiesStatus (UnmatchedToCity)
    SELECT RTRIM(LTRIM(ToCity.value))
    FROM dbo.SplitString(@To, ',') AS ToCity
    WHERE (
        @EverywhereTo = 0
        AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(ToCity.value)))
    ) OR (
        @EverywhereTo = 1
        AND NOT EXISTS (SELECT 1 FROM CodeCitys WHERE City = RTRIM(LTRIM(REPLACE(ToCity.value, '-', ''))))
    );

    WITH MatchedCities AS (
        SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
               t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
               t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal,
               t1.DateNewPriceChanged, t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
        FROM comprGOOGLAirline t1
        INNER JOIN CodeCitys f ON t1.[From] = f.code
        WHERE (
            (@EverywhereFrom = 0 AND f.city IN (SELECT MatchedFromCity FROM #CitiesStatus)
                AND f.code NOT IN (SELECT REPLACE(UnmatchedFromCity, '-', '') FROM #CitiesStatus WHERE UnmatchedFromCity LIKE '-%'))
            OR
            (@EverywhereFrom = 1 AND f.city NOT IN (SELECT REPLACE(MatchedFromCity, '-', '') FROM #CitiesStatus WHERE MatchedFromCity IS NOT NULL)
                AND f.[code] NOT IN (SELECT REPLACE(UnMatchedFromCity, '-', '') FROM #CitiesStatus WHERE UnMatchedFromCity IS NOT NULL))
        )

        UNION ALL

        SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
               t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
               t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal,
               t1.DateNewPriceChanged, t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal
        FROM comprGOOGLAirline t1
        WHERE (@EverywhereFrom = 0 AND t1.[From] IN (SELECT UnmatchedFromCity FROM #CitiesStatus))
    ),
    MatchedToCities AS (
        SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
               t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
               t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal, t1.DateNewPriceChanged
        FROM comprGOOGLAirline t1
        INNER JOIN CodeCitys f ON t1.[To] = f.code
        WHERE (
            (@EverywhereTo = 0 AND f.city IN (SELECT MatchedToCity FROM #CitiesStatus)
                AND f.code NOT IN (SELECT REPLACE(UnmatchedToCity, '-', '') FROM #CitiesStatus WHERE UnmatchedToCity LIKE '-%'))
            OR
            (@EverywhereTo = 1 AND f.city NOT IN (SELECT REPLACE(MatchedToCity, '-', '') FROM #CitiesStatus WHERE MatchedToCity IS NOT NULL)
                AND f.[code] NOT IN (SELECT REPLACE(UnMatchedToCity, '-', '') FROM #CitiesStatus WHERE UnMatchedToCity IS NOT NULL))
        )

        UNION ALL

        SELECT t1.[From], t1.[To], t1.citys, t1.Dates, t1.Olde_price, t1.New_price,
               t1.[Difference], t1.Cheapest, t1.Airline, t1.Aircode, t1.Cabin, t1.[Days],
               t1.Stops, t1.web, t1.IsTargetFound, t1.NewUploadDate, t1.OtaDiscount, t1.OtaTotal, t1.DateNewPriceChanged
        FROM comprGOOGLAirline t1
        WHERE (@EverywhereTo = 0 AND t1.[To] IN (SELECT UnmatchedToCity FROM #CitiesStatus))
    ),
    TheDates AS (
        SELECT @Fromdate AS TheDate
        UNION ALL
        SELECT DATEADD(DAY, 7, TheDate)
        FROM TheDates
        WHERE DATEADD(DAY, 7, TheDate) <= @Todate
    )

    SELECT m.[From], m.[To], m.citys, m.Dates, m.Olde_price, m.New_price,
           m.[Difference], m.Cheapest, m.Airline, m.Aircode, m.Cabin, m.[Days],
           m.Stops, m.web, m.IsTargetFound, m.NewUploadDate, m.OtaDiscount, m.OtaTotal,
           m.DateNewPriceChanged, ai.photo,
           m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal
    FROM MatchedCities m
    LEFT JOIN airlinex ai ON m.Airline = ai.Airline
    WHERE m.[From] IN (SELECT [FROM] FROM MatchedToCities)
      AND m.[To]   IN (SELECT [TO]   FROM MatchedToCities)
      AND (m.Aircode = @Aircode OR @Aircode = '')
      AND (m.Days = CONCAT(@Days, ' Nights') OR m.Days = @Days OR @Days = '')
      AND (m.Stops = @Stops OR @Stops = '')
      AND (m.Cabin = @Cabin OR @Cabin = '')
      AND (m.Airline = @Airline OR @Airline = '')
      AND (
          (@Fromdate = '1997-01-01' AND @Todate = '1997-01-01') OR
          (@Shortstays = 0 AND m.Dates BETWEEN @Fromdate AND @Todate) OR
          (@Shortstays = 1 AND Dates IN (SELECT TheDate FROM TheDates))
      )
      AND ((@IsBetween = 1 AND m.New_price BETWEEN @MinPrice AND @MaxPrice) OR (@IsBetween = 0))
      AND ((@IsGreater = 1 AND m.New_price > @MinPrice) OR (@IsGreater = 0))
      AND ((@IsLess = 1 AND m.New_price < @MinPrice) OR (@IsLess = 0))
      AND ((@GreenDiff = 1 AND m.[Difference] < 0) OR (@GreenDiff = 0))
      AND ((@RedDiff = 1 AND m.[Difference] > 0) OR (@RedDiff = 0))
      AND (CASE WHEN @IsTargetOnly = 1 THEN m.IsTargetFound ELSE 1 END = 1)
    ORDER BY m.New_price;

    DROP TABLE #CitiesStatus;
END

GO
