-- ============================================================================
-- Target Categorization SQL Stored Procedures
-- ============================================================================
-- Description:
-- This file contains SQL stored procedures for categorizing flight deals:
-- 1. IsOldTarget (Yellow): Difference between -5 and 0
-- 2. IsMonthTarget (Purple): Blue targets with different months than yellows
-- 3. TargetDeal (Green): Cheapest New_price among all categories
-- ============================================================================

-- First, add the new columns to the comprGOOGLAirline table if they don't exist
-- Run these ALTER TABLE statements first in your database:

/*
-- Check if columns exist, if not, add them
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[comprGOOGLAirline]') AND name = 'IsOldTarget')
BEGIN
    ALTER TABLE comprGOOGLAirline ADD IsOldTarget BIT DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[comprGOOGLAirline]') AND name = 'IsMonthTarget')
BEGIN
    ALTER TABLE comprGOOGLAirline ADD IsMonthTarget BIT DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[comprGOOGLAirline]') AND name = 'TargetDeal')
BEGIN
    ALTER TABLE comprGOOGLAirline ADD TargetDeal BIT DEFAULT 0;
END
*/

-- ============================================================================
-- Stored Procedure 1: Calculate IsOldTarget (Yellow)
-- ============================================================================
-- Description: Marks records where Difference is between -5 and 0
-- This includes: 0, -1, -2, -3, -4, -5
-- ============================================================================

