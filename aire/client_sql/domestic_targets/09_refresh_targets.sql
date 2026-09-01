
-- ============================================================
-- PART C - REFRESH THE FLAGS FOR THE DATA THAT IS ALREADY LOADED
-- ============================================================
-- Part A changed Difference for the "new deal" rows on the Airline side,
-- and Part B added three new flags on the domestic side, so both target
-- procedures have to be re-run once. From now on they run automatically
-- at the end of each upload (Finish button).
EXEC dbo.UpdateIsFoundStatusForGFAirline;

EXEC dbo.UpdateIsFoundStatusForGFDomesticAirline;
