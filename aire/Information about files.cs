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
    public partial class Information_about_files : Form
    {
        string c;
       
        public Information_about_files(string a)
        {
            InitializeComponent();
            c = a;
        }
        ado d = new ado();
        private void Information_about_files_Load(object sender, EventArgs e)
        {
            d.connecter();
            int cout;
         
            if (c =="2")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesGF]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "GF");
                d.dt = d.ds.Tables["GF"];
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {

                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                }
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesSKYS]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "SKY");
                d.dt = d.ds.Tables["SKY"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {

                    label8.Text = d.dt.Rows[0][1].ToString();
                    label9.Text = d.dt.Rows[0][2].ToString();
                    label11.Text = d.dt.Rows[1][1].ToString();
                    label12.Text = d.dt.Rows[1][2].ToString();

                }
            }
             if (c == "1")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesGF]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "GF");
                d.dt = d.ds.Tables["GF"];
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label4.Text = "";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                }
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesSKYS]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "SKY");
                d.dt = d.ds.Tables["SKY"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label10.Text = "";
                    label8.Text = d.dt.Rows[0][1].ToString();
                    label9.Text = d.dt.Rows[0][2].ToString();
                    label11.Text = d.dt.Rows[1][1].ToString();
                    label12.Text = d.dt.Rows[1][2].ToString();

                }
            }
             if (c == "3")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesTAX]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "TAX");
                d.dt = d.ds.Tables["TAX"];
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label4.Text = "";
                    label1.Text = "TAX";
                    label3.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label2.Text = d.dt.Rows[0][1].ToString();
                 

                }
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesFrs]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "Frs");
                d.dt = d.ds.Tables["Frs"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label7.Text = "Fares";
                    label10.Text = "";
                    label8.Text = d.dt.Rows[0][1].ToString();
                    label9.Text = d.dt.Rows[1][1].ToString();
                    label11.Text = "";
                    label12.Text = "";

                }
            }
             if(c=="4")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesGF]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "GF");
                d.dt = d.ds.Tables["GF"];
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {

                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label2.Visible = false;

                }
            }
             if(c=="5")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesSKYS]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "SKY");
                d.dt = d.ds.Tables["SKY"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label1.Text = "Skyscanner";
                    label8.Text = d.dt.Rows[0][1].ToString();
                    label9.Text = d.dt.Rows[0][2].ToString();
                    label11.Text = d.dt.Rows[1][1].ToString();
                    label12.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label2.Visible = false;

                }
            }
            if (c == "6")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from namefilesitx", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "ou");
                d.dt = d.ds.Tables["ou"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label1.Text = "ITX Airline comparison";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;


                }
            }
            if (c=="7")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from namefilesoutput", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "ou");
                d.dt = d.ds.Tables["ou"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label1.Text = "ITX calendar output";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                  

                }
            }
            if (c == "88")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from namefilesitx", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "ou");
                d.dt = d.ds.Tables["ou"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label1.Text = "ITX AIRLINE BOTH";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
              

                }
            }
            if (c == "8")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from namefilesitxallcabin", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "ou");
                d.dt = d.ds.Tables["ou"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label1.Text = "ITX AIRLINE BOTH";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;


                }
            }
            if (c == "9")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from namefilesquickSKYS", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "qu");
                d.dt = d.ds.Tables["qu"];
                cout = 0;
                cout = d.dt.Rows.Count;
                if (cout > 0)
                {
                    label1.Text = "quick skyscanner";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;


                }
            }
            if(c=="2347")
            {
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from [dbo].[namefilesGFCOPY]", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "GFC");
                d.dt = d.ds.Tables["GFC"];
                cout = d.dt.Rows.Count;
                MessageBox.Show(cout.ToString());
                if (cout > 0)
                {
                    label4.Text = "";
                    label2.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[0][2].ToString();
                    label5.Text = d.dt.Rows[1][1].ToString();
                    label6.Text = d.dt.Rows[1][2].ToString();

                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
