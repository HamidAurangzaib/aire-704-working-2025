using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Diagnostics;

namespace aire
{
    public partial class Search_Easyjet : Form
    {
        public Search_Easyjet()
        {
            InitializeComponent();
        }
        ado d = new ado();

        private void Search_Easyjet_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // or DataGridViewAutoSizeColumnMode.DisplayedCells
            }

            d.connecter();
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select * from nameEasyjet", d.cn);
            d.ds = new DataSet();
            int count;
            d.da.Fill(d.ds, "hldy");
            count = d.ds.Tables["hldy"].Rows.Count;
            if (count > 0)
            {
                label1.Text += d.ds.Tables["hldy"].Rows[0][1].ToString();
                if(count > 1)label2.Text += d.ds.Tables["hldy"].Rows[1][1].ToString();


            }

            List<string> lst = new List<string>();
            lst.Add("0");
            lst.Add("1");
            lst.Add("2");
            lst.Add("3");
            lst.Add("4");
            lst.Add("5");
            
            List<string> lst1 = new List<string>();
            lst1.Add("01");
            lst1.Add("02");
            lst1.Add("03");
            lst1.Add("04");
            lst1.Add("05");
            lst1.Add("06");
            lst1.Add("07");
            lst1.Add("08");
            lst1.Add("09");
            lst1.Add("10");
            lst1.Add("11");
            lst1.Add("12");
            comboBox1.DataSource = lst;
            
