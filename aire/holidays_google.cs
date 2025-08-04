using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aire
{
    public partial class holidays_google : Form
    {
        public holidays_google()
        {
            InitializeComponent();
        }
        ado d = new ado();
        int days;
        string strDays,name;
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        int nbr = 0;
        private async  void search(string frm,string to,string hotel,string dys,int nbrDays)
        {
            //label2.Visible = true;
            //if(nbrDays==14)
            //{
            //    await Task.Run(() =>
            //    {

            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec inserttableGFforHoliday " + frm + "," + to + "," + hotel + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec InsertHoliday " + hotel + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltholidy ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec updtHoliday14 ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        comboBox1.Items.Clear();
            //        d.da = new SqlDataAdapter("select distinct Dates from holidy ORDER BY DateS asc", d.cn);
            //        d.da.Fill(d.ds, "com1");
            //        comboBox1.SelectedItem = null;
            //        comboBox1.SelectedText = "--select--";
            //        comboBox1.DataSource = d.ds.Tables["com1"];
            //        comboBox1.DisplayMember = "Dates";
            //        comboBox1.ValueMember = "Dates";

            //    });
            //    label2.Visible = false;
               
               
            //}
            //else
            //{
            //    await Task.Run(() =>
            //    {
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec inserttableGFforHoliday2to7 '" + frm + "','" + to + "','" + hotel + "','" + dys +"'," + nbrDays + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec InsertHoliday2to7 '" + hotel + "'," + nbrDays + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltholidy2to7 ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec updtHoliday " + nbrDays + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        comboBox1.Items.Clear();
            //        d.da = new SqlDataAdapter("select distinct Dates from holidy2to7 ORDER BY DateS asc", d.cn);
            //        d.da.Fill(d.ds, "com1");
                    
            //        comboBox1.DataSource = d.ds.Tables["com1"];
            //        comboBox1.DisplayMember = "Dates";
            //        comboBox1.ValueMember = "Dates";
            //        comboBox1.SelectedItem = null;
            //        comboBox1.SelectedText = "--select--";

            //    });
            //    label2.Visible = false;
               
               
            //}
           
        }
        DataSet ds1 = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {
            if (TripadvisorRadio.Checked && WorldwideRadio.Checked)
            {
                if (date1.Text == null || date2.Text == null)
                {
                    MessageBox.Show("Please enter From and To Dates");
                }
                customerBindingSource.Rows.Clear();
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "searchGFATripadvisorHotel";
                d.cmdd.Parameters.Add("@HotelName", SqlDbType.VarChar, 100).Value = HotelDDL.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = AirlineDDL.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = txtFrom.Text.ToString() != "" ? txtFrom.Text.ToString() : "Not Selected";
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 100).Value = textTo.Text.ToString() != "" ? textTo.Text.ToString() : "Not Selected";
                d.cmdd.Parameters.Add("@Days", SqlDbType.VarChar, 100).Value = strDays != null ? strDays.ToString() : "Not Selected";
                d.cmdd.Parameters.Add("@StartDate", SqlDbType.VarChar, 100).Value = date1.Text;
                d.cmdd.Parameters.Add("@EndDate", SqlDbType.VarChar, 100).Value = date2.Text;
                d.cmdd.Connection = d.cn;

                d.dt.Load(d.cmdd.ExecuteReader());
                int cnt = d.dt.Rows.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                else
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        customerBindingSource.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), d.dt.Rows[i][3].ToString(), DateTime.Parse(d.dt.Rows[i][4].ToString()), DateTime.Parse(d.dt.Rows[i][5].ToString())
                        , d.dt.Rows[i][6].ToString(), d.dt.Rows[i][7].ToString(), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString()
                        , float.Parse(d.dt.Rows[i][10].ToString()), float.Parse(d.dt.Rows[i][11].ToString()), float.Parse(d.dt.Rows[i][12].ToString())
                        , float.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString()
                        , float.Parse(d.dt.Rows[i][16].ToString()), float.Parse(d.dt.Rows[i][17].ToString()), float.Parse(d.dt.Rows[i][18].ToString())
                        , float.Parse(d.dt.Rows[i][19].ToString()), d.dt.Rows[i][20].ToString(), d.dt.Rows[i][21].ToString(), d.dt.Rows[i][22].ToString(), d.dt.Rows[i][23].ToString());
                    }
                }
            }
            else if(TripadvisorRadio.Checked && DomesticRadio.Checked)
            {
                if (date1.Text == null || date2.Text == null)
                {
                    MessageBox.Show("Please enter From and To Dates");
                }
                customerBindingSource.Rows.Clear();
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "searchGFADomesticTripadvisorHotel";
                d.cmdd.Parameters.Add("@HotelName", SqlDbType.VarChar, 100).Value = HotelDDL.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = AirlineDDL.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = txtFrom.Text.ToString() != "" ? txtFrom.Text.ToString() : "Not Selected";
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 100).Value = textTo.Text.ToString() != "" ? textTo.Text.ToString() : "Not Selected";
                d.cmdd.Parameters.Add("@Days", SqlDbType.VarChar, 100).Value = strDays != null ? strDays.ToString() : "Not Selected";
                d.cmdd.Parameters.Add("@StartDate", SqlDbType.VarChar, 100).Value = date1.Text;
                d.cmdd.Parameters.Add("@EndDate", SqlDbType.VarChar, 100).Value = date2.Text;
                d.cmdd.Connection = d.cn;

                d.dt.Load(d.cmdd.ExecuteReader());
                int cnt = d.dt.Rows.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                else
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        customerBindingSource.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), d.dt.Rows[i][3].ToString(), DateTime.Parse(d.dt.Rows[i][4].ToString()), DateTime.Parse(d.dt.Rows[i][5].ToString())
                        , d.dt.Rows[i][6].ToString(), d.dt.Rows[i][7].ToString(), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString()
                        , float.Parse(d.dt.Rows[i][10].ToString()), float.Parse(d.dt.Rows[i][11].ToString()), float.Parse(d.dt.Rows[i][12].ToString())
                        , float.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString()
                        , float.Parse(d.dt.Rows[i][16].ToString()), float.Parse(d.dt.Rows[i][17].ToString()), float.Parse(d.dt.Rows[i][18].ToString())
                        , float.Parse(d.dt.Rows[i][19].ToString()), d.dt.Rows[i][20].ToString(), d.dt.Rows[i][21].ToString(), d.dt.Rows[i][22].ToString(), d.dt.Rows[i][23].ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select 'Search Hotel' and 'Search Flight' Radio Buttons");
            }
        }

        private void holidays_google_Load(object sender, EventArgs e)
        {
            this.customerBindingSource.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            //int counter = 0;
            //foreach (DataGridViewColumn column in customerBindingSource.Columns)
            //{
            //    if (counter < 21)
            //    {
            //        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //    }
            //    else
            //    {
            //        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //    }
            //    counter++;
            //}
            d.connecter();
            comb();
            BindAirlineDDL();
            d.cmdd = new System.Data.SqlClient.SqlCommand("delete  Holiday", d.cn);
            d.cmdd.ExecuteNonQuery();
        }
        private void comb()
        {
            d.ds.Clear();

            d.da = new SqlDataAdapter("select distinct Hotel_name from cmprTripadvisor order by Hotel_name", d.cn);
            d.da.Fill(d.ds, "Name");

            //Adding 'Please Select'
            DataRow HdRow = d.ds.Tables["Name"].NewRow();
            HdRow[0] = "Please Select";
            d.ds.Tables["Name"].Rows.InsertAt(HdRow, 0);

            HotelDDL.DataSource = d.ds.Tables["Name"];
            HotelDDL.DisplayMember = "Hotel_name";
            HotelDDL.ValueMember = "Hotel_name";
        }
        private void RD2Days_CheckedChanged(object sender, EventArgs e)
        {
            days = 2;
            strDays = "2 day";
        }

        private void RD3Days_CheckedChanged(object sender, EventArgs e)
        {
            days = 3;
            strDays = "3 day";
        }

        private void RD4Days_CheckedChanged(object sender, EventArgs e)
        {
            days = 4;
            strDays = "4 day";
        }

        private void RD7Days_CheckedChanged(object sender, EventArgs e)
        {
            days = 7;
            strDays = "7 day";
        }

        private void Trivago_CheckedChanged(object sender, EventArgs e)
        {
            name = "Trivago";
        }

        private void Tripadvisor_CheckedChanged(object sender, EventArgs e)
        {
            name = "Tripadvisor";
        }
        string dates;
        private  void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //dates = comboBox1.Text;
            //if (dates != "--select--")
            //{
            //    if (days == 14)
            //    {

            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec Insrthld3 '" + dates + "'", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHldy3 ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec cursorHoliday ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHldy ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHoliday2 ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec updtHomeHoliday ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHolidayByDateIsNotInHotel  '" + dates + "','" + days + "'," + name + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //    }
            //    else
            //    {
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec Insrthld32to7 '" + dates + "'", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHldy32to7 ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec caseforall " + days + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHldy ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHoliday2 ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec updtHomeHoliday ", d.cn);
            //        d.cmdd.ExecuteNonQuery();
            //        d.cmdd = new System.Data.SqlClient.SqlCommand("exec dltHolidayByDateIsNotInHotel  '" + dates + "','" + days + "'," + name + "", d.cn);
            //        d.cmdd.ExecuteNonQuery();

            //    }


            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            customerBindingSource.Rows.Clear();
            int count;

            d.dt.Rows.Clear();


            d.da = new SqlDataAdapter("select * from Holiday", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "HLD");
            count = d.ds.Tables["HLD"].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                customerBindingSource.Rows.Add(
                    d.ds.Tables["HLD"].Rows[i][0].ToString(), d.ds.Tables["HLD"].Rows[i][1].ToString(), d.ds.Tables["HLD"].Rows[i][2].ToString(),
                    d.ds.Tables["HLD"].Rows[i][3].ToString(), DateTime.Parse(d.ds.Tables["HLD"].Rows[i][4].ToString()), DateTime.Parse(d.ds.Tables["HLD"].Rows[i][5].ToString()),
                    d.ds.Tables["HLD"].Rows[i][6].ToString(), d.ds.Tables["HLD"].Rows[i][7].ToString(), d.ds.Tables["HLD"].Rows[i][8].ToString(),
                    float.Parse(d.ds.Tables["HLD"].Rows[i][9].ToString()), float.Parse(d.ds.Tables["HLD"].Rows[i][10].ToString()), float.Parse(d.ds.Tables["HLD"].Rows[i][11].ToString()),
                    d.ds.Tables["HLD"].Rows[i][12].ToString(), d.ds.Tables["HLD"].Rows[i][13].ToString(), d.ds.Tables["HLD"].Rows[i][14].ToString(),
                    float.Parse(d.ds.Tables["HLD"].Rows[i][15].ToString()), int.Parse(d.ds.Tables["HLD"].Rows[i][16].ToString()), int.Parse(d.ds.Tables["HLD"].Rows[i][17].ToString()),
                    d.ds.Tables["HLD"].Rows[i][18].ToString(), d.ds.Tables["HLD"].Rows[i][19].ToString(), d.ds.Tables["HLD"].Rows[i][20].ToString()
                    );
            }

        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (DomesticRadio.Checked)
                BindAirlineDDL();
        }
        private void BindAirlineDDL()
        {
            if (d.ds.Tables["Airline"] != null)
                d.ds.Tables["Airline"].Rows.Clear();

            if (DomesticRadio.Checked)
            {
                d.da = new SqlDataAdapter("select distinct [Airline] from comprGOOGLCOPY", d.cn);
                d.da.Fill(d.ds, "Airline");
            }
            else
            {

                d.da = new SqlDataAdapter("select distinct [Airline] from comprGOOGLAirline", d.cn);
                d.da.Fill(d.ds, "Airline");
            }

            //Adding 'Please Select'
            DataRow HrRow = d.ds.Tables["Airline"].NewRow();
            HrRow[0] = "Please Select";
            d.ds.Tables["Airline"].Rows.InsertAt(HrRow, 0);

            AirlineDDL.DataSource = d.ds.Tables["Airline"];
            AirlineDDL.DisplayMember = "Airline";
            AirlineDDL.ValueMember = "Airline";

            AirlineDDL.SelectedIndex = 0;
        }
        private void customerBindingSource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    String[] spearator = { "https://" };

                    var val = this.customerBindingSource[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string str = val;

                    string[] tbl = str.Split(spearator, StringSplitOptions.None);
                    int cnt = 0;
                    cnt = tbl.Length;
                    DataTable dthtl = new DataTable();
                    if (cnt >= 2)
                    {
                        Process.Start(val);
                    }


                }
            }
            catch { }
        }

        private void WorldwideRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (WorldwideRadio.Checked)
                BindAirlineDDL();
        }

        private void RD14Days_CheckedChanged(object sender, EventArgs e)
        {
            days = 14;
            strDays = "14 day";
        }
    }
}
