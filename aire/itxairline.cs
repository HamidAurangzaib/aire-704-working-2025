using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using PagedList;
using System.Linq;
namespace aire
{
    public partial class itxairline : Form
    {
        private readonly SynchronizationContext synchronizationcontext;
        ado d = new ado();
        string cabin;
        public itxairline(string itxcabin)
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;
            cabin = itxcabin;
        }
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string from1;
        private void comb()
        {
            if (cabin == "all")
            {
                from1 = "each_itx_bothallcabin";
            }
            else if(cabin=="normal")
            {
                from1 = "each_itx_both";
            }
            d.da = new SqlDataAdapter("select distinct [From] from " + from1 + "", d.cn);
            d.da.Fill(d.ds, "com1");
            d.da = new SqlDataAdapter("select distinct [To] from " + from1 + "", d.cn);
            d.da.Fill(ds1, "com2");
            d.da = new SqlDataAdapter("select distinct Airline from " + from1 + "", d.cn);
            d.da.Fill(ds2, "com3");
            comboBox2.DataSource = d.ds.Tables["com1"];
            comboBox2.DisplayMember = "From";
            comboBox2.ValueMember = "From";

            comboBox3.DataSource = ds1.Tables["com2"];
            comboBox3.DisplayMember = "To";
            comboBox3.ValueMember = "To";

            comboBox1.DataSource = ds2.Tables["com3"];
            comboBox1.DisplayMember = "Airline";
            comboBox1.ValueMember = "Airline";
        }

        private void itxairline_Load(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            d.connecter();
            
            button8.Visible = false;
            button9.Visible = false;
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            comb();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";

            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }

        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();

        public async void datagridvColor()
        {
            

            

            await Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToDouble(row.Cells[6].Value) < 0)
                    {
                        row.Cells[6].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Gray;
                    }

