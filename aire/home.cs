using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace aire
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

      

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Serch s = new Serch();
            s.Show();
        }
  
        private void button6_Click(object sender, EventArgs e)
        {
            codeAirline c = new codeAirline();
            c.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            code_airline CD = new code_airline();
            CD.Show();
        }
        ado d = new ado();
        private void home_Load(object sender, EventArgs e)
        {
            UploadImage.Hide();
            d.connecter();
            
            // Execute cleanup procedures if they exist (optional)
            try
            {
                d.cmdd = new SqlCommand("exec dlltGF0", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Stored procedure doesn't exist - this is OK for new/local databases
                System.Diagnostics.Debug.WriteLine("dlltGF0 stored procedure not found: " + ex.Message);
            }

            try
            {
                d.cmdd = new SqlCommand("exec dlltitx0", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Stored procedure doesn't exist - this is OK for new/local databases
                System.Diagnostics.Debug.WriteLine("dlltitx0 stored procedure not found: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            groupcode gp = new groupcode();
            gp.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Srch_Hotel sh = new Srch_Hotel();
            sh.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            search_UK uk = new search_UK();
            uk.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Search_USA usa = new Search_USA();
            usa.Show();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            GF_Hotel g = new GF_Hotel();
            g.Show();
        }

        private void UploadImage_Click(object sender, EventArgs e)
        {
            ImageUpload up = new ImageUpload();
            up.Show();
        }
    }
}
