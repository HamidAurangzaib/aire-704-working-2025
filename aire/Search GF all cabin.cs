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
    public partial class Search_GF_all_cabin : Form
    {
        ado d = new ado();
        private readonly SynchronizationContext synchronizationcontext;

        public Search_GF_all_cabin()
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;

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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }
        }
        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();
        private void Search_GF_all_cabin_Load(object sender, EventArgs e)
        {
            d.connecter();
            label5.Visible = false;
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;


            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {

                searchfordata(textBox1.Text, textBox2.Text, "serchFromToGOOGleAll");

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {

                searchfordata(textBox1.Text, "", "serchFromGOOGleAll");
                datagridvColor();
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {

                searchfordata("", textBox2.Text, "serchToGOOGleAll");
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
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (checkBox7.Checked == true && checkBox8.Checked == false)
            {
                if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                {

                    somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "priceGOOGL1All");

                    datagridvColor();
                }
                else if (radioButton1.Checked && minPrice.Text != "")
                {
                    dataGridView1.Rows.Clear();

                    somme(float.Parse(minPrice.Text), 99999, "googlebigAll");

                    datagridvColor();
                }
                else if (radioButton2.Checked && minPrice.Text != "")
                {
                    dataGridView1.Rows.Clear();

                    somme(float.Parse(minPrice.Text), 99999, "googlelosAll");


                    datagridvColor();
                }


            }
            //end new price

            if (checkBox8.Text.Equals("Difference price") && checkBox7.Checked == false)
            {
                
                    if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                    {

                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "btwnOlde_pricericeGFAll");


                        datagridvColor();
                    }
                    else if (radioButton1.Checked == true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();


                        somme(float.Parse(minPrice.Text), 99999, "difgooglebigAll");


                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();


                        somme(float.Parse(minPrice.Text), 99999, "difgooglelosAll");


                        datagridvColor();
                    }
                

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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();


            dates("serchGGl1All");


            datagridvColor();
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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (checkBox7.Checked == true && checkBox8.Checked == false)
                {
                    if (radioButton1.Checked == true && minPrice.Text != "")
                    {

                        pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopriceGOOGlebigAll");


                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {

                        pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "");

                        datagridvColor();
                    }
                    else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "serchFromTopriceGOOGleAll")
                    {

                        pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopriceGOOGlebetweenAll");


                        datagridvColor();
                    }
                    else { MessageBox.Show("You must fill in the blank field "); }
                }

                else if (checkBox7.Checked == false && checkBox8.Checked == true)
                {
                    MessageBox.Show("You can only use the new price");
                }
            }
            else {
                MessageBox.Show("You must fill in the blank field FROM and TO");
                 }
        
            textBox1.Text = "";
            textBox2.Text = "";
            minPrice.Text = "";
            maxprice.Text = "";
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Columns[10].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridView1.Columns[10].DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                    MessageBox.Show(date);
                    for (int i = 0; i < dthtl.Rows.Count; i++)
                    {
                        if (str.Equals(dthtl.Rows[i][0].ToString()))
                        {

                            Hotel h = new Hotel(str, date);
                            h.Show();
                        }
                    }

                }
            }
            catch { }
            datagridvColor();
        }


        private void searchcabin(string str,string from,string to,string cabin)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            if (from != "" && to != "")
            {
                d.cmdd.Parameters.Add("@", SqlDbType.VarChar, 50).Value = from;
                d.cmdd.Parameters.Add("@", SqlDbType.VarChar, 50).Value = to;
            }
            d.cmdd.Parameters.Add("@", SqlDbType.VarChar, 50).Value = cabin;
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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {

            searchcabin("CabinAll", "", "", textBox4.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            searchcabin("PlaceCabinAll", textBox1.Text, textBox2.Text, textBox4.Text);
        }

        int pagenumber = 1;
        IPagedList<allGFcabin> list;

        public async Task<IPagedList<allGFcabin>> GetPagedListAsync(int pageNumber = 1, int pageSize = 10000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (allcabin db = new allcabin())
                {
                    return db.allGFcabins.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        private async void button6_Click(object sender, EventArgs e)
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

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Olde_price = 0 and New_price > 0", "New_price desc");
            foreach (DataRow dr in ligne)
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(),dr[10].ToString());
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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }

            datagridvColor();
            radioButton5.Checked = false;
        }
    }
}
