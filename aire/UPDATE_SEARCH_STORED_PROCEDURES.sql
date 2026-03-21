-- ============================================================================
-- SQL Script to Update Search Stored Procedures to Include Target Categorization Columns
-- ============================================================================
-- This script updates the search stored procedures to include the new columns:
-- IsOldTarget, IsMonthTarget, TargetDeal
-- 
-- IMPORTANT: You need to modify your existing stored procedures to include these columns
-- in their SELECT statements. This is a template showing what needs to be added.
-- ============================================================================

USE DB_A61545_andycom;
GO

-- ============================================================================
-- Example: How to update your stored procedures
-- ============================================================================
-- You need to add these columns to the SELECT statement in your stored procedures:
-- 
-- serchFromToMultiGroupCityGOOGleAirlineEverywhere
-- serchFromMultiGroupCityGOOGleAirlineEverywhere
-- serchToMultiGroupCityGOOGleAirlineEverywhere
-- serchWithoutFromToGOOGleAirline
-- 
-- Add these columns to the SELECT statement:
-- IsOldTarget, IsMonthTarget, TargetDeal
-- ============================================================================

-- Example template for updating a stored procedure:
/*
IF OBJECT_ID('dbo.serchFromToMultiGroupCityGOOGleAirlineEverywhere', 'P') IS NOT NULL
    DROP PROCEDURE dbo.serchFromToMultiGroupCityGOOGleAirlineEverywhere;
GO

CREATE PROCEDURE dbo.serchFromToMultiGroupCityGOOGleAirlineEverywhere
    @From VARCHAR(200) = NULL,
    @To VARCHAR(200) = NULL,
    @IsTargetOnly BIT = 0,
    @Airline VARCHAR(200) = NULL,
    @Aircode VARCHAR(50) = NULL,
    @Days VARCHAR(10) = NULL,
    @Cabin VARCHAR(100) = NULL,
    @Shortstays BIT = 0,
    @Fromdate DATE = NULL,
    @Todate DATE = NULL,
    @IsBetween BIT = 0,
    @IsGreater BIT = 0,
    @IsLess BIT = 0,
    @MinPrice FLOAT = NULL,
    @MaxPrice FLOAT = NULL,
    @Stops VARCHAR(10) = NULL,
    @EverywhereFrom BIT = 0,
    @EverywhereTo BIT = 0,
    @GreenDiff BIT = 0,
    @RedDiff BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Your existing WHERE clause logic here...
    -- Then in your SELECT statement, make sure to include:
    
    SELECT 
        id,
        [From],
        [To],
        citys,
        Dates,
        Olde_price,
        New_price,
        Difference,
        Cheapest,
        Airline,
        Aircode,
        Cabin,
        Days,
        Stops,
        IsTargetFound,
        NewUploadDate,
        web,
        Name,
        OtaDiscount,
        OtaTotal,
        -- ADD THESE THREE COLUMNS:
        IsOldTarget,      -- NEW: Yellow categorization
        IsMonthTarget,   -- NEW: Purple categorization
        TargetDeal       -- NEW: Green categorization
    FROM comprGOOGLAirline
    WHERE -- Your existing WHERE conditions...
END;
GO
*/

-- ============================================================================
-- Quick Check: Verify columns exist in your stored procedures
-- ============================================================================
-- Run this query to check if your stored procedures return the new columns:
/*
SELECT 
    OBJECT_NAME(c.object_id) AS ProcedureName,
    c.name AS ColumnName
FROM sys.dm_exec_describe_first_result_set(
    'EXEC serchFromToMultiGroupCityGOOGleAirlineEverywhere', 
    NULL, 
    0
) c
WHERE c.name IN ('IsOldTarget', 'IsMonthTarget', 'TargetDeal');
*/

PRINT '';
PRINT '========================================';
PRINT 'IMPORTANT INSTRUCTIONS:';
PRINT '========================================';
PRINT '1. Open each of your search stored procedures in SSMS';
PRINT '2. Find the SELECT statement';
PRINT '3. Add these three columns to the SELECT list:';
PRINT '   - IsOldTarget';
PRINT '   - IsMonthTarget';
PRINT '   - TargetDeal';
PRINT '4. Save and execute the updated stored procedures';
PRINT '';
PRINT 'The stored procedures to update are:';
PRINT '  - serchFromToMultiGroupCityGOOGleAirlineEverywhere';
PRINT '  - serchFromMultiGroupCityGOOGleAirlineEverywhere';
PRINT '  - serchToMultiGroupCityGOOGleAirlineEverywhere';
PRINT '  - serchWithoutFromToGOOGleAirline';
PRINT '';
PRINT 'After updating, the search results will show color coding:';
PRINT '  - GREEN = TargetDeal (Best deals)';
PRINT '  - PURPLE = IsMonthTarget (Month-specific targets)';
PRINT '  - YELLOW = IsOldTarget (Difference -5 to 0)';
PRINT '  - BLUE = IsTargetFound (Regular targets)';
PRINT '========================================';
GO

