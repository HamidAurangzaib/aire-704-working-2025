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
using System.Diagnostics;


namespace aire
{
    public partial class Search_SKYS_GF_USA : Form
    {
        public Search_SKYS_GF_USA()
        {
            InitializeComponent();
        }
        ado d = new ado();
        DataTable dt = new DataTable();
        DataSet ds1 = new DataSet();
        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();

        private void Search_SKYS_GF_USA_Load(object sender, EventArgs e)
        {
            d.connecter();
            label5.Visible = false;
            comboBox1.Items.Add("google");
            comboBox1.Items.Add("skyscanner");
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;


            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }
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
                        if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Orange;
                        }
                        if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Gray;

                        }
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
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
        public int cnt = 0;
        public void searchfordata(string frm, string to, string nameProc)
        {
            d.dt.Rows.Clear();


            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;

            d.cmdd.CommandText = "" + nameProc + "";

            if (frm != "" && textBox2.Text == "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
            }


            else if (frm == "" && to != "")
            {
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            else if (frm != "" && to != "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {

                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
            {

                searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOGOOGleCOPY");

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
            {

                searchfordata(textBox1.Text, "", "serchFROMGOOGleCOPY");
                datagridvColor();
            }
            else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
            {

                searchfordata("", textBox2.Text, "serchTOGOOGleCOPY");
                datagridvColor();

            }
            else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
            {

                searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOskyCOPY");

                datagridvColor();


            }
            else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
            {


                searchfordata(textBox1.Text, "", "serchFROMskyCOPY");
                datagridvColor();



            }
            else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
            {

                searchfordata("", textBox2.Text, "serchTOskyCOPY");
                datagridvColor();

            }
        }
        public void somme(float a, float b, string str)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;

            if (a != 99999 && b == 99999)
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;

            else if (a == 99999 && b != 99999)
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;

            else if (a != 99999 && b != 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            }
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (checkBox7.Checked == true && checkBox8.Checked == false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {

                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "priceGOOGL1COPY");

                        datagridvColor();
                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(minPrice.Text), 99999, "googlebigCOPY");

                        datagridvColor();
                    }
                    else if (radioButton2.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(minPrice.Text), 99999, "googlelosCOPY");


                        datagridvColor();
                    }
                }
                if (comboBox1.Text.Equals("skyscanner"))
                {
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {
                        dataGridView1.Rows.Clear();



                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "priceskysc1COPY");


                        datagridvColor();


                    }
                    else if (radioButton2.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(minPrice.Text), 99999, "skyscannerlowCOPY");


                        datagridvColor();

                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(minPrice.Text), 99999, "skyscannerbigCOPY");


                        datagridvColor();
                    }

                }
            }
            if (checkBox8.Text.Equals("Difference price") && checkBox7.Checked == false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                    {

                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "btwnOlde_pricericeGFCOPY");


                        datagridvColor();
                    }
                    else if (radioButton1.Checked == true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();


                        somme(float.Parse(minPrice.Text), 99999, "difgooglebigCOPY");


                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();


                        somme(float.Parse(minPrice.Text), 99999, "difgooglelosCOPY");


                        datagridvColor();
                    }
                }

                else if (comboBox1.Text.Equals("skyscanner"))
                {
                    if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                    {

                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "btwnoldpriceskysCOPY");

                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(minPrice.Text), 99999, "difskylosCOPY");


                        datagridvColor();

                    }
                    else if (radioButton1.Checked == true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(minPrice.Text), 99999, "difskybigCOPY");

                        datagridvColor();
                    }

                }
            }
           
        }
        public void dates(string str)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");

            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;

            if (checkBox6.Checked == true && checkBox5.Checked == false)
            {
                dataGridView1.Rows.Clear();


                dates("serchGGl1COPY");


                datagridvColor();
            }
            else if (checkBox6.Checked == false && checkBox5.Checked == true)
            {
                dataGridView1.Rows.Clear();
                dates("serchskysc1COPY");

                datagridvColor();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;

            if (chekGoogle.Checked == true && chekSkys.Checked == false)
            {
                dataGridView1.Rows.Clear();
                cabingoogle("cabingoogleCOPY");
                datagridvColor();
            }
            else if (chekSkys.Checked == true && chekGoogle.Checked == false)
            {
                dataGridView1.Rows.Clear();
                cabingoogle("cabinskyCOPY");
                datagridvColor();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = true;
            label4.Visible = true;
            dataGridView1.Rows.Clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
            label4.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
            label4.Visible = false;
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
        private void cabingoogle(string str)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "cabingoogleCOPY";
            d.cmdd.Parameters.Add("@cabin", SqlDbType.VarChar, 20).Value = textBox3.Text;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }
        }

        private void chekGoogle_CheckedChanged(object sender, EventArgs e)
        {
            chekSkys.Checked = false;
            chekGoogle.Checked = true;
        }

        private void chekSkys_CheckedChanged(object sender, EventArgs e)
        {
            chekGoogle.Checked = false;
            chekSkys.Checked = true;
        }
        public void pricewithfrom_to(string frm, string to, float price1, float price2, string nameproce)
        {
            dataGridView1.Rows.Clear();
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproce;
            if (frm != "" && to != "" && price1 != 99999 && price2 != 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);
            }
            else if (frm != "" && to != "" && price1 != 99999 && price2 == 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
            }


            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == false && comboBox1.Text.Equals("google"))
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (checkBox7.Checked == true && checkBox8.Checked == false)
                    {
                        if (radioButton1.Checked == true && minPrice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopriceGOOGlebigCOPY");


                            datagridvColor();
                        }
                        else if (radioButton2.Checked == true && minPrice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopriceGOOGleCOPY");

                            datagridvColor();
                        }
                        else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopriceGOOGlebetweenCOPY");


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
            else if (checkBox1.Checked == false && checkBox2.Checked == true && comboBox1.Text.Equals("skyscanner"))
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (checkBox7.Checked == true && checkBox8.Checked == false)
                    {
                        if (radioButton1.Checked == true && minPrice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopriceskysbigCOPY");



                            datagridvColor();
                        }
                        else if (radioButton2.Checked == true && minPrice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopriceskyCOPY");


                            datagridvColor();
                        }

                        else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopriceskysbetweenCOPY");



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
            textBox1.Text = "";
            textBox2.Text = "";
            minPrice.Text = "";
            maxprice.Text = "";
        }
        int pagenumber = 1;
        IPagedList<comprGOOGLCOPY1> list;
        public async Task<IPagedList<comprGOOGLCOPY1>> GetPagedListAsync(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (allcopoy1 db = new allcopoy1())
                {
                    return db.comprGOOGLCOPY1.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        IPagedList<comprskyCOPY1> list1;
        public async Task<IPagedList<comprskyCOPY1>> GetPagedListAsync1(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (allcopoy1 db = new allcopoy1())
                {
                    return db.comprskyCOPY1.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        private void deleteclmn()
        {

            dataGridView2.Columns.Remove("id");

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
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Gray;

                    }
                }
            });
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            button8.Visible = true;
            button9.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
            if (GF.Checked == true && SKYS.Checked == false)
            {
                list = await GetPagedListAsync();
                button8.Enabled = list.HasPreviousPage;
                button9.Enabled = list.HasNextPage;
                dataGridView2.DataSource = list.ToList();
                label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                color();
                deleteclmn();
            }
            if (SKYS.Checked == true && GF.Checked == false)
            {
                list1 = await GetPagedListAsync1();
                button8.Enabled = list1.HasPreviousPage;
                button9.Enabled = list1.HasNextPage;
                dataGridView2.DataSource = list1.ToList();
                label6.Text = string.Format("page {0}/{1}", pagenumber, list1.PageCount);
                color();
                deleteclmn();
            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (list.HasPreviousPage)
            {
                if (GF.Checked == true)
                {
                    list = await GetPagedListAsync(--pagenumber);
                    button8.Enabled = list.HasPreviousPage;
                    button9.Enabled = list.HasNextPage;
                    dataGridView2.DataSource = list.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                    color();
                    deleteclmn();
                }
                if (SKYS.Checked == true)
                {
                    list1 = await GetPagedListAsync1(--pagenumber);
                    button8.Enabled = list1.HasPreviousPage;
                    button9.Enabled = list1.HasNextPage;
                    dataGridView2.DataSource = list1.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list1.PageCount);
                    color();
                    deleteclmn();
                }
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            if (list.HasNextPage)
            {
                if (GF.Checked == true)
                {
                    list = await GetPagedListAsync(++pagenumber);
                    button8.Enabled = list.HasPreviousPage;
                    button9.Enabled = list.HasNextPage;
                    dataGridView2.DataSource = list.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                    color();
                    deleteclmn();
                }
                if (SKYS.Checked == true)
                {
                    list1 = await GetPagedListAsync1(++pagenumber);
                    button8.Enabled = list1.HasPreviousPage;
                    button9.Enabled = list1.HasNextPage;
                    dataGridView2.DataSource = list1.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list1.PageCount);
                    color();
                    deleteclmn();
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Columns[9].DefaultCellStyle.SelectionForeColor = Color.Blue;
            dataGridView2.Columns[9].DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView2.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (e.RowIndex > -1)
            {
                String[] spearator = { "https://" };

                var val = this.dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();
                string str = val;



                string[] tbl = str.Split(spearator, StringSplitOptions.None);
                cnt = 0;
                cnt = tbl.Length;

                if (cnt >= 2)
                {
                    Process.Start(val);
                }
            }
            color();
        }

        private void GF_CheckedChanged(object sender, EventArgs e)
        {
            SKYS.Checked = false;
        }

        private void SKYS_CheckedChanged(object sender, EventArgs e)
        {
            GF.Checked = false;
        }
        private void citygoogle(string str)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = textBox4.Text;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (checkBox4.Checked == true && checkBox3.Checked == false)
            {
                citygoogle("citysGFCOPY");
                datagridvColor();
            }
            else if (checkBox3.Checked == true && checkBox4.Checked == false)
            {
                citygoogle("citysskyCOPY");
                datagridvColor();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Columns[9].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridView1.Columns[9].DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (e.RowIndex > -1)
                {
                    String[] spearator = { "https://" };

                    var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string str = val;
                    int index = e.RowIndex;
                    string date = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    string[] tbl = str.Split(spearator, StringSplitOptions.None);
                    cnt = 0;
                    cnt = tbl.Length;

                    if (cnt >= 2)
                    {
                        Process.Start(val);
                    }

                    for (int i = 0; i < dthtl.Rows.Count; i++)
                    {
                        if (str.Equals(dthtl.Rows[i][0].ToString()))
                        {

                            Hotel h = new Hotel(str,date);
                            h.Show();
                        }
                    }

                }
            }
            catch { }
            datagridvColor();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Olde_price = 0 and New_price > 0", "New_price desc");
            foreach (DataRow dr in ligne)
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString());
            }

            datagridvColor();
            ligne = null;
            radioButton4.Checked = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int cntDt = d.dt.Rows.Count;
            for (int i = 0; i < cntDt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString());
            }

            datagridvColor();
            radioButton5.Checked = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            google_usa usa = new google_usa();
            usa.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SKYS_USA su = new SKYS_USA();
            su.Show();
        }
    }
}
