using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace aire
{
    /// <summary>
    /// Helper class for managing target dates split by months
    /// Provides methods to extract and organize target dates by month
    /// </summary>
    public class ClassTargetDateMonths
    {
        /// <summary>
        /// Gets all target dates grouped by month for a specific name
        /// Returns dictionary with month names as keys and lists of dates as values
        /// </summary>
        public static Dictionary<string, List<DateTime>> GetTargetDatesByMonth(SqlConnection connection, string name)
        {
            var result = new Dictionary<string, List<DateTime>>();
            
            string query = @"
                SELECT DISTINCT Dates
                FROM comprGOOGLAirline
                WHERE Name = @Name
                    AND (IsTargetFound = 1 OR IsOldTarget = 1 OR IsMonthTarget = 1 OR TargetDeal = 1)
                    AND Dates IS NOT NULL
                ORDER BY Dates";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = reader.GetDateTime(0);
                        string monthKey = date.ToString("MMMM yyyy");
                        
                        if (!result.ContainsKey(monthKey))
                        {
                            result[monthKey] = new List<DateTime>();
                        }
                        result[monthKey].Add(date);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets target dates for a specific month
        /// </summary>
        public static List<DateTime> GetTargetDatesForMonth(SqlConnection connection, string name, int month, int year)
        {
            var dates = new List<DateTime>();
            
            string query = @"
                SELECT DISTINCT Dates
                FROM comprGOOGLAirline
                WHERE Name = @Name
                    AND (IsTargetFound = 1 OR IsOldTarget = 1 OR IsMonthTarget = 1 OR TargetDeal = 1)
                    AND Dates IS NOT NULL
                    AND MONTH(Dates) = @Month
                    AND YEAR(Dates) = @Year
                ORDER BY Dates";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dates.Add(reader.GetDateTime(0));
                    }
                }
            }

            return dates;
        }

        /// <summary>
        /// Gets target dates for January
        /// </summary>
        public static List<DateTime> GetTargetDatesJanuary(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 1, year);
        }

        /// <summary>
        /// Gets target dates for February
        /// </summary>
        public static List<DateTime> GetTargetDatesFebruary(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 2, year);
        }

        /// <summary>
        /// Gets target dates for March
        /// </summary>
        public static List<DateTime> GetTargetDatesMarch(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 3, year);
        }

        /// <summary>
        /// Gets target dates for April
        /// </summary>
        public static List<DateTime> GetTargetDatesApril(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 4, year);
        }

        /// <summary>
        /// Gets target dates for May
        /// </summary>
        public static List<DateTime> GetTargetDatesMay(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 5, year);
        }

        /// <summary>
        /// Gets target dates for June
        /// </summary>
        public static List<DateTime> GetTargetDatesJune(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 6, year);
        }

        /// <summary>
        /// Gets target dates for July
        /// </summary>
        public static List<DateTime> GetTargetDatesJuly(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 7, year);
        }

        /// <summary>
        /// Gets target dates for August
        /// </summary>
        public static List<DateTime> GetTargetDatesAugust(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 8, year);
        }

        /// <summary>
        /// Gets target dates for September
        /// </summary>
        public static List<DateTime> GetTargetDatesSeptember(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 9, year);
        }

        /// <summary>
        /// Gets target dates for October
        /// </summary>
        public static List<DateTime> GetTargetDatesOctober(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 10, year);
        }

        /// <summary>
        /// Gets target dates for November
        /// </summary>
        public static List<DateTime> GetTargetDatesNovember(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 11, year);
        }

        /// <summary>
        /// Gets target dates for December
        /// </summary>
        public static List<DateTime> GetTargetDatesDecember(SqlConnection connection, string name, int year)
        {
            return GetTargetDatesForMonth(connection, name, 12, year);
        }

        /// <summary>
        /// Gets a summary of target dates by month for a specific name
        /// Returns a dictionary with month names and counts
        /// </summary>
        public static Dictionary<string, int> GetTargetDatesSummaryByMonth(SqlConnection connection, string name)
        {
            var summary = new Dictionary<string, int>();
            
            string query = @"
                SELECT 
                    DATENAME(MONTH, Dates) + ' ' + CAST(YEAR(Dates) AS VARCHAR) as MonthYear,
                    COUNT(DISTINCT Dates) as DateCount
                FROM comprGOOGLAirline
                WHERE Name = @Name
                    AND (IsTargetFound = 1 OR IsOldTarget = 1 OR IsMonthTarget = 1 OR TargetDeal = 1)
                    AND Dates IS NOT NULL
                GROUP BY DATENAME(MONTH, Dates), MONTH(Dates), YEAR(Dates)
                ORDER BY YEAR(Dates), MONTH(Dates)";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string monthYear = reader.GetString(0);
                        int count = reader.GetInt32(1);
                        summary[monthYear] = count;
                    }
                }
            }

            return summary;
        }

        /// <summary>
        /// Gets all unique years that have target dates
        /// </summary>
        public static List<int> GetTargetDateYears(SqlConnection connection, string name)
        {
            var years = new List<int>();
            
            string query = @"
                SELECT DISTINCT YEAR(Dates) as Year
                FROM comprGOOGLAirline
                WHERE Name = @Name
                    AND (IsTargetFound = 1 OR IsOldTarget = 1 OR IsMonthTarget = 1 OR TargetDeal = 1)
                    AND Dates IS NOT NULL
                ORDER BY YEAR(Dates)";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        years.Add(reader.GetInt32(0));
                    }
                }
            }

            return years;
        }
    }
}




