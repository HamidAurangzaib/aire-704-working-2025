

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
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal, t1.IsTargetDealOld
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
           t1.IsOldTarget, t1.IsMonthTarget, t1.IsTargetDeal, t1.IsTargetDealOld
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
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal, m.IsTargetDealOld
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

