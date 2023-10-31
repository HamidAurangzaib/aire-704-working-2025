using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace aire
{
    public partial class target : Form
    {
        ado d = new ado();
        public target()
        {
            InitializeComponent();
        }


        private void target_Load(object sender, EventArgs e)
        {
            d.connecter();
        }
        DataTable dt = new DataTable();
        public void searchFROMTO()
        {
            dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchfromtoallA";
            d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dt.Load(d.dr);
            DataView dv = new DataView(dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    dv[i][4].ToString(), (dv[i][5].ToString()));

            }


        }
        public void searchFROM()
        {
            dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchFromallA";
            d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dt.Load(d.dr);
            DataView dv = new DataView(dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()));
            }

        }

        public void searchTO()
        {
            dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchToAllTargetA";
            d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dt.Load(d.dr);
            DataView dv = new DataView(dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()));
            }

        }


        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "" )
            {
                searchFROMTO();

            }
            else if (textBox1.Text != "" && textBox2.Text == "" )
            {
                searchFROM();
              
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchTO();
                
            }
        }

        public void searchFROMTOB()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchfromtoallB";
            d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                  double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));

            }


        }
        public void searchFROMB()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchFromallB";
            d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                  double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
            }

        }

        public void searchTOB()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchToAllTargetB";
            d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                  double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                searchFROMTOB();

            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                searchFROMB();

            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchTOB();

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
       

                dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "serchAllTargetAdate";
                d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");

                d.cmdd.Connection = d.cn;
                d.dr = d.cmdd.ExecuteReader();
                dt.Load(d.dr);
                DataView dv = new DataView(dt);
                int cnt = dv.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()));
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            d.dt.Rows.Clear();
           
            dataGridView2.Rows.Clear();
            char[] c = { ',', '.' };

            string str = textBox5.Text;



            string[] tbl = str.Split(c);
            int cnt;
            cnt = tbl.Length;

            if (cnt > 7)
            {
                MessageBox.Show("The maximum is 7 codes");
            }
            else if (cnt < 6)
            {
                if (cnt == 1)
                {
                    string vr = tbl[0];

                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
                else if (cnt == 2)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB1";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;

                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
                else if (cnt == 3)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    string vr2 = tbl[0];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB2";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;

                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
                else if (cnt == 4)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    string vr2 = tbl[2];
                    string vr3 = tbl[3];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB3";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;
                    d.cmdd.Parameters.Add("@airlin3", SqlDbType.VarChar, 20).Value = vr3;

                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
                else if (cnt == 5)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    string vr2 = tbl[2];
                    string vr3 = tbl[3];
                    string vr4 = tbl[4];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB4";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;
                    d.cmdd.Parameters.Add("@airlin3", SqlDbType.VarChar, 20).Value = vr3;
                    d.cmdd.Parameters.Add("@airlin4", SqlDbType.VarChar, 20).Value = vr4;
                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
                else if (cnt == 6)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    string vr2 = tbl[2];
                    string vr3 = tbl[3];
                    string vr4 = tbl[4];
                    string vr5 = tbl[5];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB5";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;
                    d.cmdd.Parameters.Add("@airlin3", SqlDbType.VarChar, 20).Value = vr3;
                    d.cmdd.Parameters.Add("@airlin4", SqlDbType.VarChar, 20).Value = vr4;
                    d.cmdd.Parameters.Add("@airlin5", SqlDbType.VarChar, 20).Value = vr5;
                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
                else if (cnt == 7)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    string vr2 = tbl[2];
                    string vr3 = tbl[3];
                    string vr4 = tbl[4];
                    string vr5 = tbl[5];
                    string vr6 = tbl[6];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "airlineAllTargetB6";
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;
                    d.cmdd.Parameters.Add("@airlin3", SqlDbType.VarChar, 20).Value = vr3;
                    d.cmdd.Parameters.Add("@airlin4", SqlDbType.VarChar, 20).Value = vr4;
                    d.cmdd.Parameters.Add("@airlin5", SqlDbType.VarChar, 20).Value = vr5;
                    d.cmdd.Parameters.Add("@airlin6", SqlDbType.VarChar, 20).Value = vr6;
                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;

                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                          double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()));
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            dataGridView1.Rows.Clear();
            dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "selectA";
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dt.Load(d.dr);
            DataView dv = new DataView(dt);
            int cntd = dv.Count;

            for (int i = 0; i < cntd; i++)
            {

                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    dv[i][4].ToString(), dv[i][5].ToString());

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "selectB";
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cntd = dv.Count;

            for (int i = 0; i < cntd; i++)
            {
                dataGridView2.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(), dv[i][4].ToString(),
                  dv[i][5].ToString(),dv[i][6].ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            upload_target up = new upload_target();
            up.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            upload_gf_domestic_target up = new upload_gf_domestic_target();
            up.ShowDialog();
        }
    }
}
