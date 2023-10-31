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
    public partial class each_output_all_cabin : Form
    {
        private readonly SynchronizationContext synchronizationcontext;
        public each_output_all_cabin()
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;
        }
        ado d = new ado();
        DataSet ds1 = new DataSet();
        private void comb()
        {
            d.da = new SqlDataAdapter("select distinct [From] from each_outputALL", d.cn);
            d.da.Fill(d.ds, "com1");
            d.da = new SqlDataAdapter("select distinct [To] from each_outputALL", d.cn);
            d.da.Fill(ds1, "com2");
            comboBox2.DataSource = d.ds.Tables["com1"];
            comboBox2.DisplayMember = "From";
            comboBox2.ValueMember = "From";

            comboBox3.DataSource = ds1.Tables["com2"];
            comboBox3.DisplayMember = "To";
            comboBox3.ValueMember = "To";
        }


        private void each_output_all_cabin_Load(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            d.connecter();
            comb();
            button8.Visible = false;
            button9.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";

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
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
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
        public void searchFROMTO(string frm, string to, string NameProc)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = NameProc;
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
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());

            }


        }
        string cbnB1, cbnB2, cbnB3, itxdate;

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

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = true;
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

        private void button3_Click(object sender, EventArgs e)
        {
            button8.Visible = false;
            button9.Visible = false;
            label5.Text = "";
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();

                itxdate = "serchoutputALL";
           
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = itxdate;
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
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }

            datagridvColor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button8.Visible = false;
            button9.Visible = false;
            label5.Text = "";
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();

          
                Iprice1 = "priceoutputALL";
                Iprice2 = "priceoutput1ALL";
                Iprice3 = "priceoutputlowALL";
                Iprice4 = "diffoutputALL";
                Iprice5 = "diffoutput1ALL";
                Iprice6 = "diffoutputlowALL";
            
          
           
            if (checkBox7.Checked == true && checkBox8.Checked == false)
            {

                if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                {



                    price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), Iprice1);


                    datagridvColor();
                }
                else if (radioButton1.Checked && minPrice.Text != "")
                {

                    price(float.Parse(minPrice.Text), 99999, Iprice2);

                    datagridvColor();
                }
                else if (radioButton2.Checked && minPrice.Text != "")
                {

                    price(float.Parse(minPrice.Text), 99999, Iprice3);

                    datagridvColor();
                }

            }
            else if (checkBox8.Checked == true && checkBox7.Checked == false)
            {
                if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                {


                    price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), Iprice4);

                    datagridvColor();
                }
                else if (radioButton1.Checked && minPrice.Text != "")
                {



                    price(float.Parse(minPrice.Text), 99999, Iprice5);

                    datagridvColor();
                }
                else if (radioButton2.Checked && minPrice.Text != "")
                {

                    price(float.Parse(minPrice.Text), 99999, Iprice6);


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

        private void button1_Click(object sender, EventArgs e)
        {
            
                cbnB1 = "serchFromtooutputALL";
                cbnB2 = "serchFromoutputALL";
                cbnB3 = "serchTooutputALL";
          
            button8.Visible = false;
            button9.Visible = false;
            label5.Text = "";
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                searchFROMTO(textBox1.Text, textBox2.Text, cbnB1);

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                searchFROMTO(textBox1.Text, "", cbnB2);
                datagridvColor();
                textBox1.Text = "";
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchFROMTO("", textBox2.Text, cbnB3);

                datagridvColor();
                textBox2.Text = "";
            }
        }


        public void price(float a, float b, string NameProc)
        {

            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = NameProc;
            if (a != 99999 && b != 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            }
            else if (a != 99999 && b == 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
            }
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }
        }
        int pagenumber = 1;
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

        string Iprice1, Iprice2, Iprice3, Iprice4, Iprice5, Iprice6;




        private void cabinoutput(string str,string from,string to)
        {
           
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = textBox4.Text;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            button8.Visible = false;
            button9.Visible = false;
            label5.Text = "";
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if(Rjustcabin.Checked==true && textBox4.Text!="")
            cabinoutput("citysoutputB","","");
            else if(RcabinFromTo.Checked==true && textBox4.Text!="")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    cabinoutput("citysoutputB", textBox1.Text, textBox2.Text);
                }
                else
                {
                    MessageBox.Show("Fill in the blank field From and TO");
                }
            }
            datagridvColor();


        }

        public void pricewithfrmto(string frm, string to, float a, float b, string NameProc)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = NameProc;

            if (frm != "" && to != "" && a != 99999 && b != 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = a;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            }
            else if (frm != "" && to != "" && a != 99999 && b == 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = a;
            }

            d.cmdd.Connection = d.cn;
            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }
        }

        string p1, p2, p3;
        private void button7_Click(object sender, EventArgs e)
        {
            button8.Visible = false;
            button9.Visible = false;
            label5.Text = "";
            d.dt.Rows.Clear();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
           

                p1 = "serchFromTopriceoutputbigALL";
                p2 = "serchFromTopriceoutputALL";
                p3 = "serchFromTopriceoutputbetweenALL";

           
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (checkBox7.Checked == true && checkBox8.Checked == false)
                {
                    if (radioButton1.Checked == true && minPrice.Text != "")
                    {

                        pricewithfrmto(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, p1);

                        textBox1.Text = "";
                        textBox2.Text = "";
                        minPrice.Text = "";
                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {

                        pricewithfrmto(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, p2);

                        textBox1.Text = "";
                        textBox2.Text = "";
                        minPrice.Text = "";
                        datagridvColor();
                    }
                    else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                    {

                        pricewithfrmto(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), p3);

                        textBox1.Text = "";
                        textBox2.Text = "";
                        minPrice.Text = "";
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

        IPagedList<each_outputALL> list;

        public async Task<IPagedList<each_outputALL>> GetPagedListAsync(int pageNumber = 1, int pageSize = 10000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (allcabin db = new allcabin())
                {
                    return db.each_outputALL.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
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

    }
}
