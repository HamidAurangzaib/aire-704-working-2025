

-- ------------------------------------------------------------
-- B2. serchWithoutFromToGOOGleDomestic
--     Adds IsOldTarget / IsMonthTarget / IsTargetDeal / IsTargetDealOld
--     after ai.photo (result columns 18-21 - the app reads them by index).
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
       m.IsOldTarget, m.IsMonthTarget, m.IsTargetDeal, m.IsTargetDealOld
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

