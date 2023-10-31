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
using System.Diagnostics;

namespace aire
{
    public partial class Hotel : Form
    {
        string code1="null",datehotel;
      
        public Hotel(string code,string dateh)
        {
            InitializeComponent();
            code1 = code;
            datehotel = dateh;
        }


        ado d = new ado();
        string str;
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchDynamicTripadvisor";
            d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
            d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
            d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
            d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
            d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
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
                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString())
                    , float.Parse(d.dt.Rows[i][6].ToString()), float.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString()
                    , d.dt.Rows[i][9].ToString(), d.dt.Rows[i][15].ToString()
                    , float.Parse(d.dt.Rows[i][12].ToString()), float.Parse(d.dt.Rows[i][19].ToString())
                    , float.Parse(d.dt.Rows[i][14].ToString()), float.Parse(d.dt.Rows[i][20].ToString())
                    , float.Parse(d.dt.Rows[i][16].ToString())
                    , d.dt.Rows[i][23].ToString(), d.dt.Rows[i][24].ToString()
                    , d.dt.Rows[i][17].ToString(), float.Parse(d.dt.Rows[i][18].ToString())
                    , d.dt.Rows[i][21].ToString(), DateTime.Parse(d.dt.Rows[i][22].ToString())
                    , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }

                // Add buttons to the last two columns
                AddButtonsToLastTwoColumns();

                datagridvColor();
            }

            //if (textBox1.Text!="" && textBox2.Text=="" && textBox4.Text=="")
            //{
            //    d.dt.Rows.Clear();
            //    d.cmdd.Parameters.Clear();
            //    d.cmdd.CommandType = CommandType.StoredProcedure;
            //    d.cmdd.CommandText = "searshCodeHotel";
            //    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar,10).Value = textBox1.Text;
            //    d.cmdd.Connection = d.cn;
            //}
            //else if(textBox1.Text == "" && textBox2.Text != "" && textBox4.Text == "")
            //{
            //    d.dt.Rows.Clear();
            //    d.cmdd.Parameters.Clear();
            //    d.cmdd.CommandType = CommandType.StoredProcedure;
            //    d.cmdd.CommandText = "searshDestinationHotel";
            //    d.cmdd.Parameters.Add("@Destination", SqlDbType.VarChar, 30).Value = textBox2.Text;

            //    d.cmdd.Connection = d.cn;
            //}
            //else if(textBox1.Text == "" && textBox2.Text == "" && textBox4.Text != "")
            //{
            //    d.dt.Rows.Clear();
            //    d.cmdd.Parameters.Clear();
            //    d.cmdd.CommandType = CommandType.StoredProcedure;
            //    d.cmdd.CommandText = "searshhotel";
            //    d.cmdd.Parameters.Add("@hotel", SqlDbType.VarChar, 1000).Value = textBox4.Text;

            //    d.cmdd.Connection = d.cn;
            //}
            //d.dt.Load(d.cmdd.ExecuteReader());
            //int cnt = d.dt.Rows.Count;
            //if (cnt == 0)
            //{
            //    MessageBox.Show("The information entered is not on the database!");
            //}
            //else
            //{
            //    for (int i = 0; i < cnt; i++)
            //    {
            //        dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(),DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString())
            //        , d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString()), float.Parse(d.dt.Rows[i][6].ToString())
            //        , d.dt.Rows[i][7].ToString(), int.Parse(d.dt.Rows[i][8].ToString()), d.dt.Rows[i][9].ToString()
            //        , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), d.dt.Rows[i][13].ToString());
            //    }
            //}
        }
        private void AddButtonsToLastTwoColumns()
        {
            int lastColumnIndex = dataGridView1.Columns.Count - 1;
            int secondLastColumnIndex = dataGridView1.Columns.Count - 2;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Get the values of the last two columns
                string value1 = row.Cells[lastColumnIndex].Value?.ToString(); // Null check added here
                string value2 = row.Cells[secondLastColumnIndex].Value?.ToString(); // Null check added here

                // Create a button for the last column
                DataGridViewButtonCell buttonCell1 = new DataGridViewButtonCell();
                buttonCell1.Value = "Details";
                buttonCell1.Tag = value1; // Set the URL for the button

                // Create a button for the second last column
                DataGridViewButtonCell buttonCell2 = new DataGridViewButtonCell();
                buttonCell2.Value = "Details";
                buttonCell2.Tag = value2; // Set the URL for the button

                row.Cells[lastColumnIndex] = buttonCell1;
                row.Cells[secondLastColumnIndex] = buttonCell2;
            }
        }
        public async void datagridvColor()
        {

            try
            {
                await Task.Run(() =>
                {

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToDouble(row.Cells[14].Value) < 0)
                        {
                            row.Cells[14].Style.BackColor = Color.LightGreen;
                        }
                        else if (Convert.ToDouble(row.Cells[14].Value) > 0)
                        {
                            row.Cells[14].Style.BackColor = Color.Red;
                        }
                        if (Convert.ToDouble(row.Cells[14].Value) == 0 && Convert.ToDouble(row.Cells[12].Value != null ? (row.Cells[12].Value.ToString() != "Call us" && row.Cells[12].Value.ToString() != "#EANF#" && row.Cells[12].Value.ToString() != "" ? row.Cells[12].Value : 0) : 0) == 0 && Convert.ToDouble(row.Cells[13].Value != null ? (row.Cells[13].Value.ToString() != "Call us" && row.Cells[13].Value.ToString() != "#EANF#" && row.Cells[13].Value.ToString() != "" ? row.Cells[13].Value : 0) : 0) > 0)
                        {
                            row.Cells[14].Style.BackColor = Color.Orange;
                        }
                        if (Convert.ToDouble(row.Cells[14].Value) == 0 && Convert.ToDouble(row.Cells[12].Value != null ? (row.Cells[12].Value.ToString() != "Call us" && row.Cells[12].Value.ToString() != "#EANF#" && row.Cells[12].Value.ToString() != "" ? row.Cells[12].Value : 0) : 0) > 0 && Convert.ToDouble(row.Cells[13].Value != null ? (row.Cells[13].Value.ToString() != "Call us" && row.Cells[13].Value.ToString() != "#EANF#" && row.Cells[13].Value.ToString() != "" ? row.Cells[13].Value : 0) : 0) == 0)
                        {
                            row.Cells[14].Style.BackColor = Color.Gray;
                        }
                    }
                });
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            d.dt.Rows.Clear();
           

            if(comboBox1.Text=="Price" && textA.Text != "")
            {
                if(radiogreater.Checked==true && radioless2.Checked==false && radiobetween.Checked==false)
                {
                   
                   
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchTripadvisorGreaterTotalPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);

                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == true && radiobetween.Checked == false)
                {
                   

                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchTripadvisorLessTotalPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;
                }
                else if (radiogreater.Checked == false && radioless2.Checked == false && radiobetween.Checked == true && textB.Text != "")
                {
                  
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchTripadvisorBetweenTotalPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(textB.Text);
                    d.cmdd.Connection = d.cn;
                }
                else
                {
                    d.dt.Rows.Clear();
                    MessageBox.Show("The information entered is not on the database!");
                    return;
                }
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
                        dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString())
                        , float.Parse(d.dt.Rows[i][6].ToString()), float.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString()
                        , d.dt.Rows[i][9].ToString(), d.dt.Rows[i][15].ToString()
                        , float.Parse(d.dt.Rows[i][12].ToString()), float.Parse(d.dt.Rows[i][19].ToString())
                        , float.Parse(d.dt.Rows[i][14].ToString()), float.Parse(d.dt.Rows[i][20].ToString())
                        , float.Parse(d.dt.Rows[i][16].ToString())
                        , d.dt.Rows[i][23].ToString(), d.dt.Rows[i][24].ToString()
                        , d.dt.Rows[i][17].ToString(), float.Parse(d.dt.Rows[i][18].ToString())
                        , d.dt.Rows[i][21].ToString(), DateTime.Parse(d.dt.Rows[i][22].ToString())
                        , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                    }
                    // Add buttons to the last two columns
                    AddButtonsToLastTwoColumns();

                    datagridvColor();
                }
            }
            else if(comboBox1.Text=="Reviews" && textA.Text != "")
            {
                if (radiogreater.Checked == true && radioless2.Checked == false && radiobetween.Checked == false)
                {
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchTripadvisorGreaterReview";
                    d.cmdd.Parameters.Add("@review", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == true && radiobetween.Checked == false)
                {
                   
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchTripadvisorLesserReview";
                    d.cmdd.Parameters.Add("@review", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == false && radiobetween.Checked == true && textB.Text != "")
                {
                 

                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchTripadvisorBetweenReview";
                    d.cmdd.Parameters.Add("@review", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Parameters.Add("@review2", SqlDbType.Float).Value = float.Parse(textB.Text);
                    d.cmdd.Connection = d.cn;

                }
                else
                {
                    d.dt.Rows.Clear();
                    MessageBox.Show("The information entered is not on the database!");
                    return;
                }
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
                        dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString())
                        , float.Parse(d.dt.Rows[i][6].ToString()), float.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString()
                        , d.dt.Rows[i][9].ToString(), d.dt.Rows[i][15].ToString()
                        , float.Parse(d.dt.Rows[i][12].ToString()), float.Parse(d.dt.Rows[i][19].ToString())
                        , float.Parse(d.dt.Rows[i][14].ToString()), float.Parse(d.dt.Rows[i][20].ToString())
                        , float.Parse(d.dt.Rows[i][16].ToString())
                        , d.dt.Rows[i][23].ToString(), d.dt.Rows[i][24].ToString()
                        , d.dt.Rows[i][17].ToString(), float.Parse(d.dt.Rows[i][18].ToString())
                        , d.dt.Rows[i][21].ToString(), DateTime.Parse(d.dt.Rows[i][22].ToString())
                        , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                    }
                    // Add buttons to the last two columns
                    AddButtonsToLastTwoColumns();

                    datagridvColor();
                }
            }
            else
            {
                MessageBox.Show("The information entered is not on the database!");
            }
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

            comboBox3.DataSource = d.ds.Tables["Name"];
            comboBox3.DisplayMember = "Hotel_name";
            comboBox3.ValueMember = "Hotel_name";

            d.da = new SqlDataAdapter("select distinct [From] from cmprTripadvisor", d.cn);
            d.da.Fill(d.ds, "From");

            //Adding 'Please Select'
            DataRow HrRow = d.ds.Tables["From"].NewRow();
            HrRow[0] = "Please Select";
            d.ds.Tables["From"].Rows.InsertAt(HrRow, 0);

            comboBox2.DataSource = d.ds.Tables["From"];
            comboBox2.DisplayMember = "From";
            comboBox2.ValueMember = "From";

            d.da = new SqlDataAdapter("select distinct Board from cmprTripadvisor where Board <> ''", d.cn);
            d.da.Fill(d.ds, "Board");

            //Adding 'Please Select'
            DataRow BoardRow = d.ds.Tables["Board"].NewRow();
            BoardRow[0] = "Please Select";
            d.ds.Tables["Board"].Rows.InsertAt(BoardRow, 0);

            comboBox5.DataSource = d.ds.Tables["Board"];
            comboBox5.DisplayMember = "Board";
            comboBox5.ValueMember = "Board";

            comboBox4.SelectedItem = 0;
            comboBox4.SelectedText = "Please Select";
            comboBox4.Items.Add("3");
            comboBox4.Items.Add("4");
            comboBox4.Items.Add("5");

        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

           
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchTripadvisorHotel";
            d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());


            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString())
                , float.Parse(d.dt.Rows[i][6].ToString()), float.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString()
                , d.dt.Rows[i][9].ToString(), d.dt.Rows[i][15].ToString()
                , float.Parse(d.dt.Rows[i][12].ToString()), float.Parse(d.dt.Rows[i][19].ToString())
                , float.Parse(d.dt.Rows[i][14].ToString()), float.Parse(d.dt.Rows[i][20].ToString())
                , float.Parse(d.dt.Rows[i][16].ToString())
                , d.dt.Rows[i][23].ToString(), d.dt.Rows[i][24].ToString()
                , d.dt.Rows[i][17].ToString(), float.Parse(d.dt.Rows[i][18].ToString())
                , d.dt.Rows[i][21].ToString(), DateTime.Parse(d.dt.Rows[i][22].ToString())
                , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
            }

            // Add buttons to the last two columns
            AddButtonsToLastTwoColumns();

            datagridvColor();
        }
        DataTable dtnamehotel = new DataTable();
        DataTable dtnamehotel2 = new DataTable();
        DataTable dtnamehotel3 = new DataTable();
        private void Hotel_Load(object sender, EventArgs e)
        {
            B.Enabled = false;
            textB.Enabled = false;
            comboBox1.Items.Add("Price");
            comboBox1.Items.Add("Reviews");

            textB.Enabled = false;
            B.Enabled = false;

            d.connecter();
            comb();
            
            d.cmdd.CommandText = "select top 1 * from nameHotel order by id DESC";
            d.cmdd.Connection = d.cn;
            dtnamehotel.Load(d.cmdd.ExecuteReader());
            if (dtnamehotel.Rows.Count != 0)
            {
                adrs.Text = dtnamehotel.Rows[0][0].ToString();
                if (dtnamehotel.Rows[0][0].ToString() != dtnamehotel.Rows[0][1].ToString())
                    adrs.Text += "\n" + dtnamehotel.Rows[0][1].ToString();

                adrs.BackColor = Color.CornflowerBlue;
            }
            //*************
            d.cmdd.CommandText = "select top 1 * from nameHotelTRIVAGO order by id DESC";
            d.cmdd.Connection = d.cn;
            dtnamehotel2.Load(d.cmdd.ExecuteReader());
            if (dtnamehotel2.Rows.Count != 0)
            {
                label7.Text = dtnamehotel2.Rows[0][0].ToString();
                if (dtnamehotel2.Rows[0][0].ToString() != dtnamehotel2.Rows[0][1].ToString())
                    label7.Text += "\n" + dtnamehotel2.Rows[0][1].ToString();

                label7.BackColor = Color.CornflowerBlue;
            }
            //**************
            d.cmdd.CommandText = "select top 1 * from namefilesTRIPADVISOR order by id DESC";
            d.cmdd.Connection = d.cn;
            dtnamehotel3.Load(d.cmdd.ExecuteReader());
            if (dtnamehotel3.Rows.Count != 0)
            {
                label8.Text = dtnamehotel3.Rows[0][0].ToString();
                if (dtnamehotel3.Rows[0][0].ToString() != dtnamehotel3.Rows[0][1].ToString())
                    label8.Text += "\n" + dtnamehotel3.Rows[0][1].ToString();

                label8.BackColor = Color.CornflowerBlue;
            }

            if (code1!="null" && datehotel!="null")
              srechcode(code1,datehotel);
        }
        DataTable dtcode = new DataTable();
        public void srechcode(string a,string b)
        {
            MessageBox.Show(a);
            dtcode.Rows.Clear();
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchcodeHotel";
            d.cmdd.Parameters.Add("@code", SqlDbType.VarChar,20).Value = a;
            d.cmdd.Parameters.Add("@datehotel", SqlDbType.DateTime).Value = Convert.ToDateTime(b);
            d.cmdd.Connection = d.cn;
            dtcode.Load(d.cmdd.ExecuteReader());

            int cnt = dtcode.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString())
                , d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString()), float.Parse(d.dt.Rows[i][6].ToString())
                , d.dt.Rows[i][7].ToString(), int.Parse(d.dt.Rows[i][8].ToString()), d.dt.Rows[i][9].ToString()
                , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), d.dt.Rows[i][13].ToString());
            }

        }
        private void radiogreater_CheckedChanged(object sender, EventArgs e)
        {
            textB.Enabled = false;
            B.Enabled = false;
        }

        private void radioless2_CheckedChanged(object sender, EventArgs e)
        {

            textB.Enabled = false;
            B.Enabled = false;
        }

        private void radiobetween_CheckedChanged(object sender, EventArgs e)
        {

            textB.Enabled = true;
            B.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            upload_Tripadvisor ht = new upload_Tripadvisor();
            ht.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                dataGridView1.Rows.Clear();
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "searchDynamicAllOptionsTripadvisor";
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                d.cmdd.Connection = d.cn;
            }
            else if (comboBox1.Text == "Price")
            {
                if (radiogreater.Checked == true && radioless2.Checked == false && radiobetween.Checked == false)
                {
                    dataGridView1.Rows.Clear();
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDynamicAllOptionsPrice1Tripadvisor";
                    d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                    d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                    d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == true && radiobetween.Checked == false)
                {
                    dataGridView1.Rows.Clear();
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDynamicAllOptionsPrice2Tripadvisor";
                    d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                    d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                    d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;
                }
                else if (radiogreater.Checked == false && radioless2.Checked == false && radiobetween.Checked == true)
                {
                    dataGridView1.Rows.Clear();
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDynamicAllOptionsPrice3Tripadvisor";
                    d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                    d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                    d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(textB.Text);
                    d.cmdd.Connection = d.cn;
                }
            }
            else if (comboBox1.Text == "Reviews")
            {
                if (radiogreater.Checked == true && radioless2.Checked == false && radiobetween.Checked == false)
                {
                    dataGridView1.Rows.Clear();
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDynamicAllOptionsReview1Tripadvisor";
                    d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                    d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                    d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@review", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == true && radiobetween.Checked == false)
                {
                    dataGridView1.Rows.Clear();
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDynamicAllOptionsReview2Tripadvisor";
                    d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                    d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                    d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@review", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == false && radiobetween.Checked == true)
                {
                    dataGridView1.Rows.Clear();
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDynamicAllOptionsReview3Tripadvisor";
                    d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 100).Value = textBox1.Text.ToString();
                    d.cmdd.Parameters.Add("@Board", SqlDbType.VarChar, 100).Value = comboBox5.SelectedValue.ToString();
                    d.cmdd.Parameters.Add("@Star", SqlDbType.VarChar, 100).Value = comboBox4.Text != "Please Select" && comboBox4.Text != "" ? Convert.ToInt16(comboBox4.Text) : 10;
                    d.cmdd.Parameters.Add("@indate", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@outdate", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                    d.cmdd.Parameters.Add("@review", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Parameters.Add("@review2", SqlDbType.Float).Value = float.Parse(textB.Text);
                    d.cmdd.Connection = d.cn;

                }
            }

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
                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), DateTime.Parse(d.dt.Rows[i][2].ToString()), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(), float.Parse(d.dt.Rows[i][5].ToString())
                    , float.Parse(d.dt.Rows[i][6].ToString()), float.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString()
                    , d.dt.Rows[i][9].ToString(), d.dt.Rows[i][15].ToString()
                    , float.Parse(d.dt.Rows[i][12].ToString()), float.Parse(d.dt.Rows[i][19].ToString())
                    , float.Parse(d.dt.Rows[i][14].ToString()), float.Parse(d.dt.Rows[i][20].ToString())
                    , float.Parse(d.dt.Rows[i][16].ToString())
                    , d.dt.Rows[i][23].ToString(), d.dt.Rows[i][24].ToString()
                    , d.dt.Rows[i][17].ToString(), float.Parse(d.dt.Rows[i][18].ToString())
                    , d.dt.Rows[i][21].ToString(), DateTime.Parse(d.dt.Rows[i][22].ToString())
                    , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                // Add buttons to the last two columns
                AddButtonsToLastTwoColumns();

                datagridvColor();
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int lastColumnIndex = dataGridView1.Columns.Count - 1;
            int secondLastColumnIndex = dataGridView1.Columns.Count - 2;
            if (e.RowIndex >= 0 && (e.ColumnIndex == lastColumnIndex || e.ColumnIndex == secondLastColumnIndex))
            {
                HandleButtonClick(e.RowIndex, e.ColumnIndex);
            }
        }
        private void HandleButtonClick(int rowIndex, int columnIndex)
        {
            try
            {
                dataGridView1.Columns[11].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridView1.Columns[11].DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (rowIndex >= 0 && columnIndex >= 0)
                {
                    // Get the actual value from the underlying data source (e.g., DataTable)
                    var val = dataGridView1.Rows[rowIndex].Cells[columnIndex].Tag?.ToString();
                    string date = dataGridView1.Rows[rowIndex].Cells[3].Value?.ToString();

                    if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(date))
                    {
                        String[] spearator = { "https://" };
                        string[] tbl = val.Split(spearator, StringSplitOptions.None);
                        int cnt = tbl.Length;

                        if (cnt >= 2)
                        {
                            Process.Start(val);
                        }

                        // Additional logic for handling other cases can be added here

                        // Example: Open a form based on certain conditions
                        // if (someCondition)
                        // {
                        //     Hotel h = new Hotel(val, date);
                        //     h.Show();
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here if necessary
                Console.WriteLine("Error: " + ex.Message);
            }

            datagridvColor(); // Call your datagridvColor function if needed
        }



        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        dataGridView1.Columns[11].DefaultCellStyle.SelectionForeColor = Color.Blue;
        //        dataGridView1.Columns[11].DefaultCellStyle.SelectionBackColor = Color.White;
        //        dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        if (e.RowIndex > -1)
        //        {
        //            String[] spearator = { "https://" };

        //            var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
        //            string str = val;
        //            int index = e.RowIndex;
        //            string date = dataGridView1.Rows[index].Cells[3].Value.ToString();

        //            string[] tbl = str.Split(spearator, StringSplitOptions.None);
        //            int cnt = 0;
        //            cnt = tbl.Length;
        //            DataTable dthtl = new DataTable();
        //            if (cnt >= 2)
        //            {
        //                Process.Start(val);
        //            }

        //            for (int i = 0; i < dthtl.Rows.Count; i++)
        //            {
        //                if (str.Equals(dthtl.Rows[i][0].ToString()))
        //                {

        //                    Hotel h = new Hotel(str, date);
        //                    h.Show();
        //                }
        //            }

        //        }
        //    }
        //    catch { }
        //    datagridvColor();
        //}

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
