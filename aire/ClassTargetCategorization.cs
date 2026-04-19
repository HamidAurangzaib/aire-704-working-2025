using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace aire
{
    /// <summary>
    /// Helper class for categorizing flight deals into different target types
    /// - IsOldTarget (Yellow): Difference between -5 and 0
    /// - IsMonthTarget (Purple): Blue targets with different months than yellows
    /// - TargetDeal (Green): Cheapest New_price among all categories
    /// </summary>
    public class ClassTargetCategorization
    {
        /// <summary>
        /// Categorizes records as IsOldTarget (Yellow) based on difference criteria
        /// Difference must be between -5 and 0 (inclusive): 0, -1, -2, -3, -4, -5
        /// </summary>
        public static void CalculateIsOldTarget(SqlConnection connection, string name)
        {
            string query = @"
                UPDATE comprGOOGLAirline
                SET IsOldTarget = 
                    CASE 
                        WHEN Difference >= -5 AND Difference <= 0 
                        THEN 1 
                        ELSE 0 
                    END
                WHERE Name = @Name";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Categorizes records as IsMonthTarget (Purple)
        /// Logic: If blue (IsTargetFound=1) and yellow (IsOldTarget=1) records exist with:
        /// - Same From, To, Airline, Stops, Cabin
        /// - Different months
        /// AND no price (Olde_price OR New_price) in the same month is cheaper than the blue row's New_price
        /// Then the blue records become purple (IsMonthTarget=1)
        /// If a cheaper price exists in the same month (including RED records or new-only records with Old=0),
        /// the blue row stays IsTargetFound only.
        /// </summary>
        public static void CalculateIsMonthTarget(SqlConnection connection, string name)
        {
            string query = @"
                -- Reset IsMonthTarget
                UPDATE comprGOOGLAirline
                SET IsMonthTarget = 0
                WHERE Name = @Name;

                -- Set IsMonthTarget for blue records with different months than yellows,
                -- but only if no price (Olde_price OR New_price) cheaper than this row's New_price
                -- exists in the same month. This covers:
                --   1) RED records (Difference > 0) where New_price is still cheaper
                --   2) New-only records (Olde_price=0) where New_price is cheaper
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
                    )
                    AND NOT EXISTS (
                        SELECT 1
                        FROM comprGOOGLAirline sm
                        WHERE sm.Name = @Name
                            AND blue.[From]  = sm.[From]
                            AND blue.[To]    = sm.[To]
                            AND blue.Airline = sm.Airline
                            AND blue.Stops   = sm.Stops
                            AND blue.Cabin   = sm.Cabin
                            AND MONTH(sm.Dates) = MONTH(blue.Dates)
                            AND YEAR(sm.Dates)  = YEAR(blue.Dates)
                            AND (
                                (sm.Olde_price > 0 AND sm.Olde_price < blue.New_price)
                                OR (sm.New_price > 0 AND sm.New_price < blue.New_price)
                            )
                    )";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Categorizes records as TargetDeal (Green)
        /// Logic: Blue (IsTargetFound=1) records where New_price is cheaper than:
        /// - All yellow (IsOldTarget=1) records with same From, To, Airline, Stops, Cabin, Aircode
        /// - All purple (IsMonthTarget=1) records with same criteria
        /// - All other blue records (IsTargetFound=1) with same criteria
        /// Only blue records that are cheaper than ALL of these become Green (TargetDeal)
        /// </summary>
        public static void CalculateTargetDeal(SqlConnection connection, string name)
        {
            string query = @"
                -- Reset IsTargetDeal
                UPDATE comprGOOGLAirline
                SET IsTargetDeal = 0
                WHERE Name = @Name;

                -- Set IsTargetDeal for blue records that are cheaper than yellow, purple, and other blue records
                UPDATE blue
                SET blue.IsTargetDeal = 1
                FROM comprGOOGLAirline blue
                WHERE blue.Name = @Name
                    AND blue.IsTargetFound = 1
                    AND blue.IsOldTarget = 0
                    AND blue.IsMonthTarget = 0
                    AND blue.New_price > 0
                    -- Must be cheaper than all yellow records with same route (strictly less than)
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
                    -- Must be cheaper than all purple records with same route (strictly less than)
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
                    )";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Resets IsMonthTarget (Purple) for rows that share the same month/route as a TargetDeal (Green).
        /// If a TargetDeal already exists in a month, no other row in that month needs to be purple —
        /// the TargetDeal IS the cheapest for that month, so others revert to IsTargetFound (Blue).
        /// </summary>
        public static void ResetMonthTargetWhenTargetDealExists(SqlConnection connection, string name)
        {
            string query = @"
                UPDATE blue
                SET blue.IsMonthTarget = 0
                FROM comprGOOGLAirline blue
                WHERE blue.Name = @Name
                    AND blue.IsMonthTarget = 1
                    AND EXISTS (
                        SELECT 1
                        FROM comprGOOGLAirline green
                        WHERE green.Name = @Name
                            AND green.IsTargetDeal = 1
                            AND blue.[From]   = green.[From]
                            AND blue.[To]     = green.[To]
                            AND blue.Airline  = green.Airline
                            AND blue.Stops    = green.Stops
                            AND blue.Cabin    = green.Cabin
                            AND MONTH(blue.Dates) = MONTH(green.Dates)
                            AND YEAR(blue.Dates)  = YEAR(green.Dates)
                    )";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Categorizes records as IsTargetDealOld (Orange)
        /// Logic: Records that are currently IsOldTarget (Yellow) but were IsTargetDeal (Green)
        /// on a previous upload date for the same route/date/airline/stops/cabin/aircode,
        /// AND are still the cheapest — i.e. no other record has a lower New_price for the same route.
        /// This persists upload-to-upload until the price changes (which moves it back to blue or red).
        /// </summary>
        public static void CalculateIsTargetDealOld(SqlConnection connection, string name)
        {
            string query = @"
                -- Reset IsTargetDealOld
                UPDATE comprGOOGLAirline
                SET IsTargetDealOld = 0
                WHERE Name = @Name;

                -- Set IsTargetDealOld: was Green on a prior upload, price unchanged (IsOldTarget=1),
                -- and still the cheapest for this route
                UPDATE curr
                SET curr.IsTargetDealOld = 1
                FROM comprGOOGLAirline curr
                WHERE curr.Name = @Name
                    AND curr.IsOldTarget = 1
                    AND curr.New_price > 0
                    -- Was previously IsTargetDeal (Green) for same route + flight date
                    AND EXISTS (
                        SELECT 1
                        FROM comprGOOGLAirline prev
                        WHERE prev.Name         = @Name
                            AND prev.IsTargetDeal  = 1
                            AND prev.[From]        = curr.[From]
                            AND prev.[To]          = curr.[To]
                            AND prev.Airline       = curr.Airline
                            AND prev.Stops         = curr.Stops
                            AND prev.Cabin         = curr.Cabin
                            AND prev.Aircode       = curr.Aircode
                            AND prev.Dates         = curr.Dates
                            AND prev.NewUploadDate < curr.NewUploadDate
                    )
                    -- Still the cheapest: no other record for same route has a lower New_price
                    AND NOT EXISTS (
                        SELECT 1
                        FROM comprGOOGLAirline cheaper
                        WHERE cheaper.Name      = @Name
                            AND cheaper.[From]  = curr.[From]
                            AND cheaper.[To]    = curr.[To]
                            AND cheaper.Airline = curr.Airline
                            AND cheaper.Stops   = curr.Stops
                            AND cheaper.Cabin   = curr.Cabin
                            AND cheaper.Aircode = curr.Aircode
                            AND cheaper.New_price > 0
                            AND cheaper.New_price < curr.New_price
                            AND cheaper.id <> curr.id
                    )";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Runs all target categorization calculations in order
        /// </summary>
        public static void CalculateAllTargetCategories(SqlConnection connection, string name)
        {
            // Step 1: Calculate IsOldTarget (Yellow)
            CalculateIsOldTarget(connection, name);

            // Step 2: Calculate IsMonthTarget (Purple)
            CalculateIsMonthTarget(connection, name);

            // Step 3: Calculate IsTargetDeal (Green)
            CalculateTargetDeal(connection, name);

            // Step 4: Remove purple from months that already have a IsTargetDeal (Green).
            //         Those rows revert to IsTargetFound (Blue) — the IsTargetDeal is the cheapest.
            ResetMonthTargetWhenTargetDealExists(connection, name);

            // Step 5: Calculate IsTargetDealOld (Orange) — was Green, same price, still cheapest
            CalculateIsTargetDealOld(connection, name);
        }

        /// <summary>
        /// Gets the month name from a date for display purposes
        /// </summary>
        public static string GetMonthName(DateTime date)
        {
            return date.ToString("MMMM");
        }

        /// <summary>
        /// Groups target dates by month for analysis
        /// </summary>
        public static Dictionary<string, List<DateTime>> GroupTargetDatesByMonth(List<DateTime> targetDates)
        {
            var groupedDates = new Dictionary<string, List<DateTime>>();

            foreach (var date in targetDates)
            {
                string monthYear = date.ToString("MMMM yyyy");
                if (!groupedDates.ContainsKey(monthYear))
                {
                    groupedDates[monthYear] = new List<DateTime>();
                }
                groupedDates[monthYear].Add(date);
            }

            return groupedDates;
        }

        /// <summary>
        /// Gets the category name based on flags
        /// </summary>
        public static string GetCategoryName(bool isOldTarget, bool isMonthTarget, bool targetDeal, bool isTargetFound, bool isTargetDealOld = false)
        {
            if (targetDeal) return "TargetDeal (Green)";
            if (isTargetDealOld) return "TargetDealOld (Orange)";
            if (isMonthTarget) return "MonthTarget (Purple)";
            if (isOldTarget) return "OldTarget (Yellow)";
            if (isTargetFound) return "Target (Blue)";
            return "Regular";
        }

        /// <summary>
        /// Gets the appropriate color for a record based on its category
        /// </summary>
        public static System.Drawing.Color GetCategoryColor(bool isOldTarget, bool isMonthTarget, bool targetDeal, bool isTargetFound, bool isTargetDealOld = false)
        {
            if (targetDeal) return System.Drawing.Color.LightGreen;       // Green: best deal
            if (isTargetDealOld) return System.Drawing.Color.Orange;      // Orange: was Green, same price
            if (isMonthTarget) return System.Drawing.Color.MediumPurple;  // Purple: month targets
            if (isOldTarget) return System.Drawing.Color.Yellow;          // Yellow: old targets
            if (isTargetFound) return System.Drawing.Color.LightBlue;     // Blue: regular targets
            return System.Drawing.Color.White;
        }
    }
}



