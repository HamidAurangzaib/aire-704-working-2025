-- ============================================================
-- fix_cmprGCOPY_newdates.sql
-- Run in SSMS against DB_A61545_andycom
-- Fix: domestic tab now shows new-only dates (not in old file)
--      with Olde_price=0, Difference=0, displayed as ORANGE.
-- ============================================================

-- Changed from INNER JOIN (comma syntax) to RIGHT JOIN so that
-- rows in googleFnewCOPY with no matching date in googlef1oldCOPY
-- are still inserted with Olde_price=0.
-- upd_cmprgoogleCOPY (already exists) sets Difference=0 for
-- these new-only rows, which triggers the ORANGE colour in the grid.

ALTER PROC [dbo].[cmprGCOPY]
    @Name VARCHAR(100)
AS
BEGIN
    SET QUERY_GOVERNOR_COST_LIMIT 90000;

    DELETE FROM comprGOOGLCOPY WHERE [Name] = @Name;

    INSERT INTO comprGOOGLCOPY
        ([From], [To], citys, Dates, Olde_price, New_price, [Difference],
         Cheapest, Airline, Aircode, Cabin, Stops, [Days], web, [Name], NewUploadDate)
    SELECT
        gn.[From], gn.[To], 'a', gn.Dates,
        ISNULL(gld.Montant, 0),        -- 0 when date is new-only
        gn.Montant,
        0,
        CASE WHEN ISNULL(gld.Montant, 0) = 0 THEN gn.Montant ELSE 0 END,
        gn.Airline, gn.Aircode, gn.Cabin, gn.Stops, gn.[Days],
        gn.web, gn.[Name], gn.NewUploadDate
    FROM googlef1oldCOPY gld
    RIGHT JOIN googleFnewCOPY gn
        ON  gn.[From]   = gld.[From]
        AND gn.[To]     = gld.[To]
        AND gn.Dates    = gld.Dates
        AND gn.[Days]   = gld.[Days]
        AND gn.Stops    = gld.Stops
        AND gn.Airline  = gld.Airline
        AND gn.Aircode  = gld.Aircode
        AND gld.[Name]  = @Name
    WHERE gn.[Name] = @Name;

    -- Remove any rows where both prices are 0 (bad data)
    DELETE FROM comprGOOGLCOPY WHERE Olde_price = 0 AND New_price = 0;
END

GO