                    for (int i = 0; i < dthtl.Rows.Count; i++)
                    {
                        if (Convert.ToString(row.Cells[1].Value).Equals(dthtl.Rows[i][0].ToString()))
                        {
                            row.Cells[1].Style.BackColor = Color.YellowGreen;
                        }
                    }
                }
            });
        }
        string search1;
        public void searchFROMTO()
        {

            if (cabin == "all")
            {
                search1 = "serchFromitxallcabin";
            }
            else if (cabin == "normal")
            {
                search1 = "serchFromitx";
            }
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = search1;
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
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());

            }


        }
        string search2, search3;
        public void searchFROM()
        {

            if (cabin == "all")
            {
                search2 = "serchFromitx_bothallcabin";
            }
            else if (cabin == "normal")
            {
                search2 = "serchFromitx_both";
            }

            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = search2;
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
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
            }

        }


        public void searchTO()
        {

            if (cabin == "all")
            {
                search3 = "serchToitx_bothallcabin";
            }
            else if (cabin == "normal")
            {
                search3 = "serchToitx_both";
            }
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = search3;
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
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                searchFROMTO();

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                searchFROM();
                datagridvColor();
                textBox1.Text = "";
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchTO();

                datagridvColor();
                textBox2.Text = "";
            }
        }
        string ITXdate;
        double minP, maxP;
        private void myfunction()
        {
            if (min.Text != "" && max.Text != "")
            {
                minP = double.Parse(min.Text);
                maxP = double.Parse(max.Text);
            }
            else if (min.Text != "" && max.Text == "")
            {
                minP = double.Parse(min.Text);
                maxP = double.Parse(min.Text);
            }
            else
            {
                minP = double.Parse(max.Text);
                maxP = double.Parse(max.Text);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (min.Text=="" && max.Text=="")
            {
                if (cabin == "all")
                {
                    ITXdate = "serchitx_bothallcabin";
                }
                else if (cabin == "normal")
                {
                    ITXdate = "serchitx_both";
                }
               


                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();

                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = ITXdate;
                d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");

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
                    dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                        double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                }
            }
            else
            {
                if (cabin == "all")
                {
                    ITXdate = "searchDatePriceEach_itx_bothallcabin";
                }
                else if (cabin == "normal")
                {
                    ITXdate = "searchDatePriceEach_itx_both";
                }


                myfunction();
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();

                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = ITXdate;
                d.cmdd.Parameters.Add("@dateA", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@dateB", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = minP;
                d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = maxP;
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
                    dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                        double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                }
            }

            min.Text = "";
            max.Text = "";
            datagridvColor();
        }
        string cabin1;
        private void cabinitxairline()
        {
            if (cabin == "all")
            {
                cabin1 = "Cabinitx_bothallcabin";
            }
            else if (cabin == "normal")
            {
                cabin1 = "Cabinitx_both";
            }
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = cabin1;
            d.cmdd.Parameters.Add("@cabin", SqlDbType.VarChar, 20).Value = textBox3.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            cabinitxairline();
            datagridvColor();
        }

        string airline1, airline2, airline3, airline4, airline5, airline6, airline7;
        private void button6_Click(object sender, EventArgs e)
        {


            if (cabin == "all")
            {
                airline1 = "airlineeach_itx_bothallcabin";
                airline2 = "airlineeach_itx_both1allcabin";
                airline3 = "airlineeach_itx_both2allcabin";
                airline4 = "airlineeach_itx_both3allcabin";
                airline5 = "airlineeach_itx_both4allcabin";
                airline6 = "airlineeach_itx_both5allcabin";
                airline7 = "airlineeach_itx_both6allcabin";
            }
            else if (cabin == "normal")
            {
                airline1 = "airlineeach_itx_both";
                airline2 = "airlineeach_itx_both1";
                airline3 = "airlineeach_itx_both2";
                airline4 = "airlineeach_itx_both3";
                airline5 = "airlineeach_itx_both4";
                airline6 = "airlineeach_itx_both5";
                airline7 = "airlineeach_itx_both6";
            }
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            char[] c = { ',', '.',' '};

            string str = textBox4.Text;



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
                    d.cmdd.CommandText = airline1;
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }
                }
                else if (cnt == 2)
                {
                    string vr = tbl[0];
                    string vr1 = tbl[1];
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = airline2;
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;

                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
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
                    d.cmdd.CommandText = airline3;
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;

                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
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
                    d.cmdd.CommandText = airline4;
                    d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar, 20).Value = vr;
                    d.cmdd.Parameters.Add("@airlin1", SqlDbType.VarChar, 20).Value = vr1;
                    d.cmdd.Parameters.Add("@airlin2", SqlDbType.VarChar, 20).Value = vr2;
                    d.cmdd.Parameters.Add("@airlin3", SqlDbType.VarChar, 20).Value = vr3;

                    d.cmdd.Connection = d.cn;
                    d.dr = d.cmdd.ExecuteReader();
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cntd = dv.Count;
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
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
                    d.cmdd.CommandText = airline5;
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
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
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
                    d.cmdd.CommandText = airline6;
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
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
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
                    d.cmdd.CommandText = airline7;
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
                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    for (int i = 0; i < cntd; i++)
                    {
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }
                }
            }
            datagridvColor();

        }
        string price1, price2, price3, price4, price5, price6;

        private void button2_Click(object sender, EventArgs e)
        {
            if (cabin == "all")
            {

                price1 = "priceeach_itx_bothallcabin";
                price2 = "priceitx_both1allcabin";
                price3 = "priceitxairlinelowallcabin";
                price4 = "diffitxairlineallcabin";
                price5 = "diffitxairline1allcabin";
                price6 = "diffitxairlineallcabin";
            }
            else if (cabin == "normal")
            {
                price1 = "priceeach_itx_both";
                price2 = "priceitx_both1";
                price3 = "priceitxairlinelow";
                price4 = "diffitxairline";
                price5 = "diffitxairline1";
                price6 = "diffitxairline";
            }
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (checkBox7.Checked == true && checkBox8.Checked == false)
            {

                if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                {


                    if (d.dt.Rows.Count != 0)
                    {
                        d.dt.Rows.Clear();
                    }
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = price1;
                    d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);
                    d.cmdd.Parameters.Add("@price2", SqlDbType.Int).Value = double.Parse(maxprice.Text);

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
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }

                    datagridvColor();
                }
                else if (radioButton1.Checked && minPrice.Text != "")
                {

                    if (d.dt.Rows.Count != 0)
                    {
                        d.dt.Rows.Clear();
                    }
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = price2;
                    d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);


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
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }
                    datagridvColor();
                }
                else if (radioButton2.Checked && minPrice.Text != "")
                {
                    if (d.dt.Rows.Count != 0)
                    {
                        d.dt.Rows.Clear();
                    }
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = price3;
                    d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);


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
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }
                    datagridvColor();
                }




            }
            else if (checkBox8.Checked == true && checkBox7.Checked == false)
            {
                if (radioButton3.Checked==true && minPrice.Text != "" && maxprice.Text != "")
                {


                    if (d.dt.Rows.Count != 0)
                    {
                        d.dt.Rows.Clear();
                    }
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = price4;
                    d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);
                    d.cmdd.Parameters.Add("@price2", SqlDbType.Int).Value = double.Parse(maxprice.Text);

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
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }

                    datagridvColor();
                }
                else if (radioButton1.Checked && minPrice.Text != "")
                {

                    if (d.dt.Rows.Count != 0)
                    {
                        d.dt.Rows.Clear();
                    }
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = price5;
                    d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);


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
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }
                    datagridvColor();
                }
                else if (radioButton2.Checked && minPrice.Text != "")
                {
                    if (d.dt.Rows.Count != 0)
                    {
                        d.dt.Rows.Clear();
                    }
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = price6;
                    d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);


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
                        dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                            double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                    }
                    datagridvColor();
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = true;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible == true && dataGridView2.Visible == false && dataGridView1.Rows.Count > 0)
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
            else if (dataGridView2.Visible == true && dataGridView1.Visible == false && dataGridView2.Rows.Count > 0)
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
                    int c = dataGridView2.RowCount;

                    await Task.Run(() =>
                    {
                        for (i = 0; i <= dataGridView2.RowCount - 1; i++)
                        {
                            for (j = 0; j <= dataGridView2.ColumnCount - 1; j++)
                            {
                                DataGridViewCell cell = dataGridView2[j, i];
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
        int pagenumber = 1;
        IPagedList<each_itx_both> list;
        public async Task<IPagedList<each_itx_both>> GetPagedListAsync(int pageNumber = 1, int pageSize = 100)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (DB_A61545_andycomEntities10 db = new DB_A61545_andycomEntities10())
                {
                    return db.each_itx_both.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        private async void colr()
        {
            await Task.Run(() =>
            {
                dataGridView2.Columns.Remove("id");
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (Convert.ToDouble(row.Cells[5].Value) < 0)
                    {
                        row.Cells[5].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[5].Value) > 0)
                    {
                        row.Cells[5].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[3].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0)
                    {
                        row.Cells[5].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[3].Value) > 0 && Convert.ToDouble(row.Cells[4].Value) == 0)
                    {
                        row.Cells[5].Style.BackColor = Color.Gray;
                    }
                }
            });
        }
        private async void button7_Click(object sender, EventArgs e)
        {
            button8.Visible = true;
            button9.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
            list = await GetPagedListAsync();
            button8.Enabled = list.HasPreviousPage;
            button9.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label5.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            colr();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string str = "";
            if(cabin=="all")
            {
                str = "8";
            }
            else if(cabin=="normal")
            {
                str = "88";
            }
            Information_about_files inf = new Information_about_files(str);
            inf.ShowDialog();
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            if (list.HasNextPage)
            {
                list = await GetPagedListAsync(++pagenumber);
                button8.Enabled = list.HasPreviousPage;
                button9.Enabled = list.HasNextPage;
                dataGridView2.DataSource = list.ToList();
                label5.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                colr();
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (list.HasPreviousPage)
            {
                list = await GetPagedListAsync(--pagenumber);
                button8.Enabled = list.HasPreviousPage;
                button9.Enabled = list.HasNextPage;
                dataGridView2.DataSource = list.ToList();
                label5.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                colr();
            }
        }
        string pricefrm1, pricefrm2, pricefrm3;
        private void button10_Click(object sender, EventArgs e)
        {

            if (cabin == "all")
            {

                pricefrm1 = "serchFromTopriceitxbigallcabin";
                pricefrm2 = "serchFromTopriceitxallcabin";
                pricefrm3 = "serchFromTopriceitxbetweenallcabin";
            }
            else if (cabin == "normal")
            {
                pricefrm1 = "serchFromTopriceitxbig";
                pricefrm2 = "serchFromTopriceitx";
                pricefrm3 = "serchFromTopriceitxbetween";
               
            }
            button8.Visible = false;
            button9.Visible = false;
            label5.Text = "";
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();

            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (checkBox7.Checked == true && checkBox8.Checked == false)
                {
                    if (radioButton1.Checked == true && minPrice.Text != "")
                    {
                        if (d.dt.Rows.Count != 0)
                        {
                            d.dt.Rows.Clear();
                        }
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = pricefrm1;
                        d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                        d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);

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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                                double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                        }
                        textBox1.Text = "";
                        textBox2.Text = "";
                        minPrice.Text = "";
                        maxprice.Text = "";
                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {
                        if (d.dt.Rows.Count != 0)
                        {
                            d.dt.Rows.Clear();
                        }
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = pricefrm2;
                        d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                        d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);

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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                                double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                        }
                        textBox1.Text = "";
                        textBox2.Text = "";
                        minPrice.Text = "";
                        maxprice.Text = "";
                        datagridvColor();
                    }
                    else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                    {
                        if (d.dt.Rows.Count != 0)
                        {
                            d.dt.Rows.Clear();
                        }
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = pricefrm3;
                        d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                        d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                        d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);

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
                            dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                                double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
                        }
                        textBox1.Text = "";
                        textBox2.Text = "";
                        minPrice.Text = "";
                        maxprice.Text = "";
                        datagridvColor();
                    }
                    else { MessageBox.Show("You must fill in the blank field "); }
                }

                else if (checkBox7.Checked == false && checkBox8.Checked == true)
                {
                    MessageBox.Show("You can only use the new price");
                }
            }
            else { MessageBox.Show("You must fill in the blank field FROM and TO"); }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            itx_both itx = new itx_both(cabin);
            itx.Show();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            colr();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            datagridvColor();
        }
      


        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox1.Text = comboBox2.SelectedValue.ToString();
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox2.Text = comboBox3.SelectedValue.ToString();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox4.Text = comboBox1.SelectedValue.ToString();
        }
        string city1;
        private void cityitx()
        {
            if (cabin == "all")
            {

                city1 = "citysitxallcabin";
            }
            else if (cabin == "normal")
            {
                city1 = "citysitx";

            }

            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = city1;
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 50).Value = textBox5.Text;
           

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
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                    double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString());
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            cityitx();
            datagridvColor();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                string str = val;
                int index = e.RowIndex;
                string date = dataGridView1.Rows[index].Cells[3].Value.ToString();
                for (int i = 0; i < dthtl.Rows.Count; i++)
                {
                    if (str.Equals(dthtl.Rows[i][0].ToString()))
                    {
                        Hotel h = new Hotel(str,date);
                        h.Show();
                    }
                }
            }
            catch { }
            datagridvColor();
        }
    }
}
