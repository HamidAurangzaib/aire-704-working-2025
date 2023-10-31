using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace aire
{
    public partial class interim : Form
    {
        ado d = new ado();
        public interim()
        {
            InitializeComponent();
        }

        private void interim_Load(object sender, EventArgs e)
        {
            d.connecter();     
            comboBox1.Items.Add("google");
            comboBox1.Items.Add("skyscanner");
            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }
        public void searchFROMTO(string frm,string to,string nameproc)
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
            else if(frm != "" && to == "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
            }
            else if(frm == "" && to != "")
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
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
            {
                searchFROMTO(textBox1.Text,textBox2.Text, "serchFromTointerim");

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
            {
                searchFROMTO(textBox1.Text,"", "serchFrominterim");
                datagridvColor();
            }
            else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
            {
                searchFROMTO("", textBox2.Text, "serchTointerim");
                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
            {


                searchFROMTO(textBox1.Text, textBox2.Text, "serchFromTointerimsky");
                datagridvColor();


            }
            else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
            {


                searchFROMTO(textBox1.Text,"", "serchFrominterimsky");
                datagridvColor();



            }
            else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
            {


                searchFROMTO("", textBox2.Text, "serchTointerimsky");
                datagridvColor();



            }
        }

        public void dates(string NameProc)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = NameProc;
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (checkBox6.Checked == true && checkBox5.Checked == false)
            {
                
                dates("serchinterim");
                datagridvColor();
            }
            else if (checkBox6.Checked == false && checkBox5.Checked == true)
            {

                dates("serchinterimsky");
                

                datagridvColor();
            }
        }
        public void price(float min , float max, string NameProc)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = NameProc;
            if (min != 99999 && max != 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = min;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = max;
            }

            else if (min != 99999 && max == 99999)
            { d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = min; }

            d.cmdd.Connection = d.cn;
            
            d.dt.Load(d.cmdd.ExecuteReader());
            ;
            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (checkBox7.Checked == true && checkBox8.Checked == false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {

                        price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "priceinterimbtwn");
                       

                        datagridvColor();
                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                       
                        price(float.Parse(minPrice.Text), 99999, "interimbig");
                        

                        datagridvColor();
                    }
                    else if (radioButton2.Checked && minPrice.Text != "")
                    {
                         
                        d.cmdd.Parameters.Add("@price1", SqlDbType.Int).Value = double.Parse(minPrice.Text);
                        price(float.Parse(minPrice.Text),99999, "interimlos");
                        
                        datagridvColor();
                    }
                }
                if (comboBox1.Text.Equals("skyscanner"))
                {
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {
                         
                        price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "priceinterimbtwnsky");
                        
                        datagridvColor();


                    }
                    else if (radioButton2.Checked && minPrice.Text != "")
                    {
                         
                        
                        price(float.Parse(minPrice.Text),99999, "interimlossky");
                         
                        datagridvColor();

                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                         
                       
                        price(float.Parse(minPrice.Text),99999, "interimbigsky");
                         

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
                        
                        price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "btwninterim");
                        

                        datagridvColor();
                    }
                    else if (radioButton1.Checked == true && minPrice.Text != "")
                    {
                         
                        price(float.Parse(minPrice.Text), 99999, "difinterimbig");
                         

                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {
                         
                        price(float.Parse(minPrice.Text), 99999, "difinterimlos");
                         

                        datagridvColor();
                    }
                }

                else if (comboBox1.Text.Equals("skyscanner"))
                {
                    if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                    {
                       
                        price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "btwninterimsky");
                       

                        datagridvColor();
                    }
                    else if (radioButton2.Checked == true && minPrice.Text != "")
                    {
                       
                        price(float.Parse(minPrice.Text), 99999, "difinterimlossky");
                       

                        datagridvColor();

                    }
                    else if (radioButton1.Checked == true && minPrice.Text != "")
                    {
                       
                        price(float.Parse(minPrice.Text), 99999, "difinterimbigsky");
                       

                        datagridvColor();
                    }

                }
            }
        }
        private void cabingoogle()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "Cabininterim";
            d.cmdd.Parameters.Add("@cabin", SqlDbType.VarChar, 20).Value = textBox3.Text;
            d.cmdd.Connection = d.cn;
             
            d.dt.Load(d.cmdd.ExecuteReader());
            
            int cnt =d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }
        }
        private void cabinskyscanner()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "Cabininterimsky";
            d.cmdd.Parameters.Add("@cabin", SqlDbType.VarChar, 20).Value = textBox3.Text;
            d.cmdd.Connection = d.cn;
             d.dt.Load(d.cmdd.ExecuteReader());
            
            int cnt =d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (chekGoogle.Checked == true && chekSkys.Checked == false)
            {
                cabingoogle();
                datagridvColor();
            }
            else if (chekSkys.Checked == true && chekGoogle.Checked == false)
            {
                cabinskyscanner();
                datagridvColor();
            }
        }

        private async void button7_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            ulpoad ul = new ulpoad();
            ul.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            upload_for_Skyscanner us = new upload_for_Skyscanner();
            us.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
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

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Olde_Price = 0 and New_Price > 0", "New_Price desc");
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
    }
}
