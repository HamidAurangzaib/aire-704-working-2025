using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using PagedList;

namespace aire
{
    public partial class airline : Form
    {
        ado d = new ado();
        private readonly SynchronizationContext synchronizationcontext;
        public airline()
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;
        }
        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellFormattingEventArgs e) 
        {
            

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells[7].Value) < 0)
                {
                    row.Cells[7].Style.BackColor = Color.LightGreen;
                }
                else if (Convert.ToInt32(row.Cells[7].Value) > 0)
                {

                    row.Cells[7].Style.BackColor = Color.Red;
                }
                if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[6].Value) > 0)
                {
                    row.Cells[7].Style.BackColor = Color.Orange;
                }
                if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0 && Convert.ToDouble(row.Cells[6].Value) == 0)
                {
                    row.Cells[7].Style.BackColor = Color.Gray;
                }

                for (int i = 0; i < dthtl.Rows.Count; i++)
                {
                    if (Convert.ToString(row.Cells[2].Value).Equals(dthtl.Rows[i][0].ToString()))
                    {
                        row.Cells[2].Style.BackColor = Color.YellowGreen;
                    }
                }
            }
        }
        public  void searchFROMTO()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchFROMTO";
            d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
           
                for (int i = 0; i < cnt; i++)
                {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                    dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                    double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                    dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                    double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
            }

        }
        public async void searchFROM()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchFROM";
            d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
            await Task.Run(() =>
            {
                for (int i = 0; i < cnt; i++)
                {
                    dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                        dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                        double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                        dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                        double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                }
            });
        }
        public async void searchTO()
        {
             d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serchTO";
            d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
           
                for (int i = 0; i < cnt; i++)
                {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                    dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                    double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                    dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                    double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
            }

        }
        private  void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
          
            if (textBox1.Text != "" && textBox2.Text != "")
        {
            

                searchFROMTO();


                dtgrdvwcolor();


            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {

                searchFROM();

                dtgrdvwcolor();


            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {

                searchTO();

                dtgrdvwcolor();

            }
        }
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        private void comb()
        {
            d.da = new SqlDataAdapter("select distinct [From] from t", d.cn);
            d.da.Fill(d.ds, "com1");
            d.da = new SqlDataAdapter("select distinct [To] from t", d.cn);
            d.da.Fill(ds1, "com2");
            d.da = new SqlDataAdapter("select distinct Airlin from t", d.cn);
            d.da.Fill(ds2, "com3");
            comboBox1.DataSource = d.ds.Tables["com1"];
            comboBox1.DisplayMember = "From";
            comboBox1.ValueMember = "From";

            comboBox3.DataSource = ds1.Tables["com2"];
            comboBox3.DisplayMember = "To";
            comboBox3.ValueMember = "To";

            comboBox2.DataSource = ds2.Tables["com3"];
            comboBox2.DisplayMember = "Airlin";
            comboBox2.ValueMember = "Airlin";
        }
        private async void airline_Load(object sender, EventArgs e)
        {
            d.connecter();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            await Task.Run(() =>
            {
                d.da = new SqlDataAdapter("select t1.[From],t1.Via,t1.[To],t1.citys,t1.Airlin,t1.Olde_price,t1.New_price,t1.[Difference],t1.Cheapest,t1.Total_Tax,t1.Total_amount,t1.Class,t1.Dates,t1.Cabin,t1.TAX1,t1.Tcode1,t1.TAX2,t1.Tcode2,t1.TAX3,t1.Tcode3,t1.Farebasis,t1.Season from t t1", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "t");
                d.dt = d.ds.Tables["t"];
            });
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 5000;
            trackBar1.TickStyle = TickStyle.Both;
            trackBar1.TickFrequency = 1;
            comb();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            dshtl.Clear();
            dthtl.Rows.Clear();

            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];

        }
        private  void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            d.dview = new DataView(d.dt);
            
            int mont = int.Parse(nbr.Text.ToString());
            if (mont > 0)
            {
                if (radioButton1.Checked==true)
                { d.dview.RowFilter = "New_price <=" + nbr.Text; }
                else if (radioButton2.Checked==true)
                { d.dview.RowFilter = "Difference <=" + double.Parse(nbr.Text); }

                int cnt = d.dview.Count;
                
                    for (int i = 0; i < cnt; i++)
                    {

                        dataGridView1.Rows.Add(d.dview[i][0].ToString(), d.dview[i][1].ToString(), d.dview[i][2].ToString(), d.dview[i][3].ToString(),
                            d.dview[i][4].ToString(),double.Parse( d.dview[i][5].ToString()), double.Parse(d.dview[i][6].ToString()), double.Parse(d.dview[i][7].ToString()),
                            double.Parse(d.dview[i][8].ToString()), double.Parse(d.dview[i][9].ToString()), double.Parse(d.dview[i][10].ToString()), d.dview[i][11].ToString(),
                            d.dview[i][12].ToString(), d.dview[i][13].ToString(), double.Parse(d.dview[i][14].ToString()), d.dview[i][15].ToString(),
                            double.Parse(d.dview[i][16].ToString()), d.dview[i][17].ToString(), double.Parse(d.dview[i][18].ToString()), d.dview[i][19].ToString(), d.dview[i][20].ToString(), d.dview[i][21].ToString());


                    }


                dtgrdvwcolor();

            }
            
        }


        public async void dtgrdvwcolor()
        {
            await Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToInt32(row.Cells[7].Value) < 0)
                    {
                        row.Cells[7].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToInt32(row.Cells[7].Value) > 0)
                    {

                        row.Cells[7].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0 && Convert.ToDouble(row.Cells[6].Value) == 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Gray;
                    }

                    for (int i = 0; i < dthtl.Rows.Count; i++)
                    {
                        if (Convert.ToString(row.Cells[2].Value).Equals(dthtl.Rows[i][0].ToString()))
                        {
                            row.Cells[2].Style.BackColor = Color.YellowGreen;
                        }
                    }
                }
            });
        }
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (textBox3.Text != "" && textBox6.Text == "" && textBox4.Text == "")
            {
                DataRow[] lignes ;
               
                lignes = d.dt.Select("Cabin like '" + textBox3.Text + "%'");
               
                    foreach (DataRow d in lignes)
                    {
                    dataGridView1.Rows.Add(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8],
                        d[9], d[10], d[11], d[12], d[13], d[14], d[15], d[16],
                        d[17], d[18], d[19],d[20],d[21]);
                    }
                dtgrdvwcolor();



            }
            else if(textBox3.Text != "" && textBox6.Text != "" && textBox4.Text == "")
            {
                 DataRow[] lignes ;
               
                lignes = d.dt.Select("Cabin like '" + textBox3.Text + "%' and Airlin like '" + textBox6.Text + "%'");
               
                    foreach (DataRow d in lignes)
                    {
                        dataGridView1.Rows.Add(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8],
                            d[9], d[10], d[11], d[12], d[13], d[14], d[15], d[16],
                            d[17], d[18], d[19], d[20], d[21]);
                    }
                dtgrdvwcolor();
            }
            else if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text=="")
            {
                DataRow[] lignes;

                lignes = d.dt.Select("Cabin like '" + textBox3.Text + "%' and Class like '" + textBox4.Text + "%'");

                foreach (DataRow d in lignes)
                {
                    dataGridView1.Rows.Add(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8],
                        d[9], d[10], d[11], d[12], d[13], d[14], d[15], d[16],
                        d[17], d[18], d[19], d[20], d[21]);
                }
                dtgrdvwcolor();
            }
            else if (textBox3.Text == "" && textBox4.Text != "" && textBox6.Text == "")
            {
                DataRow[] lignes;

                lignes = d.dt.Select("Class like '" + textBox4.Text + "%'");

                foreach (DataRow d in lignes)
                {
                    dataGridView1.Rows.Add(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8],
                        d[9], d[10], d[11], d[12], d[13], d[14], d[15], d[16],
                        d[17], d[18], d[19], d[20], d[21]);
                }
                dtgrdvwcolor();
            }
            else if (textBox3.Text == "" && textBox4.Text == "" && textBox6.Text != "")
            {
                dataGridView1.Rows.Clear();
                char[] c = { ',', '.' };

                string str = textBox6.Text;



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
                        d.cmdd.CommandText = "airlinet";
                        d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cntd = dv.Count;

                        for (int i = 0; i < cntd; i++)
                        {
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                        }
                    }
                    else if (cnt == 2)
                    {
                        string vr = tbl[0];
                        string vr1 = tbl[1];
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "airlinet1";
                        d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                        d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;

                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cntd = dv.Count;

                        for (int i = 0; i < cntd; i++)
                        {
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
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
                        d.cmdd.CommandText = "airlinet2";
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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
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
                        d.cmdd.CommandText = "airlinet3";
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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
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
                        d.cmdd.CommandText = "airlinet4";
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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
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
                        d.cmdd.CommandText = "airlinet5";
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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
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
                        d.cmdd.CommandText = "airlinet6";
                        d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                        d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                        d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;
                        d.cmdd.Parameters.Add("@airlin3", SqlDbType.VarChar, 20).Value = vr3;
                        d.cmdd.Parameters.Add("@airlin4", SqlDbType.VarChar, 20).Value = vr4;
                        d.cmdd.Parameters.Add("@airlin5", SqlDbType.VarChar, 20).Value = vr5;
                        d.cmdd.Parameters.Add("@airlin6S", SqlDbType.VarChar, 20).Value = vr6;
                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cntd = dv.Count;

                        for (int i = 0; i < cntd; i++)
                        {
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                        }
                    }
                }
                dtgrdvwcolor();
            }
            else if (textBox3.Text == "" && textBox4.Text != "" && textBox6.Text != "")
            {
                DataRow[] lignes;

                lignes = d.dt.Select("Airlin like '" + textBox6.Text + "%' and Class like '" + textBox4.Text + "%'");

                foreach (DataRow d in lignes)
                {
                    dataGridView1.Rows.Add(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8],
                        d[9], d[10], d[11], d[12], d[13], d[14], d[15], d[16],
                        d[17], d[18], d[19], d[20], d[21]);
                }
                dtgrdvwcolor();
            }
            else if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "")
            {
                DataRow[] lignes;

                lignes = d.dt.Select("Airlin like '" + textBox6.Text + "%' and Class like '" + textBox4.Text + "%'and Cabin like '" + textBox3.Text + "%'");

                foreach (DataRow d in lignes)
                {
                    dataGridView1.Rows.Add(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8],
                        d[9], d[10], d[11], d[12], d[13], d[14], d[15], d[16],
                        d[17], d[18], d[19], d[20], d[21]);
                }
                dtgrdvwcolor();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            nbr.Text = trackBar1.Value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dtgrdvwcolor();
        }
        public  void searchAIRPORTS()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "cityst";
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = textBox5.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
           if(cnt==0)
            {
                MessageBox.Show("Table 'airport code' does not contain this code or \n Table 'tax and fares' does not contain this code");
            }
                for (int i = 0; i < cnt; i++)
                {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                    dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                    double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                    dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                    double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
            }

        }

        private  void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            searchAIRPORTS();

            dtgrdvwcolor();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string str = "3";
            Information_about_files inf = new Information_about_files(str);
            inf.ShowDialog();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("Please enter the file name! ", "the file name", "", -1, -1);
            if (str != "")
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                int i = 0;
                int j = 0;
                int c = dataGridView1.RowCount;

                await Task.Run(() =>
                {
                    for (i = 0; i <= dataGridView1.RowCount - 1; i++)
                    {
                        for (j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                        {
                            DataGridViewCell cell = dataGridView1[j, i];
                            xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                        }
                    }
                });
                str = str + ".xls";
                xlWorkBook.SaveAs(str, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel file created , you can find the file c:\\" + str);

            }
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if(textBox7.Text!="")
            {
                nbr.Text = textBox7.Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (radioButton1.Checked==true)
                {
                    if (nbr.Text != "0")
                    {
                        if (d.dt.Rows.Count != 0)
                        {
                            d.dt.Rows.Clear();
                        }
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "serchFromToprice";
                        d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                        d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(nbr.Text);

                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cnt = dv.Count;

                        for (int i = 0; i < cnt; i++)
                        {
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                                dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                                double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                                dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                                double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                        }
                        textBox1.Text = "";
                        textBox2.Text = "";
                        nbr.Text = "";
                        dtgrdvwcolor();
                    }
                    else { MessageBox.Show("You must fill in the blank field"); }
                }
                else { MessageBox.Show("You can only use the new price"); }
            }
            else
            {
                MessageBox.Show("You must fill in the blank field FROM and TO");
            }
        }

        private void nbr_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
           addFares a = new addFares();
            a.ShowDialog();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            addDATA ad = new addDATA();
            ad.Show();
        }
        int pagenumber = 1;
        IPagedList<t> list;
        public async Task<IPagedList<t>> GetPagedListAsync(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (airlinEntities db = new airlinEntities())
                {
                    return db.t.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        public async void color()
        {
            await Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (Convert.ToInt32(row.Cells[7].Value) < 0)
                    {
                        row.Cells[7].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToInt32(row.Cells[7].Value) > 0)
                    {

                        row.Cells[7].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0 && Convert.ToDouble(row.Cells[6].Value) == 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Gray;
                    }
                }
            });
        }
        private async void button10_Click(object sender, EventArgs e)
        {
            button11.Visible = true;
            button12.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;

            list = await GetPagedListAsync();
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label7.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            color();
            
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            list = await GetPagedListAsync(++pagenumber);
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label7.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            color();
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            list = await GetPagedListAsync(--pagenumber);
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label7.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            color();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            color();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedValue.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox3.SelectedValue.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox6.Text = comboBox2.SelectedValue.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            if (textBox3.Text == "" && textBox4.Text == "" && textBox6.Text != "" && textBox1.Text!="" && textBox2.Text!="")
            {
                dataGridView1.Rows.Clear();
                char[] c = { ',', '.' };

                string str = textBox6.Text;



                string[] tbl = str.Split(c);
                int cnt;
                cnt = tbl.Length;
                
                if (cnt > 1)
                {
                    MessageBox.Show("The maximum is 1 codes");
                }
                
                else if (cnt < 2)
                {
                   
                    if (cnt == 1)
                    {
                        string vr = tbl[0];
                       
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "FromToAirline";
                        d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
                        d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
                        d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cntd = dv.Count;
                        MessageBox.Show("" + cntd);
                        for (int i = 0; i < cntd; i++)
                        {
                           dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                           dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                           double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                           dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                           double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                        }
                    }
                   
                    
                }
                dtgrdvwcolor();
            }
            else if (textBox3.Text == "" && textBox4.Text == "" && textBox6.Text != "" && textBox1.Text != "" && textBox2.Text == "")
            {
                dataGridView1.Rows.Clear();
                char[] c = { ',', '.' };

                string str = textBox6.Text;



                string[] tbl = str.Split(c);
                int cnt;
                cnt = tbl.Length;

                if (cnt > 1)
                {
                    MessageBox.Show("The maximum is 1 codes");
                }

                else if (cnt < 2)
                {

                    if (cnt == 1)
                    {
                        string vr = tbl[0];

                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "FromToAirline1";
                        d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
                        d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cntd = dv.Count;
                        MessageBox.Show("" + cntd);
                        for (int i = 0; i < cntd; i++)
                        {
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                            dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                            double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                            dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                            double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                        }
                    }


                }
                dtgrdvwcolor();
            }
          else if (textBox3.Text == "" && textBox4.Text == "" && textBox6.Text != "" && textBox1.Text == "" && textBox2.Text != "")
            {
                dataGridView1.Rows.Clear();
                char[] c = { ',', '.' };

                string str = textBox6.Text;



                string[] tbl = str.Split(c);
                int cnt;
                cnt = tbl.Length;

                if (cnt > 1)
                {
                    MessageBox.Show("The maximum is 1 codes");
                }

                else if (cnt < 2)
                {

                    if (cnt == 1)
                    {
                        string vr = tbl[0];

                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "FromToAirline2";
                        d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
                        d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                        d.cmdd.Connection = d.cn;
                        d.dr = d.cmdd.ExecuteReader();
                        d.dt.Load(d.dr);
                        DataView dv = new DataView(d.dt);
                        int cntd = dv.Count;
                        MessageBox.Show("" + cntd);
                        for (int i = 0; i < cntd; i++)
                        {
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), dv[i][3].ToString(),
                            dv[i][4].ToString(), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()),
                            double.Parse(dv[i][8].ToString()), double.Parse(dv[i][9].ToString()), double.Parse(dv[i][10].ToString()), dv[i][11].ToString(),
                            dv[i][12].ToString(), dv[i][13].ToString(), double.Parse(dv[i][14].ToString()), dv[i][15].ToString(),
                            double.Parse(dv[i][16].ToString()), dv[i][17].ToString(), double.Parse(dv[i][18].ToString()), dv[i][19].ToString(), dv[i][20].ToString(), dv[i][21].ToString());
                        }
                    }


                }
                dtgrdvwcolor();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                string str = val;
               
                for (int i = 0; i < dthtl.Rows.Count; i++)
                {
                    if (str.Equals(dthtl.Rows[i][0].ToString()))
                    {
                        Hotel h = new Hotel(str,"null");
                        h.Show();
                    }
                }
            }
            catch { }
            dtgrdvwcolor();
        }
    }
}
