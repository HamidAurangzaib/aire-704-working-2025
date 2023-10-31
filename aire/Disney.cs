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
    public partial class Disney : Form
    {
        string code1="null",datehotel;
        ado d = new ado();
        string str;
        public Disney(string code,string dateh)
        {
            InitializeComponent();
            code1 = code;
            datehotel = dateh;
        }
        private void comb()
        {
            d.ds.Clear();
            
            d.da = new SqlDataAdapter("select distinct Hotel_dropdown from cmprDisney", d.cn);
            d.da.Fill(d.ds, "Category");

            //Adding 'Please Select'
            DataRow HdRow = d.ds.Tables["Category"].NewRow();
            HdRow[0] = "Please Select";
            d.ds.Tables["Category"].Rows.InsertAt(HdRow, 0);

            comboBox2.DataSource = d.ds.Tables["Category"];
            comboBox2.DisplayMember = "Hotel_dropdown";
            comboBox2.ValueMember = "Hotel_dropdown";

            d.da = new SqlDataAdapter("select distinct Hotel_resort from cmprDisney", d.cn);
            d.da.Fill(d.ds, "Name");

            //Adding 'Please Select'
            DataRow HrRow = d.ds.Tables["Name"].NewRow();
            HrRow[0] = "Please Select";
            d.ds.Tables["Name"].Rows.InsertAt(HrRow, 0);

            comboBox3.DataSource = d.ds.Tables["Name"];
            comboBox3.DisplayMember = "Hotel_resort";
            comboBox3.ValueMember = "Hotel_resort";

        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            
            //if (comboBox2.SelectedValue.ToString() != "" && comboBox3.SelectedValue.ToString() == "")
            //{
            //    d.dt.Rows.Clear();
            //    d.cmdd.Parameters.Clear();
            //    d.cmdd.CommandType = CommandType.StoredProcedure;
            //    d.cmdd.CommandText = "searchCategoryDisney";
            //    d.cmdd.Parameters.Add("@Category", SqlDbType.VarChar,100).Value = comboBox2.SelectedValue.ToString();
            //    d.cmdd.Connection = d.cn;
            //}
            //else if(comboBox2.SelectedValue.ToString() == "" && comboBox3.SelectedValue.ToString() != "")
            //{
            //    d.dt.Rows.Clear();
            //    d.cmdd.Parameters.Clear();
            //    d.cmdd.CommandType = CommandType.StoredProcedure;
            //    d.cmdd.CommandText = "searchNameDisney";
            //    d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();

            //    d.cmdd.Connection = d.cn;
            //}
            //else if (comboBox2.SelectedValue.ToString() != "" && comboBox3.SelectedValue.ToString() != "")
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchDynamicDisney";
            d.cmdd.Parameters.Add("@Category", SqlDbType.VarChar, 100).Value = comboBox2.SelectedValue.ToString();
            d.cmdd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = comboBox3.SelectedValue.ToString();
            d.cmdd.Parameters.Add("@IncludeFreeDine", SqlDbType.Bit, 100).Value = checkBoxFreeDine.CheckState;

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
                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), DateTime.Parse(d.dt.Rows[i][1].ToString()), float.Parse(d.dt.Rows[i][2].ToString()), float.Parse(d.dt.Rows[i][3].ToString())
                    , float.Parse(d.dt.Rows[i][4].ToString()), float.Parse(d.dt.Rows[i][5].ToString()), float.Parse(d.dt.Rows[i][6].ToString())
                    , d.dt.Rows[i][7].ToString(), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString()
                    , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), d.dt.Rows[i][13].ToString()
                    , d.dt.Rows[i][14].ToString(), d.dt.Rows[i][17].ToString(), d.dt.Rows[i][15].ToString(), float.Parse(d.dt.Rows[i][18].ToString()), d.dt.Rows[i][16].ToString());
                }

                datagridvColor();
            }
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            d.dt.Rows.Clear();
           

            if(comboBox1.Text=="Total Price")
            {
                if(radiogreater.Checked==true && radioless2.Checked==false && radiobetween.Checked==false)
                {
                   
                   
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDisneyGreaterTotalPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);

                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == true && radiobetween.Checked == false)
                {
                   

                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDisneyLesserTotalPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;
                }
                else if (radiogreater.Checked == false && radioless2.Checked == false && radiobetween.Checked == true)
                {
                  
                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDisneyBetweenTotalPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(textB.Text);
                    d.cmdd.Connection = d.cn;
                }
            }
            else if(comboBox1.Text=="Hotel Price")
            {
                if (radiogreater.Checked == true && radioless2.Checked == false && radiobetween.Checked == false)
                {


                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDisneyGreaterHotelPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);

                    d.cmdd.Connection = d.cn;

                }
                else if (radiogreater.Checked == false && radioless2.Checked == true && radiobetween.Checked == false)
                {


                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDisneyLesserHotelPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Connection = d.cn;
                }
                else if (radiogreater.Checked == false && radioless2.Checked == false && radiobetween.Checked == true)
                {

                    d.dt.Rows.Clear();
                    d.cmdd.Parameters.Clear();
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    d.cmdd.CommandText = "searchDisneyBetweenHotelPrice";
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(textA.Text);
                    d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(textB.Text);
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
                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), DateTime.Parse(d.dt.Rows[i][1].ToString()), float.Parse(d.dt.Rows[i][2].ToString()), float.Parse(d.dt.Rows[i][3].ToString())
                    , float.Parse(d.dt.Rows[i][4].ToString()), float.Parse(d.dt.Rows[i][5].ToString()), float.Parse(d.dt.Rows[i][6].ToString())
                    , d.dt.Rows[i][7].ToString(), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString()
                    , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), d.dt.Rows[i][13].ToString()
                    , d.dt.Rows[i][14].ToString(), d.dt.Rows[i][17].ToString(), d.dt.Rows[i][15].ToString(), float.Parse(d.dt.Rows[i][18].ToString()), d.dt.Rows[i][16].ToString());
                }

                datagridvColor();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

           
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchdateDisney";
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
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), DateTime.Parse(d.dt.Rows[i][1].ToString()), float.Parse(d.dt.Rows[i][2].ToString()), float.Parse(d.dt.Rows[i][3].ToString())
                    , float.Parse(d.dt.Rows[i][4].ToString()), float.Parse(d.dt.Rows[i][5].ToString()), float.Parse(d.dt.Rows[i][6].ToString())
                    , d.dt.Rows[i][7].ToString(), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString()
                    , d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), d.dt.Rows[i][13].ToString()
                    , d.dt.Rows[i][14].ToString(), d.dt.Rows[i][17].ToString(), d.dt.Rows[i][15].ToString(), float.Parse(d.dt.Rows[i][18].ToString()), d.dt.Rows[i][16].ToString());
            }

            datagridvColor();
        }
        DataTable dtnamehotel = new DataTable();
        DataTable dtnamehotel2 = new DataTable();
        DataTable dtnamehotel3 = new DataTable();
        private void Hotel_Load(object sender, EventArgs e)
        {
            B.Enabled = false;
            textB.Enabled = false;
            comboBox1.Items.Add("Total Price");
            comboBox1.Items.Add("Hotel Price");

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
            d.cmdd.CommandText = "select top 1 * from namefilesDisney order by id DESC";
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
        public async void datagridvColor()
        {

            try
            {
                await Task.Run(() =>
                {

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToDouble(row.Cells[17].Value) < 0)
                        {
                            row.Cells[17].Style.BackColor = Color.LightGreen;
                        }
                        else if (Convert.ToDouble(row.Cells[17].Value) > 0)
                        {
                            row.Cells[17].Style.BackColor = Color.Red;
                        }
                        if (Convert.ToDouble(row.Cells[17].Value) == 0 && Convert.ToDouble(row.Cells[15].Value != null ? (row.Cells[15].Value.ToString() != "Call us" && row.Cells[15].Value.ToString() != "#EANF#" && row.Cells[15].Value.ToString() != "" ? row.Cells[15].Value : 0) : 0) == 0 && Convert.ToDouble(row.Cells[16].Value != null ? (row.Cells[16].Value.ToString() != "Call us" && row.Cells[16].Value.ToString() != "#EANF#" && row.Cells[16].Value.ToString() != "" ? row.Cells[16].Value : 0) : 0) > 0)
                        {
                            row.Cells[17].Style.BackColor = Color.Orange;
                        }
                        if (Convert.ToDouble(row.Cells[17].Value) == 0 && Convert.ToDouble(row.Cells[15].Value != null ? (row.Cells[15].Value.ToString() != "Call us" && row.Cells[15].Value.ToString() != "#EANF#" && row.Cells[15].Value.ToString() != "" ? row.Cells[15].Value : 0) : 0) > 0 && Convert.ToDouble(row.Cells[16].Value != null ? (row.Cells[16].Value.ToString() != "Call us" && row.Cells[16].Value.ToString() != "#EANF#" && row.Cells[16].Value.ToString() != "" ? row.Cells[16].Value : 0) : 0) == 0)
                        {
                            row.Cells[17].Style.BackColor = Color.Gray;
                        }
                    }
                });
            }
            catch { }
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
            upload_Disney Disney = new upload_Disney();
            Disney.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
