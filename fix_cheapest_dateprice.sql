-- ============================================================
-- fix_cheapest_dateprice.sql
-- Run in SSMS against DB_A61545_andycom
-- ============================================================
--
-- FIX 1: CHEAPEST — Reset to MIN(Olde_price, New_price) for every row.
--   The stored values were wrong/stale. Starting fresh:
--   · Both prices > 0  →  MIN(Olde_price, New_price)
--   · Only New_price   →  New_price
--   · Only Olde_price  →  Olde_price
--   Future uploads (in ALTER PROC below): only update if New_price
--   is strictly less than the stored Cheapest — otherwise leave as-is.
--   e.g. stored Cheapest = 652, New_price = 808 → stays 652.
--        stored Cheapest = 652, New_price = 538 → updates to 538.
--
-- FIX 2: DateNewPriceChanged
--   · New_price = Olde_price (Difference = 0) → keep existing date (e.g. 15-Mar)
--   · New_price ≠ Olde_price (any change, up or down) → NewUploadDate (e.g. 19-Mar)
-- ============================================================

-- -------------------------------------------------------
-- PART A: Fix existing data RIGHT NOW
-- -------------------------------------------------------

-- Reset Cheapest to the lower of the two prices for every row
UPDATE comprGOOGLAirline
SET Cheapest =
    CASE
        WHEN Olde_price > 0 AND New_price > 0
            THEN CASE WHEN Olde_price < New_price THEN Olde_price ELSE New_price END
        WHEN New_price  > 0 THEN New_price
        WHEN Olde_price > 0 THEN Olde_price
        ELSE 0
    END;

-- Fix DateNewPriceChanged
--   Difference = 0 → keep existing date
--   Difference ≠ 0 → stamp NewUploadDate
UPDATE comprGOOGLAirline
SET DateNewPriceChanged =
    CASE
        WHEN New_price = Olde_price THEN ISNULL(DateNewPriceChanged, NewUploadDate)
        ELSE NewUploadDate
    END;

GO

-- -------------------------------------------------------
-- PART B: ALTER upd_cmprgoogleAirline
--   Every future Finish upload will apply the same logic.
-- -------------------------------------------------------
ALTER PROCEDURE [dbo].[upd_cmprgoogleAirline]
AS
BEGIN
    -- 1. Difference
    UPDATE comprGOOGLAirline
    SET [Difference] = New_price - Olde_price;

    -- 2. Cheapest: only lower it, never raise it, never allow 0
    --    · New_price = 0 (missing)          → keep existing Cheapest
    --    · New_price < stored Cheapest       → update to New_price  (new all-time low)
    --    · Cheapest not yet set (= 0)        → seed with MIN(Olde_price, New_price)
    --    · Otherwise                         → leave Cheapest unchanged
    UPDATE comprGOOGLAirline
    SET Cheapest =
        CASE
            WHEN New_price = 0
                THEN CASE WHEN Cheapest > 0 THEN Cheapest ELSE Olde_price END
            WHEN Cheapest = 0
                THEN CASE WHEN Olde_price > 0 AND Olde_price < New_price THEN Olde_price ELSE New_price END
            WHEN New_price < Cheapest THEN New_price
            ELSE Cheapest
        END;

    -- 3. DateNewPriceChanged
    --    · Same price (Difference = 0) → keep the date from the last real change
    --    · Price changed (any direction) → stamp today's upload date
    UPDATE comprGOOGLAirline
    SET DateNewPriceChanged =
        CASE
            WHEN New_price = Olde_price THEN ISNULL(DateNewPriceChanged, NewUploadDate)
            ELSE NewUploadDate
        END;
END;

GO

-- -------------------------------------------------------
-- PART C: Verify — check the highlighted rows
-- -------------------------------------------------------
SELECT TOP 30
    [From], [To], Airline, Aircode, Dates, Cabin, [Days],
    Olde_price, New_price, [Difference],
    Cheapest,
    DateNewPriceChanged,
    NewUploadDate
FROM comprGOOGLAirline
ORDER BY [From], [To], Airline, Dates;
