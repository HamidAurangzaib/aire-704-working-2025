using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aire
{
    public partial class holidays : Form
    {
        public holidays()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            upload_holiday u = new upload_holiday("easyjet");
            u.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            upload_holiday u = new upload_holiday("JET2HOLIDAYS");
            u.Show();
        }

        private void holidays_Load(object sender, EventArgs e)
        {

        }
        ado d = new ado();
        int cnt;
        private void button3_Click(object sender, EventArgs e)
        {
            d.connecter();
            d.da = new SqlDataAdapter("select * from googleDays where(Dates<>In_Date)", d.cn);
            d.da.Fill(d.ds, "days14");

            for(int j=0;j<=d.ds.Tables["days14"].Rows.Count;j++)
            {
                for (int i = 0; i <= d.ds.Tables["days14"].Rows.Count; i++)
                {
                    if (DateTime.Parse(d.ds.Tables["days14"].Rows[i][3].ToString())> DateTime.Parse(d.ds.Tables["days14"].Rows[j][10].ToString()))
                    {

                    }
                }
            }
        }
    }
}