IF OBJECT_ID('dbo.SP_Calculate_IsOldTarget', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_Calculate_IsOldTarget;
GO

CREATE PROCEDURE dbo.SP_Calculate_IsOldTarget
    @Name VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Update IsOldTarget based on Difference criteria
    -- Yellow records: Difference between -5 and 0 (inclusive)
    IF @Name IS NULL
    BEGIN
        UPDATE comprGOOGLAirline
        SET IsOldTarget = 
            CASE 
                WHEN Difference >= -5 AND Difference <= 0 
                THEN 1 
                ELSE 0 
            END;
    END
    ELSE
    BEGIN
        UPDATE comprGOOGLAirline
        SET IsOldTarget = 
            CASE 
                WHEN Difference >= -5 AND Difference <= 0 
                THEN 1 
                ELSE 0 
            END
        WHERE Name = @Name;
    END

    -- Return count of records updated
    SELECT COUNT(*) as UpdatedRecords
    FROM comprGOOGLAirline
    WHERE IsOldTarget = 1
        AND (@Name IS NULL OR Name = @Name);
END;
GO

-- ============================================================================
-- Stored Procedure 2: Calculate IsMonthTarget (Purple)
-- ============================================================================
-- Description: Marks blue records that have different months compared to
--              yellow records with same (From, To, Airline, Stops, Cabin)
-- Logic:
--   1. Find blue records (IsTargetFound=1 AND IsOldTarget=0)
--   2. Check if yellow records (IsOldTarget=1) exist with same route details
--   3. If months are different, mark blue as purple (IsMonthTarget=1)
-- ============================================================================

IF OBJECT_ID('dbo.SP_Calculate_IsMonthTarget', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_Calculate_IsMonthTarget;
GO

CREATE PROCEDURE dbo.SP_Calculate_IsMonthTarget
    @Name VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Reset IsMonthTarget first
    IF @Name IS NULL
    BEGIN
        UPDATE comprGOOGLAirline
        SET IsMonthTarget = 0;
    END
    ELSE
    BEGIN
        UPDATE comprGOOGLAirline
        SET IsMonthTarget = 0
        WHERE Name = @Name;
    END

    -- Set IsMonthTarget for blue records with different months than yellows
    IF @Name IS NULL
    BEGIN
        UPDATE blue
        SET blue.IsMonthTarget = 1
        FROM comprGOOGLAirline blue
        WHERE blue.IsTargetFound = 1
            AND blue.IsOldTarget = 0
            AND EXISTS (
                SELECT 1
                FROM comprGOOGLAirline yellow
                WHERE yellow.IsOldTarget = 1
                    AND blue.[From] = yellow.[From]
                    AND blue.[To] = yellow.[To]
                    AND blue.Airline = yellow.Airline
                    AND blue.Stops = yellow.Stops
                    AND blue.Cabin = yellow.Cabin
                    AND (MONTH(blue.Dates) <> MONTH(yellow.Dates) 
                         OR YEAR(blue.Dates) <> YEAR(yellow.Dates))
            );
    END
    ELSE
    BEGIN
        UPDATE blue
        SET blue.IsMonthTarget = 1
        FROM comprGOOGLAirline blue
        WHERE blue.Name = @Name
            AND blue.IsTargetFound = 1
            AND blue.IsOldTarget = 0
            AND EXISTS (
                SELECT 1
                FROM comprGOOGLAirline yellow
                WHERE yellow.Name = @Name
                    AND yellow.IsOldTarget = 1
                    AND blue.[From] = yellow.[From]
                    AND blue.[To] = yellow.[To]
                    AND blue.Airline = yellow.Airline
                    AND blue.Stops = yellow.Stops
                    AND blue.Cabin = yellow.Cabin
                    AND (MONTH(blue.Dates) <> MONTH(yellow.Dates) 
                         OR YEAR(blue.Dates) <> YEAR(yellow.Dates))
            );
    END

    -- Return count of records updated
    SELECT COUNT(*) as UpdatedRecords
    FROM comprGOOGLAirline
    WHERE IsMonthTarget = 1
        AND (@Name IS NULL OR Name = @Name);
END;
GO

-- ============================================================================
-- Stored Procedure 3: Calculate TargetDeal (Green)
-- ============================================================================
-- Description: Marks blue records with the cheapest New_price among all
--              categories for same (From, To, Airline, Stops, Cabin, Aircode)
-- Logic:
--   1. Find minimum New_price for each unique route combination
--   2. Mark blue records (not yellow, not purple) with this minimum price
--   3. These are the best "TargetDeal" records
-- ============================================================================

IF OBJECT_ID('dbo.SP_Calculate_TargetDeal', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_Calculate_TargetDeal;
GO

CREATE PROCEDURE dbo.SP_Calculate_TargetDeal
    @Name VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Reset TargetDeal first
    IF @Name IS NULL
    BEGIN
        UPDATE comprGOOGLAirline
        SET TargetDeal = 0;
    END
    ELSE
    BEGIN
        UPDATE comprGOOGLAirline
        SET TargetDeal = 0
        WHERE Name = @Name;
    END

    -- Set TargetDeal for blue records that are cheaper than yellow, purple, and other blue records
    IF @Name IS NULL
    BEGIN
        UPDATE blue
        SET blue.TargetDeal = 1
        FROM comprGOOGLAirline blue
        WHERE blue.IsTargetFound = 1
            AND blue.IsOldTarget = 0
            AND blue.IsMonthTarget = 0
            AND blue.New_price > 0
            -- Must be cheaper than all yellow records with same route (strictly less than)
            AND NOT EXISTS (
                SELECT 1
                FROM comprGOOGLAirline yellow
                WHERE yellow.IsOldTarget = 1
                    AND blue.[From] = yellow.[From]
                    AND blue.[To] = yellow.[To]
                    AND blue.Airline = yellow.Airline
                    AND blue.Stops = yellow.Stops
                    AND blue.Cabin = yellow.Cabin
                    AND blue.Aircode = yellow.Aircode
                    AND yellow.New_price > 0
                    AND yellow.New_price <= blue.New_price
            )
            -- Must be cheaper than all purple records with same route (strictly less than)
            AND NOT EXISTS (
                SELECT 1
                FROM comprGOOGLAirline purple
                WHERE purple.IsMonthTarget = 1
                    AND blue.[From] = purple.[From]
                    AND blue.[To] = purple.[To]
                    AND blue.Airline = purple.Airline
                    AND blue.Stops = purple.Stops
                    AND blue.Cabin = purple.Cabin
                    AND blue.Aircode = purple.Aircode
                    AND purple.New_price > 0
                    AND purple.New_price <= blue.New_price
            )
            -- Must be cheaper than all other blue records with same route
            AND NOT EXISTS (
                SELECT 1
                FROM comprGOOGLAirline otherBlue
                WHERE otherBlue.IsTargetFound = 1
                    AND otherBlue.id <> blue.id
                    AND blue.[From] = otherBlue.[From]
                    AND blue.[To] = otherBlue.[To]
                    AND blue.Airline = otherBlue.Airline
                    AND blue.Stops = otherBlue.Stops
                    AND blue.Cabin = otherBlue.Cabin
                    AND blue.Aircode = otherBlue.Aircode
                    AND otherBlue.New_price > 0
                    AND otherBlue.New_price < blue.New_price
            );
    END
    ELSE
    BEGIN
        UPDATE blue
        SET blue.TargetDeal = 1
        FROM comprGOOGLAirline blue
        WHERE blue.Name = @Name
            AND blue.IsTargetFound = 1
            AND blue.IsOldTarget = 0
            AND blue.IsMonthTarget = 0
            AND blue.New_price > 0
            -- Must be cheaper than all yellow records with same route
            AND NOT EXISTS (
                SELECT 1
                FROM comprGOOGLAirline yellow
                WHERE yellow.Name = @Name
                    AND yellow.IsOldTarget = 1
                    AND blue.[From] = yellow.[From]
                    AND blue.[To] = yellow.[To]
                    AND blue.Airline = yellow.Airline
                    AND blue.Stops = yellow.Stops
                    AND blue.Cabin = yellow.Cabin
                    AND blue.Aircode = yellow.Aircode
                    AND yellow.New_price > 0
                    AND yellow.New_price <= blue.New_price
            )
            -- Must be cheaper than all purple records with same route
            AND NOT EXISTS (
                SELECT 1
                FROM comprGOOGLAirline purple
                WHERE purple.Name = @Name
                    AND purple.IsMonthTarget = 1
                    AND blue.[From] = purple.[From]
                    AND blue.[To] = purple.[To]
                    AND blue.Airline = purple.Airline
                    AND blue.Stops = purple.Stops
                    AND blue.Cabin = purple.Cabin
                    AND blue.Aircode = purple.Aircode
                    AND purple.New_price > 0
                    AND purple.New_price <= blue.New_price
            )
            -- Must be cheaper than all other blue records with same route
            AND NOT EXISTS (
                SELECT 1
                FROM comprGOOGLAirline otherBlue
                WHERE otherBlue.Name = @Name
                    AND otherBlue.IsTargetFound = 1
                    AND otherBlue.id <> blue.id
                    AND blue.[From] = otherBlue.[From]
                    AND blue.[To] = otherBlue.[To]
                    AND blue.Airline = otherBlue.Airline
                    AND blue.Stops = otherBlue.Stops
                    AND blue.Cabin = otherBlue.Cabin
                    AND blue.Aircode = otherBlue.Aircode
                    AND otherBlue.New_price > 0
                    AND otherBlue.New_price < blue.New_price
            );
    END

    -- Return count of records updated
    SELECT COUNT(*) as UpdatedRecords
    FROM comprGOOGLAirline
    WHERE TargetDeal = 1
        AND (@Name IS NULL OR Name = @Name);
END;
GO

-- ============================================================================
-- Stored Procedure 4: Calculate All Target Categories
-- ============================================================================
-- Description: Runs all three categorization procedures in the correct order
-- Order is important:
--   1. First calculate IsOldTarget (Yellow)
--   2. Then calculate IsMonthTarget (Purple) - depends on IsOldTarget
--   3. Finally calculate TargetDeal (Green) - depends on both above
-- ============================================================================

IF OBJECT_ID('dbo.SP_Calculate_AllTargetCategories', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_Calculate_AllTargetCategories;
GO

CREATE PROCEDURE dbo.SP_Calculate_AllTargetCategories
    @Name VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OldTargetCount INT;
    DECLARE @MonthTargetCount INT;
    DECLARE @TargetDealCount INT;

    -- Step 1: Calculate IsOldTarget (Yellow)
    EXEC dbo.SP_Calculate_IsOldTarget @Name = @Name;
    
    SELECT @OldTargetCount = COUNT(*) 
    FROM comprGOOGLAirline 
    WHERE IsOldTarget = 1 
        AND (@Name IS NULL OR Name = @Name);

    -- Step 2: Calculate IsMonthTarget (Purple)
    EXEC dbo.SP_Calculate_IsMonthTarget @Name = @Name;
    
    SELECT @MonthTargetCount = COUNT(*) 
    FROM comprGOOGLAirline 
    WHERE IsMonthTarget = 1 
        AND (@Name IS NULL OR Name = @Name);

    -- Step 3: Calculate TargetDeal (Green)
    EXEC dbo.SP_Calculate_TargetDeal @Name = @Name;
    
    SELECT @TargetDealCount = COUNT(*) 
    FROM comprGOOGLAirline 
    WHERE TargetDeal = 1 
        AND (@Name IS NULL OR Name = @Name);

    -- Return summary
    SELECT 
        @OldTargetCount as OldTargets_Yellow,
        @MonthTargetCount as MonthTargets_Purple,
        @TargetDealCount as TargetDeals_Green,
        (@OldTargetCount + @MonthTargetCount + @TargetDealCount) as TotalCategorized;
END;
GO

-- ============================================================================
-- Helper Query: View Target Categorization Results
-- ============================================================================
-- Use this query to view how records are categorized:
-- ============================================================================

/*
SELECT 
    id,
    [From],
    [To],
    Dates,
    MONTH(Dates) as Month,
    YEAR(Dates) as Year,
    Airline,
    Aircode,
    Cabin,
    Stops,
    New_price,
    Difference,
    IsTargetFound,
    IsOldTarget,
    IsMonthTarget,
    TargetDeal,
    CASE 
        WHEN TargetDeal = 1 THEN 'GREEN - Best Deal'
        WHEN IsMonthTarget = 1 THEN 'PURPLE - Month Target'
        WHEN IsOldTarget = 1 THEN 'YELLOW - Old Target'
        WHEN IsTargetFound = 1 THEN 'BLUE - Target'
        ELSE 'WHITE - Regular'
    END as Category_Color,
    Name
FROM comprGOOGLAirline
WHERE Name = 'YourNameHere' -- Replace with actual Name value
ORDER BY 
    [From], [To], Airline, Cabin, Stops, New_price;
*/

-- ============================================================================
-- Test Queries
-- ============================================================================

/*
-- Test 1: Execute all categorization procedures
EXEC dbo.SP_Calculate_AllTargetCategories @Name = 'YourNameHere';

-- Test 2: View results by category
SELECT 
    CASE 
        WHEN TargetDeal = 1 THEN 'GREEN - Best Deal'
        WHEN IsMonthTarget = 1 THEN 'PURPLE - Month Target'
        WHEN IsOldTarget = 1 THEN 'YELLOW - Old Target'
        WHEN IsTargetFound = 1 THEN 'BLUE - Target'
        ELSE 'WHITE - Regular'
    END as Category,
    COUNT(*) as RecordCount,
    AVG(New_price) as AvgPrice,
    MIN(New_price) as MinPrice,
    MAX(New_price) as MaxPrice
FROM comprGOOGLAirline
WHERE Name = 'YourNameHere'
GROUP BY 
    CASE 
        WHEN TargetDeal = 1 THEN 'GREEN - Best Deal'
        WHEN IsMonthTarget = 1 THEN 'PURPLE - Month Target'
        WHEN IsOldTarget = 1 THEN 'YELLOW - Old Target'
        WHEN IsTargetFound = 1 THEN 'BLUE - Target'
        ELSE 'WHITE - Regular'
    END
ORDER BY 
    CASE 
        WHEN TargetDeal = 1 THEN 1
        WHEN IsMonthTarget = 1 THEN 2
        WHEN IsOldTarget = 1 THEN 3
        WHEN IsTargetFound = 1 THEN 4
        ELSE 5
    END;

-- Test 3: View target deals grouped by route
SELECT 
    [From],
    [To],
    Airline,
    Cabin,
    COUNT(*) as DealCount,
    MIN(New_price) as BestPrice
FROM comprGOOGLAirline
WHERE Name = 'YourNameHere'
    AND TargetDeal = 1
GROUP BY [From], [To], Airline, Cabin
ORDER BY BestPrice;
*/

-- ============================================================================
-- Monthly Analysis Query
-- ============================================================================

/*
SELECT 
    DATENAME(MONTH, Dates) as MonthName,
    MONTH(Dates) as MonthNumber,
    YEAR(Dates) as Year,
    COUNT(*) as TotalRecords,
    SUM(CASE WHEN IsOldTarget = 1 THEN 1 ELSE 0 END) as YellowCount,
    SUM(CASE WHEN IsMonthTarget = 1 THEN 1 ELSE 0 END) as PurpleCount,
    SUM(CASE WHEN TargetDeal = 1 THEN 1 ELSE 0 END) as GreenCount,
    AVG(New_price) as AvgPrice,
    MIN(New_price) as MinPrice
FROM comprGOOGLAirline
WHERE Name = 'YourNameHere'
GROUP BY DATENAME(MONTH, Dates), MONTH(Dates), YEAR(Dates)
ORDER BY YEAR(Dates), MONTH(Dates);
*/





