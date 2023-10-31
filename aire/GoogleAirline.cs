using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace aire
{
    public partial class GoogleAirline : Form
    {
        public GoogleAirline()
        {
            InitializeComponent();
        }
        ado d = new ado();
        private void button9_Click(object sender, EventArgs e)
        {
            upload_FG_Airline GFAirline = new upload_FG_Airline();
            GFAirline.Show();
        }





        public int cnt = 0;
        public void searchfordata(string frm, string to, bool isTargetOnly, string nameProc)
        {
            d.dt.Rows.Clear();

            d.dt.Clear();
            d.dt.Columns.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;

            d.cmdd.CommandText = "" + nameProc + "";

            if (frm != "" && textBox2.Text == "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            }


            else if (frm == "" && to != "")
            {
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            }
            else if (frm != "" && to != "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
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
                    bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                    int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                    if (IsTargetFound.HasValue && IsTargetFound.Value)
                    {
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                    }
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

        string cbnB1, cbnB2, cbnB3;

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = true;
            label4.Visible = true;
            dataGridView1.Rows.Clear();
        }

        string price1, price2, price3, price4, price5, price6;

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



        string pricecabin1, pricecabin2, pricecabin3, pricecabin4;
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
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
      
                dataGridView1.Rows.Clear();
            if(min.Text=="" && max.Text=="")
            {
                dates("serchGGl1Airline", targetsOnlyChkbox.Checked);
            }
            else
            {
                myfunction();
                datePrice("searchDatePricecomprGOOGLAirline", targetsOnlyChkbox.Checked);
            }
            min.Text = "";
            max.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (date1.Value < date2.Value)
            {
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleAirline", textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleAirline", "", textBox2.Text, targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGleAirline", textBox1.Text, "", targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
            }
            else if(date1.Value == date2.Value)
            {
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleAirline", textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date1.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleAirline", "", textBox2.Text, targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date1.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGleAirline", textBox1.Text, "", targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date1.Value.ToString("yyyy/MM/dd")); }
            }
            else if (date1.Value != DateTime.Now && date2.Value==DateTime.Now)
            {
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleAirline", textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date1.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleAirline", "", textBox2.Text, targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date1.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGleAirline", textBox1.Text, "", targetsOnlyChkbox.Checked, date1.Value.ToString("yyyy/MM/dd"), date1.Value.ToString("yyyy/MM/dd")); }
            }
        }

        private void FromToDates(string adrss, string from, string to, bool isTargetOnly, string fromdate, string todate)
        {
            dataGridView1.Rows.Clear();
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = adrss;
            d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = from;
            d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = to;
            d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = fromdate;
            d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = todate;
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;



            if (cnt == 0)
            {
                    MessageBox.Show("The information entered is not on the database!");

            }
            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }
            datagridvColor();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Olde_price = 0 and New_price > 0", "New_price desc");
            for (int i = 0; i < cnt; i++)
            {

                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());
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
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());
            }

            datagridvColor();
            radioButton5.Checked = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(radioDate.Checked)
            {
                if(FromBox.Text!=""&& ToBox.Text!=""&& AirCodeBox.Text!="" && airlineBox.Text!="")
                   search4(1, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

              else if ((FromBox.Text != "" && ToBox.Text != "") && (AirCodeBox.Text != "" && airlineBox.Text == ""))
                   search4(2, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text != "" && ToBox.Text != "") && (AirCodeBox.Text == "" && airlineBox.Text != ""))
                    search4(2, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);


                else if ((FromBox.Text != "" && ToBox.Text == "") && (AirCodeBox.Text != "" && airlineBox.Text == ""))
                    search4(3, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text == "" && ToBox.Text != "") && (AirCodeBox.Text == "" && airlineBox.Text != ""))
                    search4(3, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text != "" && ToBox.Text == "") && (AirCodeBox.Text == "" && airlineBox.Text != ""))
                    search4(3, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text == "" && ToBox.Text != "") && (AirCodeBox.Text != "" && airlineBox.Text == ""))
                    search4(3, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);


                else if ((FromBox.Text != "" && ToBox.Text == "") && (AirCodeBox.Text != "" && airlineBox.Text != ""))
                    search4(4, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text == "" && ToBox.Text != "") && (AirCodeBox.Text != "" && airlineBox.Text != ""))
                    search4(4, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);


                else if (AirCodeBox.Text != "" && airlineBox.Text != "")
                    search4(9,"","", targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if (AirCodeBox.Text == "" && airlineBox.Text != "")
                    search4(10, "","", targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);

                else if (AirCodeBox.Text != "" && airlineBox.Text == "")
                    search4(10, "", "", targetsOnlyChkbox.Checked, FromDate.Value.ToString("yyyy/MM/dd"), ToDate.Value.ToString("yyyy/MM/dd"), AirCodeBox.Text, airlineBox.Text);


            }
            else if(radioNoDate.Checked)
            {
                if (FromBox.Text != "" && ToBox.Text != "" && AirCodeBox.Text != "" && airlineBox.Text != "")
                    search4(5, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

               else if ((FromBox.Text != "" && ToBox.Text == "") && (AirCodeBox.Text != "" && airlineBox.Text == ""))
                    search4(7, FromBox.Text, "", targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text == "" && ToBox.Text != "") && (AirCodeBox.Text == "" && airlineBox.Text != ""))
                    search4(7, "", ToBox.Text, targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text != "" && ToBox.Text == "") && (AirCodeBox.Text == "" && airlineBox.Text != ""))
                    search4(7, FromBox.Text, "", targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text == "" && ToBox.Text != "") && (AirCodeBox.Text != "" && airlineBox.Text == ""))
                    search4(7, "", ToBox.Text, targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);


                else if ((FromBox.Text != "" && ToBox.Text != "") && (AirCodeBox.Text == "" && airlineBox.Text != ""))
                    search4(6, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

                else if ((FromBox.Text != "" && ToBox.Text != "") && (AirCodeBox.Text != "" && airlineBox.Text == ""))
                    search4(6, FromBox.Text, ToBox.Text, targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);


                else if (FromBox.Text != "" && ToBox.Text == "" && (AirCodeBox.Text != "" && airlineBox.Text != ""))
                    search4(8, FromBox.Text, "", targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

                else if (FromBox.Text == "" && ToBox.Text != "" && (AirCodeBox.Text != "" && airlineBox.Text != ""))
                    search4(8,"", ToBox.Text, targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);


                else if (AirCodeBox.Text != "" || airlineBox.Text != "")
                    search4(11, "", "", targetsOnlyChkbox.Checked, "1997-01-01", "1997-01-01", AirCodeBox.Text, airlineBox.Text);

            }
            FromBox.Text = "";
            ToBox.Text = "";
            AirCodeBox.Text = "";
            airlineBox.Text = "";
            FromDate.Value = DateTime.Now;
            ToDate.Value = DateTime.Now;
        }
        private void search4(int nbrV,string frm,string to, bool isTargetOnly, string frmdate, string Tdate,string aircode, string airline)
        {
            dataGridView1.Rows.Clear();

            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "searchFormToAirlineDateGoogleAirlin";
            d.cmdd.Parameters.Add("@nbr", SqlDbType.Int).Value = nbrV;
            d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = frm;
            d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = to;
            d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = frmdate;
            d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = Tdate;
            d.cmdd.Parameters.Add("@aircode", SqlDbType.VarChar,20).Value = aircode;
            d.cmdd.Parameters.Add("@airline", SqlDbType.VarChar,40).Value = airline;
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;

            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }



            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }

            datagridvColor();
        }

        public void dates(string str, bool isTargetOnly)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;

            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }



            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }
            datagridvColor();

        }
        public void datePrice(string str, bool isTargetOnly)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@dateA", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@dateB", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = minP;
            d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = maxP;
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }



            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }
            datagridvColor();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Columns[11].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridView1.Columns[11].DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

                            Hotel h = new Hotel(str, date);
                            h.Show();
                        }
                    }

                }
            }
            catch { }
            datagridvColor();
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Stops = '" + comboBox2.SelectedValue.ToString() + "'", "New_price desc");
            foreach (DataRow dr in ligne)
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), DateTime.Parse(dr[15].ToString()), dr[13].ToString());
            }
            datagridvColor();
            ligne = null;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
                    pricecabin1 = "serchFromTopriceGOOGlebigAirline";
                    pricecabin2 = "serchFromTopriceGOOGleAirline";
                    pricecabin3 = "serchFromTopriceGOOGlebetweenAirline";
                    


            if (textBox1.Text != "" || textBox2.Text != "")
                {
                   
                        if (radioButton1.Checked == true && minPrice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, float.Parse(minPrice.Text), 99999, pricecabin1);
  
                        }
                        else if (radioButton2.Checked == true && minPrice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, float.Parse(minPrice.Text), 99999, pricecabin2);
  
                        }
                        else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                        {

                            pricewithfrom_to(textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, float.Parse(minPrice.Text), float.Parse(maxprice.Text), pricecabin3);

                        }
                  
                }

           

           
            
        }


        public void pricewithfrom_to(string frm, string to, bool isTargetOnly, float price1, float price2, string nameproce)
        {
            dataGridView1.Rows.Clear();
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();

            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproce;
            if ((frm != "" || to != "") && price1 != 99999 && price2 != 99999)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            }
            else if ((frm != "" || to != "") && price1 != 99999 && price2 == 99999)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
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
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }



            datagridvColor();
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

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
           
                price1 = "priceGOOGL1Airline";
                price2 = "googlebigAirline";
                price3 = "googlelesAirline";

           
                if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                {

                    somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), targetsOnlyChkbox.Checked, price1);

                   
                }
                else if (radioButton1.Checked && minPrice.Text != "")
                {
                   

                    somme(float.Parse(minPrice.Text), 99999, targetsOnlyChkbox.Checked, price2);

                   
                }
                else if (radioButton2.Checked && minPrice.Text != "")
                {
                    
                    somme(float.Parse(minPrice.Text), 99999, targetsOnlyChkbox.Checked, price3);

                }
    
        }


        public void somme(float a, float b, bool isTargetOnly, string str)
        {
            dataGridView1.Rows.Clear();

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
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;


                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {
                    bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                    int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                    if (IsTargetFound.HasValue && IsTargetFound.Value)
                    {
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                    }
                }

            datagridvColor();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
            label4.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
            label4.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
           
                cbnB1 = "serchFromToGOOGleAirline";
                cbnB2 = "serchFromGOOGleAirline";
                cbnB3 = "serchToGOOGleAirline";
          
            if (textBox1.Text != "" && textBox2.Text != "")
            {

                searchfordata(textBox1.Text, textBox2.Text, targetsOnlyChkbox.Checked, cbnB1);

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {

                searchfordata(textBox1.Text, "", targetsOnlyChkbox.Checked, cbnB2);
                datagridvColor();
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {

                searchfordata("", textBox2.Text, targetsOnlyChkbox.Checked, cbnB3);
                datagridvColor();

            }

          
        }


        private void comb()
        {
            d.ds.Clear();

            d.da = new SqlDataAdapter("select distinct Stops from comprGOOGLAirline", d.cn);
            d.da.Fill(d.ds, "comST");

            comboBox2.DataSource = d.ds.Tables["comST"];
            comboBox2.DisplayMember = "Stops";
            comboBox2.ValueMember = "Stops";

        }

        private void GoogleAirline_Load(object sender, EventArgs e)
        {
           
            d.connecter();
            label5.Visible = false;
            comb();

            dataGridView2.Visible = false;
            dataGridView1.Visible = true;


            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }

    }
}
