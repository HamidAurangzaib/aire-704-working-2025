using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using PagedList;
using System.Linq;

namespace aire
{
    public partial class NEW_SKYS_GF_USA : Form
    {
        ado d = new ado();
        public NEW_SKYS_GF_USA()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        DataSet ds1 = new DataSet();
        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();
        public async void datagridvColor()
        {

            try
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
            catch { }
        }
        private void comb()
        {
            d.da = new SqlDataAdapter("select distinct [From] from newskyflightcopy1", d.cn);
            d.da.Fill(d.ds, "com1");
            d.da = new SqlDataAdapter("select distinct [To] from newskyflightcopy1", d.cn);
            d.da.Fill(ds1, "com2");
            comboBox2.DataSource = d.ds.Tables["com1"];
            comboBox2.DisplayMember = "From";
            comboBox2.ValueMember = "From";

            comboBox3.DataSource = ds1.Tables["com2"];
            comboBox3.DisplayMember = "To";
            comboBox3.ValueMember = "To";
        }
        public void searchFROMTO(string frm, string to, string nameproc)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproc;
            if (frm != "" && to != "")
            {

                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            else if (frm != "" && to == "")
            {

                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;

            }
            else if (frm == "" && to != "")
            {


                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                   double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()));

            }


        }

        private void NEW_SKYS_GF_USA_Load(object sender, EventArgs e)
        {
            d.connecter();

            d.cmdd = new SqlCommand("exec insertnewskyflightcopy1", d.cn);
            d.cmdd.ExecuteNonQuery();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            comb();
            textBox1.Text = "";
            textBox2.Text = "";

            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                searchFROMTO(textBox1.Text, textBox2.Text, "serchFromTonewskyflightcopy1");

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                searchFROMTO(textBox1.Text, "", "serchFromnewskyflightcopy1");
                datagridvColor();
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchFROMTO("", textBox2.Text, "serchTonewskyflighcopy1");

                datagridvColor();

            }
        }
        public void price_place(string frm, string to, float a, float b, string nameproc)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproc;
            if (frm != "" && to != "" && a != 99999 && b != 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            }
            else if (frm != "" && to != "" && a != 99999 && b == 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
            }


            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                   double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()));
            }
            textBox1.Text = "";
            textBox2.Text = "";
            minPrice.Text = "";
            maxprice.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (minPrice.Text != "" || maxprice.Text != "")
            {
                if (checkBox7.Checked == true)
                {
                    if (radioButton1.Checked == true)
                    {

                        price(float.Parse(minPrice.Text), 99999, "pricenewskyflightcopy11");


                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true)
                    {

                        price(float.Parse(minPrice.Text), 99999, "pricenewskyflightcopy1low");
                        datagridvColor();

                    }
                    else if (radioButton3.Checked == true)
                    {

                        price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "pricenewskyflightcopy11");


                        datagridvColor();
                    }
                }
                if (checkBox8.Checked == true)
                {

                    if (radioButton1.Checked == true)
                    {

                        price(float.Parse(minPrice.Text), 99999, "diffnewskyflightcopy11");


                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true)
                    {

                        price(float.Parse(minPrice.Text), 99999, "diffnewskyflightcopy1low");

                        datagridvColor();
                    }
                    else if (radioButton3.Checked == true)
                    {

                        price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "diffnewskyflightcopy11");


                        datagridvColor();
                    }
                }

                else if (checkBox7.Checked == false && checkBox8.Checked == false) { MessageBox.Show("You can only use the price flight or price skyscanner"); }
            }
            else { MessageBox.Show("You must fill in the blank field A or B"); }
        }

        public void price(float min, float max, string nameproce)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproce;
            if (min != 99999 && max == 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = min;
            }
            else if (min != 99999 && max != 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = min;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = max;
            }

            d.cmdd.Connection = d.cn;
            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()));

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "datenewskyflightcopy1";
            d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");

            d.cmdd.Connection = d.cn;
            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                   double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()));

            }

            datagridvColor();
        }
        private void citynexgfsky()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "citysGFSKYScopy1";
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = textBox4.Text;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                   double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()));

            }

        }
        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            citynexgfsky();
            datagridvColor();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (checkBox7.Checked == true)
                {
                    if (radioButton1.Checked == true)
                    {
                        if (minPrice.Text != "")
                        {

                            price_place(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopricenewskyflighbigcopy1");


                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field A "); }
                    }
                    else if (radioButton2.Checked == true)
                    {
                        if (minPrice.Text != "")
                        {
                            price_place(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopricenewskyflighcopy1");


                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field A "); }
                    }
                    else if (radioButton3.Checked == true)
                    {
                        if (minPrice.Text != "" && maxprice.Text != "")
                        {

                            price_place(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopricenewskyflighbetweencopy1");


                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field A or B"); }
                    }
                }
                if (checkBox8.Checked == true)
                {

                    if (radioButton1.Checked == true)
                    {
                        if (minPrice.Text != "")
                        {
                            price_place(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopricenewskyflighbig2copy1");


                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field A "); }
                    }
                    else if (radioButton2.Checked == true)
                    {

                        if (minPrice.Text != "")
                        {

                            price_place(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopricenewskyfligh2copy1");


                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field A "); }
                    }
                    else if (radioButton3.Checked == true)
                    {
                        if (minPrice.Text != "" && maxprice.Text != "")
                        {


                            price_place(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopricenewskyflighbetween2copy1");


                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field A or B"); }
                    }


                }
                else if (checkBox7.Checked == true && checkBox8.Checked == true)
                {
                    MessageBox.Show("You can only use the price flight or price skyscanner");
                }

            }
            else
            {
                MessageBox.Show("You must fill in the blank field FROM or TO");
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            checkBox8.Checked = false;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            checkBox7.Checked = false;
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
        private void deleteclmn()
        {

            dataGridView2.Columns.Remove("id");

        }
        string str;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                str = val;
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
            catch
            {

            }
            datagridvColor();
        }
        private async void color()
        {
            await Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (Convert.ToDouble(row.Cells[6].Value) < 0)
                    {
                        row.Cells[6].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Red;
                    }

                }
            });
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox2.SelectedValue.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox3.SelectedValue.ToString();
        }
    }
}
