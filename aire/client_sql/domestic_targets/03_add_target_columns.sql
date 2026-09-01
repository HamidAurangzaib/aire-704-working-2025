

-- ============================================================
-- PART B - TARGET PRICES FOR COPY (DOMESTIC)
-- ============================================================
-- comprGOOGLCOPY already has the IsOldTarget / IsMonthTarget / IsTargetDeal
-- flag columns, but nothing was filling them and the search procs did not
-- return them. This is the domestic mirror of target_flags_changes.sql.
--
--   Blue   (IsTargetFound)   : New_price <= target AND Difference <= -5
--   Yellow (IsOldTarget)     : New_price <= target AND -5 < Difference <= 0
--   Purple (IsMonthTarget)   : Blue row that has a Yellow row in another month
--   Green  (IsTargetDeal)    : Blue row that is the cheapest of its group
--   Orange (IsTargetDealOld) : was Green on an earlier upload and still cheapest
--
-- This procedure only fills IsTargetFound / IsOldTarget from the target table
-- and clears the rest. The remaining four flags are worked out by the shared
-- ClassTargetCategorization code, which google_copy now calls with the
-- comprGOOGLCOPY table straight after this procedure - exactly the way the
-- Airline upload does it.
--
-- Note: comprGOOGLCOPY / tblGfDomesticTarget have no OtaDiscount / OtaTotal
-- columns, so the OTA part of the Airline procedure is not ported.
-- ------------------------------------------------------------

-- Safety net: create the flag columns if this database has not got them yet.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsOldTarget')
    ALTER TABLE comprGOOGLCOPY ADD IsOldTarget bit NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsMonthTarget')
    ALTER TABLE comprGOOGLCOPY ADD IsMonthTarget bit NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsTargetDeal')
    ALTER TABLE comprGOOGLCOPY ADD IsTargetDeal bit NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('comprGOOGLCOPY') AND name = 'IsTargetDealOld')
    ALTER TABLE comprGOOGLCOPY ADD IsTargetDealOld bit NULL;
