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
                //cn.ConnectionString = "Data Source=ALEEHYDER\\SQLEXPRESS; Database=DB_A61545_andycom;Integrated Security=true;";
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
    }
}
