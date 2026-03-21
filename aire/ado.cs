using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;


namespace aire
{
    class ado
    {
        public SqlConnection cn = new SqlConnection();
        public SqlCommand cmdd = new SqlCommand();
        public DataTable dt = new DataTable();
        public SqlDataReader dr;
        public DataTable dt1 = new DataTable();
        public DataTable dt2 = new DataTable();
        public SqlDataAdapter da;
        public SqlCommandBuilder cmd;
     
        //public SqlCommandBuilder cmd;
        public DataSet ds= new DataSet();
        public DataView dview;


        public void connecter2()
        {
            if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
            {
                //cn.ConnectionString = "Data Source=ALEEHYDER\\SQLEXPRESS; Database=DB_A61545_andycom;Integrated Security=true;";
                cn.ConnectionString = "Data Source=SQL8010.site4now.net;Initial Catalog=db_a61545_bobs;User Id=db_a61545_bobs_admin;Password=b0bsfl1gh7;";
                cn.Open();
            }
        }

        public void deconnecter2()
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public void connecter()
        {
            if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken) 
            {
                // Remote SQL Server connection (production database)
                cn.ConnectionString = "Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;";
                cn.Open();
            }
        }

        public void deconnecter()
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        // Method: Connect to local SQL Server (optional - for testing)
        public void connecterLocal()
        {
            if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
            {
                // Local SQL Server connection - DESKTOP-EMUHCLA\SQL2022
                cn.ConnectionString = "Data Source=DESKTOP-EMUHCLA\\SQL2022;Initial Catalog=DB_A61545_andycom;Integrated Security=True;";
                cn.Open();
            }
        }

        // Method: Connect to remote SQL Server
        public void connecterRemote()
        {
            if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
            {
                cn.ConnectionString = "Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;";
                cn.Open();
            }
        }

        // Helper method: Safely execute stored procedure (optional - won't crash if missing)
        public bool ExecuteStoredProcedureSafe(string procedureName, bool showErrors = false)
        {
            try
            {
                cmdd.Parameters.Clear();
                cmdd.CommandType = CommandType.StoredProcedure;
                cmdd.CommandText = procedureName;
                cmdd.Connection = cn;
                cmdd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                // Stored procedure doesn't exist - this is OK for new/local databases
                if (showErrors)
                {
                    System.Diagnostics.Debug.WriteLine($"Stored procedure '{procedureName}' not found: {ex.Message}");
                }
                return false;
            }
        }

        // Helper method: Safely execute stored procedure with DataReader (optional)
        public SqlDataReader ExecuteStoredProcedureReaderSafe(string procedureName, bool showErrors = false)
        {
            try
            {
                cmdd.Parameters.Clear();
                cmdd.CommandType = CommandType.StoredProcedure;
                cmdd.CommandText = procedureName;
                cmdd.Connection = cn;
                return cmdd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                // Stored procedure doesn't exist - return null
                if (showErrors)
                {
                    System.Diagnostics.Debug.WriteLine($"Stored procedure '{procedureName}' not found: {ex.Message}");
                }
                return null;
            }
        }

        // Helper method: Safely execute stored procedure that returns DataTable (optional)
        public DataTable ExecuteStoredProcedureDataTableSafe(string procedureName, bool showErrors = false)
        {
            DataTable result = new DataTable();
            try
            {
                cmdd.Parameters.Clear();
                cmdd.CommandType = CommandType.StoredProcedure;
                cmdd.CommandText = procedureName;
                cmdd.Connection = cn;
                using (SqlDataReader reader = cmdd.ExecuteReader())
                {
                    result.Load(reader);
                }
                return result;
            }
            catch (SqlException ex)
            {
                // Stored procedure doesn't exist - return empty table
                if (showErrors)
                {
                    System.Diagnostics.Debug.WriteLine($"Stored procedure '{procedureName}' not found: {ex.Message}");
                }
                return result; // Return empty table
            }
        }
    }
}