            comboBox2.DataSource=lst1;
        }
        public async void datagridvColor()
        {

            try
            {
                await Task.Run(() =>
                {

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {

                        if (Convert.ToDouble(row.Cells[10].Value) < 0)
                        {
                            row.Cells[10].Style.BackColor = Color.LightGreen;
                        }
                        else if (Convert.ToDouble(row.Cells[10].Value) > 0)
                        {
                            row.Cells[10].Style.BackColor = Color.Red;
                        }
                        if (Convert.ToDouble(row.Cells[10].Value) == 0 && Convert.ToDouble(row.Cells[8].Value) == 0 && Convert.ToDouble(row.Cells[9].Value) > 0)
                        {
                            row.Cells[10].Style.BackColor = Color.Orange;
                        }
                        if (Convert.ToDouble(row.Cells[10].Value) == 0 && Convert.ToDouble(row.Cells[8].Value) > 0 && Convert.ToDouble(row.Cells[9].Value) == 0)
                        {
                            row.Cells[10].Style.BackColor = Color.Gray;

                        }
                    }
                    
                });
            }
            catch { }
        }
        private void fnctionSreachDates(string nameProc, string value1, string value2)
        {
            if (value1 == "FROM") value1 = "";
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            dataGridView2.Rows.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "" + nameProc + "";
            d.cmdd.Parameters.Add("@Date1", SqlDbType.Date).Value = value1;
            d.cmdd.Parameters.Add("@Date2", SqlDbType.Date).Value = value2;
            
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {

                dataGridView2.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), d.dt.Rows[i][3].ToString(),
                      d.dt.Rows[i][4].ToString(), DateTime.Parse(d.dt.Rows[i][5].ToString()), DateTime.Parse(d.dt.Rows[i][6].ToString()), d.dt.Rows[i][7].ToString(),
                      double.Parse(d.dt.Rows[i][8].ToString()), double.Parse(d.dt.Rows[i][9].ToString()), double.Parse(d.dt.Rows[i][10].ToString()), d.dt.Rows[i][11].ToString(),
                      d.dt.Rows[i][12].ToString(), int.Parse(d.dt.Rows[i][13].ToString()), int.Parse(d.dt.Rows[i][14].ToString()),
                      d.dt.Rows[i][15].ToString(), d.dt.Rows[i][16].ToString(), d.dt.Rows[i][17].ToString(), d.dt.Rows[i][19].ToString(),
                      d.dt.Rows[i][20].ToString(), DateTime.Parse(d.dt.Rows[i][21].ToString()), d.dt.Rows[i][18].ToString());

            }
            datagridvColor();
        }
        private void fnctionSreach(string nameProc,string value1, string value2,int nbr1,int nbr2)
        {
            if (value1 == "FROM") value1 = "";
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            dataGridView2.Rows.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = ""+nameProc+"";
            if(nbr1==0)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = value1;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = value2;
            }
           else if (nbr1 == 1)
            {
                d.cmdd.Parameters.Add("@Arrive", SqlDbType.Date).Value = value1;
            }
           else if (nbr1 == 2)
            {
                d.cmdd.Parameters.Add("@Hotelname", SqlDbType.VarChar).Value = value1;
            }
            else if (nbr1 == 3)
            {
                d.cmdd.Parameters.Add("@star", SqlDbType.Int).Value = int.Parse(value1);
            }
            else if (nbr1 == 4)
            {
                d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = float.Parse(value1);
                d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = float.Parse(value2);
                d.cmdd.Parameters.Add("@nbr", SqlDbType.Int).Value = nbr2;
            }
            else if (nbr1 == 5)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = value1;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = value2;
                d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = float.Parse(textBox6.Text);
                d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = float.Parse(textBox6.Text);
              
            }
            else if (nbr1 == 6)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = value1;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = value2;
                d.cmdd.Parameters.Add("@month", SqlDbType.Int).Value = int.Parse(comboBox2.Text);
                
            }
            else if (nbr1 == 7)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = value1;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = value2;
                d.cmdd.Parameters.Add("@date", SqlDbType.Date).Value = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                
            }
            else if(nbr1==8)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = value2;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = value1;
                d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = float.Parse(textBox15.Text);
                d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = float.Parse(textBox14.Text);
                d.cmdd.Parameters.Add("@board", SqlDbType.VarChar, 20).Value = textBox16.Text;
                d.cmdd.Parameters.Add("@Transfers", SqlDbType.VarChar, 20).Value = textBox17.Text;
                d.cmdd.Parameters.Add("@Star", SqlDbType.Int).Value = int.Parse(textBox18.Text);

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

                dataGridView2.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), d.dt.Rows[i][3].ToString(),
                      d.dt.Rows[i][4].ToString(), DateTime.Parse(d.dt.Rows[i][5].ToString()), DateTime.Parse(d.dt.Rows[i][6].ToString()), d.dt.Rows[i][7].ToString(),
                      double.Parse(d.dt.Rows[i][8].ToString()), double.Parse(d.dt.Rows[i][9].ToString()), double.Parse(d.dt.Rows[i][10].ToString()), d.dt.Rows[i][11].ToString(),
                      d.dt.Rows[i][12].ToString(), int.Parse(d.dt.Rows[i][13].ToString()), int.Parse(d.dt.Rows[i][14].ToString()),
                      d.dt.Rows[i][15].ToString(), d.dt.Rows[i][16].ToString(), d.dt.Rows[i][17].ToString(), d.dt.Rows[i][19].ToString(),
                      d.dt.Rows[i][20].ToString(), DateTime.Parse(d.dt.Rows[i][21].ToString()), d.dt.Rows[i][18].ToString());

            }
            datagridvColor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="" || textBox2.Text!="")
            {
                fnctionSreach("affichFromTo", textBox1.Text, textBox2.Text,0,0);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            dateTimePicker2.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = false;
            dateTimePicker2.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                fnctionSreach("SearchMonthFromTo", textBox11.Text, textBox10.Text, 6, 0);
            }
            else if(radioButton2.Checked==true)
            {
                fnctionSreach("SearchDateFromTo", textBox11.Text, textBox10.Text, 7, 0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fnctionSreachDates("SearchDate", dateTimePicker1.Value.ToString("yyyy/MM/dd"), dateTimePicker3.Value.ToString("yyyy/MM/dd"));
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fnctionSreach("SearchHotelName",textBox9.Text , "", 2, 0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text!="0")
            fnctionSreach("SearchStar", comboBox1.Text, "", 3, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(radioButton4.Checked==true)
            {
                fnctionSreach("SearchPrice", textBox3.Text, textBox4.Text, 4, 1);
            }

            else if(radioButton3.Checked == true)
            {
                fnctionSreach("SearchPrice", textBox3.Text, textBox4.Text, 4, 2);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox5.Text!="" && textBox6.Text!="")
            fnctionSreach("SearchPriceWithFromTo", textBox7.Text, textBox8.Text, 5, 0);
        }

        private async void button6_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            upload_holiday E = new upload_holiday("easyjet");
            E.Show();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(textBox1.Text!="")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox2.Text != "")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }
       

        private void textBox11_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox11.Text != "")
            {
                textBox11.Text = "";
                textBox11.ForeColor = Color.Black;
            }
        }

        private void textBox10_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox10.Text != "")
            {
                textBox10.Text = "";
                textBox10.ForeColor = Color.Black;
            }
        }

        private void textBox3_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (textBox3.Text != "")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox4.Text != "")
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox6_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox6.Text != "")
            {
                textBox6.Text = "";
                textBox6.ForeColor = Color.Black;
            }
        }

        private void textBox7_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox7.Text != "")
            {
                textBox7.Text = "";
                textBox7.ForeColor = Color.Black;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox5.Text != "")
            {
                textBox5.Text = "";
                textBox5.ForeColor = Color.Black;
            }
        }

        private void textBox8_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox8.Text != "")
            {
                textBox8.Text = "";
                textBox8.ForeColor = Color.Black;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int cnt;
            try
            {
                dataGridView2.Columns[18].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridView2.Columns[18].DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView2.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (e.RowIndex > -1)
                {
                    String[] spearator = { "https://" };

                    var val = this.dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string str = val;
                    int index = e.RowIndex;
                    string date = dataGridView2.Rows[index].Cells[3].Value.ToString();
                    string[] tbl = str.Split(spearator, StringSplitOptions.None);
                    cnt = 0;
                    cnt = tbl.Length;

                    if (cnt >= 2)
                    {
                        Process.Start(val);
                    }
                   

                }
            }
            catch { }
        }

        private void button9_Click(object sender, EventArgs e)
        {

           
          

            if (textBox14.Text!="" && textBox15.Text!="")
            {
                if(textBox12.Text!="" && textBox13.Text!="" )
                {
                    if(textBox16.Text!="")
                    {
                        if(textBox17.Text!="")
                        {
                            if(textBox18.Text!="")
                            {
                               
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                        }
                        else
                        {
                            if (textBox18.Text != "")
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                        }
                    }
                    else
                    {
                        if (textBox17.Text != "" )
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                        }
                        else
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, textBox13.Text, 8, 0);
                            }
                        }
                    }

                }
                else if(textBox13.Text != "" && textBox12.Text == ""  )
                {
                    if (textBox16.Text != "" )
                    {
                        if (textBox17.Text != "" )
                        {
                            if (textBox18.Text != "")
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                        }
                        else
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                        }
                    }
                    else
                    {
                        if (textBox17.Text != "" )
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                        }
                        else
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", "", textBox13.Text, 8, 0);
                            }
                        }
                    }
                }
                else if (textBox12.Text != "" && textBox13.Text == "" )
                {
                    if (textBox16.Text != "" )
                    {
                        if (textBox17.Text != "" )
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text,"", 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, "", 8, 0);
                            }
                        }
                        else
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text,"", 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, "", 8, 0);
                            }
                        }
                    }
                    else
                    {
                        if (textBox17.Text != "" )
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, "", 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, "", 8, 0);
                            }
                        }
                        else
                        {
                            if (textBox18.Text != "" )
                            {
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, "", 8, 0);
                            }
                            else
                            {
                                textBox18.Text = "0";
                                fnctionSreach("SearchPriceWithFromToStarTransfers", textBox12.Text, "", 8, 0);
                            }
                        }
                    }
                }
            }
        }

        private void textBox15_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox15.Text != "")
            {
                textBox15.Text = "";
                textBox15.ForeColor = Color.Black;
            }
        }

        private void textBox14_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox14.Text != "")
            {
                textBox14.Text = "";
                textBox14.ForeColor = Color.Black;
            }
        }

        private void textBox13_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox13.Text != "")
            {
                textBox13.Text = "";
                textBox13.ForeColor = Color.Black;
            }
        }

        private void textBox12_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox12.Text != "")
            {
                textBox12.Text = "";
                textBox12.ForeColor = Color.Black;
            }
        }

        private void textBox16_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox16.Text != "")
            {
                textBox16.Text = "";
                textBox16.ForeColor = Color.Black;
            }
        }

        private void textBox17_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox17.Text != "")
            {
                textBox17.Text = "";
                textBox17.ForeColor = Color.Black;
            }
        }

        private void textBox18_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox18.Text != "")
            {
                textBox18.Text = "";
                textBox18.ForeColor = Color.Black;
            }
        }
    }
}
