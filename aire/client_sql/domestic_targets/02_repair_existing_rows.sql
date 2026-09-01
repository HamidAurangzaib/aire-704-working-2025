
-- One-off repair of the rows that are already wrong in the table (139,996 of
-- them), so you do not have to wait for the next upload.
UPDATE dbo.comprGOOGLAirline
SET [Difference] = 0
WHERE ISNULL(Olde_price, 0) = 0
  AND [Difference] <> 0;

