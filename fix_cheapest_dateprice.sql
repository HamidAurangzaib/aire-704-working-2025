-- ============================================================
-- fix_cheapest_dateprice.sql
-- Run in SSMS against DB_A61545_andycom
-- ============================================================
-- Fixes two bugs in comprGOOGLAirline:
--
-- 1. Cheapest  — must be the lower of (New_price) vs (stored Cheapest)
--               for that row, but ONLY when New_price < Olde_price.
--               If price has not dropped, Cheapest stays as-is.
--               New-only rows (Olde_price=0) always set Cheapest = New_price.
--
-- 2. DateNewPriceChanged — must only be updated to the upload date
--               when New_price < Olde_price (a real price drop occurred).
--               If price is unchanged or higher, keep the previous date.
-- ============================================================

-- -------------------------------------------------------
-- PART A: Fix existing data in the table right now
-- -------------------------------------------------------

-- Fix Cheapest
UPDATE comprGOOGLAirline
SET Cheapest =
    CASE
        WHEN Olde_price = 0                                  THEN New_price   -- new-only date
        WHEN New_price < Olde_price AND New_price < Cheapest THEN New_price   -- new all-time low
        WHEN Cheapest   = 0                                  THEN New_price   -- cheapest never set
        ELSE Cheapest                                                          -- keep existing
    END;

-- Fix DateNewPriceChanged
UPDATE comprGOOGLAirline
SET DateNewPriceChanged =
    CASE
        WHEN New_price < Olde_price THEN NewUploadDate          -- price dropped → record date
        ELSE ISNULL(DateNewPriceChanged, NewUploadDate)          -- no change    → keep old date
    END;

GO

-- -------------------------------------------------------
-- PART B: ALTER upd_cmprgoogleAirline so every future
--         upload applies the same correct logic
-- -------------------------------------------------------
ALTER PROCEDURE [dbo].[upd_cmprgoogleAirline]
AS
BEGIN
    -- Difference (positive = price up, negative = price down)
    UPDATE comprGOOGLAirline
    SET [Difference] = New_price - Olde_price;

    -- Cheapest: lowest New_price ever seen for this row
    --   · new-only rows (Olde_price=0)  → always New_price
    --   · price dropped below stored    → update to New_price
    --   · otherwise                     → keep existing
    UPDATE comprGOOGLAirline
    SET Cheapest =
        CASE
            WHEN Olde_price = 0                                  THEN New_price
            WHEN New_price < Olde_price AND New_price < Cheapest THEN New_price
            WHEN Cheapest   = 0                                  THEN New_price
            ELSE Cheapest
        END;

    -- DateNewPriceChanged: only stamp with upload date on a real price drop
    UPDATE comprGOOGLAirline
    SET DateNewPriceChanged =
        CASE
            WHEN New_price < Olde_price THEN NewUploadDate
            ELSE ISNULL(DateNewPriceChanged, NewUploadDate)
        END;
END;

GO

-- -------------------------------------------------------
-- PART C: Verify — spot-check the top rows
-- -------------------------------------------------------
SELECT TOP 20
    [From], [To], Airline, Dates, Cabin, [Days],
    Olde_price, New_price, [Difference],
    Cheapest,
    DateNewPriceChanged,
    NewUploadDate
FROM comprGOOGLAirline
ORDER BY NewUploadDate DESC, [From], [To], Dates;
