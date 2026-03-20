-- ============================================================
-- fix_cheapest_dateprice.sql
-- Run in SSMS against DB_A61545_andycom
-- ============================================================
-- Fixes three issues in comprGOOGLAirline:
--
-- 1. Cheapest  — never store 0 as cheapest.
--               New-only rows (Olde_price=0) → New_price.
--               Price dropped to new all-time low  → New_price.
--               New_price = 0 (missing data)       → keep Olde_price or existing Cheapest.
--               Otherwise                          → keep existing Cheapest.
--
-- 2. DateNewPriceChanged — update to NewUploadDate whenever
--               price CHANGES (up or down, Difference <> 0).
--               Keep existing date only when Difference = 0 (no change).
--               This means: 15-mar file → 19-mar file with same price → keeps 15-mar.
--               19-mar file with different price (any direction) → shows 19-mar.
--
-- 3. NEW PRICE = 0 rows — excluded from grid display in C# (see GoogleAirline.cs).
-- ============================================================

-- -------------------------------------------------------
-- PART A: Fix existing data in the table right now
-- -------------------------------------------------------

-- Fix Cheapest (never allow 0)
UPDATE comprGOOGLAirline
SET Cheapest =
    CASE
        WHEN New_price = 0 AND Cheapest = 0 THEN Olde_price        -- bad row: fall back to old price
        WHEN New_price = 0                  THEN Cheapest           -- bad row: keep existing cheapest
        WHEN Olde_price = 0                 THEN New_price          -- new-only date: use new price
        WHEN New_price < Cheapest OR Cheapest = 0 THEN New_price   -- new all-time low
        ELSE Cheapest                                                -- no change: keep stored
    END;

-- Fix DateNewPriceChanged: update when price changed (either direction), keep when same
UPDATE comprGOOGLAirline
SET DateNewPriceChanged =
    CASE
        WHEN New_price <> Olde_price AND New_price > 0 THEN NewUploadDate          -- price changed → stamp date
        ELSE ISNULL(DateNewPriceChanged, NewUploadDate)                             -- no change   → keep old date
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

    -- Cheapest: all-time lowest New_price for this row — never store 0
    --   · New_price = 0 (missing data)   → keep existing Cheapest (or fall back to Olde_price)
    --   · new-only rows (Olde_price = 0) → New_price
    --   · New_price is a new low          → New_price
    --   · otherwise                       → keep existing
    UPDATE comprGOOGLAirline
    SET Cheapest =
        CASE
            WHEN New_price = 0 AND Cheapest = 0 THEN Olde_price
            WHEN New_price = 0                  THEN Cheapest
            WHEN Olde_price = 0                 THEN New_price
            WHEN New_price < Cheapest OR Cheapest = 0 THEN New_price
            ELSE Cheapest
        END;

    -- DateNewPriceChanged: stamp NewUploadDate whenever price changed (up OR down).
    -- Keep the previous date only when Difference = 0 (price identical to old file).
    UPDATE comprGOOGLAirline
    SET DateNewPriceChanged =
        CASE
            WHEN New_price <> Olde_price AND New_price > 0 THEN NewUploadDate
            ELSE ISNULL(DateNewPriceChanged, NewUploadDate)
        END;
END;

GO

-- -------------------------------------------------------
-- PART C: Verify — spot-check
-- -------------------------------------------------------
SELECT TOP 20
    [From], [To], Airline, Dates, Cabin, [Days],
    Olde_price, New_price, [Difference],
    Cheapest,
    DateNewPriceChanged,
    NewUploadDate
FROM comprGOOGLAirline
WHERE New_price > 0
ORDER BY NewUploadDate DESC, [From], [To], Dates;
