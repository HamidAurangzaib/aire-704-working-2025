using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using PagedList;


namespace aire
{
    public partial class GOOGLE_SKYS : Form
    {
        ado d = new ado();
        private readonly SynchronizationContext synchronizationcontext;

        public GOOGLE_SKYS()
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public void searchfromto()
        {
            d.dt.Rows.Clear();
            string fromm, to, nameProc = "";
            fromm = textBox1.Text;
            to = textBox2.Text;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                nameProc = "serchFROMTOskyggl";
            }
            else if(textBox1.Text == "" && textBox2.Text != "")
            {
                to = textBox2.Text;
                nameProc = "serchTOskyggl";
            }
            else if(textBox1.Text != "" && textBox2.Text == "")
            {
                nameProc = "serchFROMskyggl";
                fromm = textBox1.Text;
            }
          
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            if (nameProc != "")
            { 
                d.cmdd.CommandText = "" + nameProc + "";
            }
            if (textBox1.Text != "" && textBox2.Text == "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = fromm;
            }
            

          else  if (textBox1.Text == "" && textBox2.Text != "")
            {
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            else if(textBox1.Text != "" && textBox2.Text != "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = fromm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            d.cmdd.Connection = d.cn;
            
            d.dt.Load(d.cmdd.ExecuteReader());
           
            int cnt = d.dt.Rows.Count;
            if(cnt==0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
                for (int i = 0; i < cnt; i++)
                {

                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                      double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()),
                      double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(),
                      double.Parse(d.dt.Rows[i][10].ToString()), double.Parse(d.dt.Rows[i][11].ToString()), double.Parse(d.dt.Rows[i][12].ToString()),
                      double.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString());


            }

        }

        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();
        public async void dtgrdvwcolor()
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
                        else if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Orange;
                        }
                        else if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Gray;
                        }
                        if (Convert.ToDouble(row.Cells[12].Value) < 0)
                        {
                            row.Cells[12].Style.BackColor = Color.LightGreen;
                        }
                        else if (Convert.ToDouble(row.Cells[12].Value) > 0)
                        {
                            row.Cells[12].Style.BackColor = Color.Red;
                        }
                        else if (Convert.ToDouble(row.Cells[12].Value) == 0 && Convert.ToDouble(row.Cells[10].Value) == 0 && Convert.ToDouble(row.Cells[11].Value) > 0)
                        {
                            row.Cells[12].Style.BackColor = Color.Orange;
                        }
                        else if (Convert.ToDouble(row.Cells[12].Value) == 0 && Convert.ToDouble(row.Cells[10].Value) > 0 && Convert.ToDouble(row.Cells[11].Value) == 0)
                        {
                            row.Cells[12].Style.BackColor = Color.Gray;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                
                    searchfromto();


                dtgrdvwcolor();

            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {

                searchfromto();

                dtgrdvwcolor();
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchfromto();
                dtgrdvwcolor();
            }

        }

       

        private async void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "serch1";
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
                      double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(),
                      double.Parse(d.dt.Rows[i][10].ToString()), double.Parse(d.dt.Rows[i][11].ToString()), double.Parse(d.dt.Rows[i][12].ToString()), double.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString());

            }

            dtgrdvwcolor();


        }



        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        DataSet ds1 = new DataSet();
        private void comb()
        {
            d.da = new SqlDataAdapter("select distinct [From] from tablX", d.cn);
            d.da.Fill(d.ds, "com1");
            d.da = new SqlDataAdapter("select distinct [To] from tablX", d.cn);
            d.da.Fill(d.ds, "com2");
            comboBox2.DataSource = d.ds.Tables["com1"];
            comboBox2.DisplayMember = "From";
            comboBox2.ValueMember = "From";

            comboBox3.DataSource = d.ds.Tables["com2"];
            comboBox3.DisplayMember = "To";
            comboBox3.ValueMember = "To";
        }

        private  void GOOGLE_SKYS_Load(object sender, EventArgs e)
        {
            radioButton4.Visible = false;
            radioButton5.Visible = false;
            radioButton6.Visible = false;

            d.connecter();

            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            comb();
            textBox1.Text = "";
            textBox2.Text = "";
            button11.Visible = false;
            button12.Visible = false;

            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];

        }
      

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true && checkBox2.Checked==true)
            {
                radioButton4.Visible = true;
                radioButton5.Visible = true;
                radioButton6.Visible = true;
            }
            else
            {
                radioButton4.Visible = false;
                radioButton5.Visible = false;
                radioButton6.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                radioButton4.Visible = true;
                radioButton5.Visible = true;
                radioButton6.Visible = true;
            }
            else
            {
                radioButton4.Visible = false;
                radioButton5.Visible = false;
                radioButton6.Visible = false;
            }
        }

        public void somme(float a,float b,string nameProc)
        {
            
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameProc;
            if(a!= 99999 && b==99999)
            d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
            else if(a == 99999 && b != 99999)
            d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            else if(a != 99999 && b != 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            }
            d.cmdd.Connection = d.cn;
           
            d.dt.Load(d.cmdd.ExecuteReader());
            
            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(),
                    double.Parse(d.dt.Rows[i][10].ToString()), double.Parse(d.dt.Rows[i][11].ToString()), double.Parse(d.dt.Rows[i][12].ToString()), double.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (checkBox1.Checked==true )/*google*/
            {
                if(checkBox3.Checked==true && checkBox4.Checked == false)/*new*/
                {
                    if(radioButton1.Checked==true && radioButton2.Checked == false && radioButton3.Checked ==false)/*The price is greater than A*/
                    {
                        dataGridView1.Rows.Clear();
                        
                       
                        somme(float.Parse(A.Text), 99999, "tablxgglebig");

                       
                        dtgrdvwcolor();
                    }
                    else if(radioButton2.Checked == true && radioButton3.Checked == false && radioButton1.Checked == false)/*The price is less than A*/
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(A.Text), 99999, "tablxggleless");
                       
                        dtgrdvwcolor();
                    }
                    else if(radioButton3.Checked==true && radioButton2.Checked == false && radioButton1.Checked == false)/*between*/
                    {
                        d.dt.Rows.Clear();
                       
                        somme(float.Parse(A.Text), float.Parse(B.Text), "betweentablxAB");
                        
                        dtgrdvwcolor();
                    }
                }
                else if(checkBox3.Checked == false && checkBox4.Checked == true)/*diffe*/
                {
                    if (radioButton1.Checked == true && radioButton2.Checked == false && radioButton3.Checked == false)/*The price is greater than A*/
                    {
                        dataGridView1.Rows.Clear();
                       
                        
                        
                        somme(float.Parse(A.Text), 99999, "tablxggledifbig");

                        dtgrdvwcolor();
                    }
                    else if (radioButton2.Checked == true && radioButton3.Checked == false && radioButton1.Checked == false)/*The price is less than A*/
                    {
                        dataGridView1.Rows.Clear();
                
                        somme(float.Parse(A.Text), 99999, "tablxggledifless");
                       
                        dtgrdvwcolor();
                    }
                    else if (radioButton3.Checked == true && radioButton2.Checked == false && radioButton1.Checked == false)/*between*/
                    {
                        d.dt.Rows.Clear();
                   
                        somme(float.Parse(A.Text), float.Parse(B.Text), "betweentablxABdif");
                      
                        dtgrdvwcolor();
                    }
                }
            }
            else if(checkBox2.Checked==true)/*skys*/
            {
                if (checkBox4.Checked == false && checkBox3.Checked == true)/*new*/
                {

                    if (radioButton1.Checked == true && radioButton2.Checked == false && radioButton3.Checked == false)/*The price is greater than A*/
                    {
                        dataGridView1.Rows.Clear();
                       
                       
                        somme(float.Parse(A.Text), 99999, "tablxskybig");
                
                        dtgrdvwcolor();
                    }
                    else if (radioButton2.Checked == true && radioButton3.Checked == false && radioButton1.Checked == false)/*The price is less than A*/
                    {
                        dataGridView1.Rows.Clear();
                       
                        somme(float.Parse(A.Text), 99999, "tablxskyless");

                        dtgrdvwcolor();
                    }
                    else if (radioButton3.Checked == true && radioButton2.Checked == false && radioButton1.Checked == false)/*between*/
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(A.Text), float.Parse(B.Text), "betweentablxABnew");
                        
                        dtgrdvwcolor();
                    }

                }
                else if (checkBox3.Checked == false && checkBox4.Checked == true)/*diffe*/
                {
                    if (radioButton1.Checked == true && radioButton2.Checked == false && radioButton3.Checked == false)/*The price is greater than A*/
                    {
                        dataGridView1.Rows.Clear();
                      
                         somme(float.Parse(A.Text), 99999, "tablxskydifbig");

                        dtgrdvwcolor();

                    }
                    else if (radioButton2.Checked == true && radioButton3.Checked == false && radioButton1.Checked == false)/*The price is less than A*/
                    {
                        dataGridView1.Rows.Clear();

                          somme(float.Parse(A.Text), 99999, "tablxskydifless");

                        
                        dtgrdvwcolor();
                    }
                    else if (radioButton3.Checked == true && radioButton2.Checked == false && radioButton1.Checked == false)/*between*/
                    {
                        dataGridView1.Rows.Clear();

                        somme(float.Parse(A.Text), float.Parse(B.Text), "betweentablxABdifsky");

                        dtgrdvwcolor();
                    }
                }
              }
            if(checkBox1.Checked == true && checkBox2.Checked == true)/*google and skys*/
            {
             
                if (checkBox4.Checked == true || checkBox3.Checked == true)/*new*/
                {
                   
                    if (radioButton4.Checked==true && radioButton5.Checked==false && radioButton6.Checked==false && checkBox4.Checked == true)
                    {
                        dataGridView1.Rows.Clear();
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "NewpriceGFNewpriceSkys";
                        d.cmdd.Connection = d.cn;
                      
                        d.dt.Load(d.cmdd.ExecuteReader());
                        
                        int cnt = d.dt.Rows.Count;

                        for (int i = 0; i < cnt; i++)
                        {
                            dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                            double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(),
                            double.Parse(d.dt.Rows[i][10].ToString()), double.Parse(d.dt.Rows[i][11].ToString()), double.Parse(d.dt.Rows[i][12].ToString()), double.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString());
                        }
                        dtgrdvwcolor();
                    }
                    else if(radioButton4.Checked == false && radioButton5.Checked == true && radioButton6.Checked == false && checkBox4.Checked==true)
                    {
                        dataGridView1.Rows.Clear();
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "DifferenceGFDifferenceSkys";
                        d.cmdd.Connection = d.cn;
                        
                        d.dt.Load(d.cmdd.ExecuteReader());
                        
                        int cnt = d.dt.Rows.Count;

                        for (int i = 0; i < cnt; i++)
                        {
                            dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                            double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(),
                            double.Parse(d.dt.Rows[i][10].ToString()), double.Parse(d.dt.Rows[i][11].ToString()), double.Parse(d.dt.Rows[i][12].ToString()), double.Parse(d.dt.Rows[i][13].ToString()), d.dt.Rows[i][14].ToString(), d.dt.Rows[i][15].ToString());
                        }
                        dtgrdvwcolor();
                    }
                    else if (radioButton4.Checked == false && radioButton5.Checked == false && radioButton6.Checked == true && checkBox3.Checked == true)
                    {
                        dataGridView1.Rows.Clear();
                    
                        somme(float.Parse(A.Text),99999,"NewpriceGFSKYSlessthanA");
                        dtgrdvwcolor();
                    }
                }
               
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            lB.Visible = false;
            B.Visible = false;
            A.Visible = true;
            lA.Visible = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            lB.Visible = false;
            B.Visible = false;
            A.Visible = true;
            lA.Visible = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            lB.Visible = true;
            B.Visible = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = false;
            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                radioButton5.Visible = false;
                radioButton4.Visible = true;
                radioButton6.Visible = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = false;
            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                radioButton4.Visible = false;
                radioButton6.Visible = false;
                radioButton5.Visible = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            lA.Visible = false;
            A.Visible = false;
            lB.Visible = false;
            B.Visible = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            lB.Visible = false;
            A.Visible = false;
            B.Visible = false;
            lA.Visible = false;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            lB.Visible = false;
            B.Visible = false;
            lA.Visible = true;
            A.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string str = "1";
            Information_about_files inf = new Information_about_files(str);
            inf.ShowDialog();
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
        private void cabintablX()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "cabintablX";
            d.cmdd.Parameters.Add("@cabin", SqlDbType.VarChar, 20).Value = textcabin.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
            if(cnt==0)
            {
                MessageBox.Show("There is no match between cabin flight and cabin skyscanner or "+textcabin.Text+" is not present in one of the tables ");
            }
            for (int i = 0; i < cnt; i++)
            {

                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                      double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString(),
                      double.Parse(dv[i][10].ToString()), double.Parse(dv[i][11].ToString()), double.Parse(dv[i][12].ToString()), double.Parse(dv[i][13].ToString()), dv[i][14].ToString(), dv[i][15].ToString());


            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            cabintablX();
            dtgrdvwcolor();
        }
        public async void color()
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
                    else if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Orange;
                    }
                    else if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Gray;
                    }
                    if (Convert.ToDouble(row.Cells[12].Value) < 0)
                    {
                        row.Cells[12].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[12].Value) > 0)
                    {
                        row.Cells[12].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToDouble(row.Cells[12].Value) == 0 && Convert.ToDouble(row.Cells[10].Value) == 0 && Convert.ToDouble(row.Cells[11].Value) > 0)
                    {
                        row.Cells[12].Style.BackColor = Color.Orange;
                    }
                    else if (Convert.ToDouble(row.Cells[12].Value) == 0 && Convert.ToDouble(row.Cells[10].Value) > 0 && Convert.ToDouble(row.Cells[11].Value) == 0)
                    {
                        row.Cells[12].Style.BackColor = Color.Gray;
                    }
                }


            });
        }
        int pagenumber = 1;
        IPagedList<tablX> list;
        public async Task<IPagedList<tablX>> GetPagedListAsync(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (tablxEntities12 db = new tablxEntities12())
                {
                    return db.tablXes.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        private async void button10_Click(object sender, EventArgs e)
        {
            button11.Visible = true;
            button12.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
           
                list = await GetPagedListAsync();
                button11.Enabled = list.HasPreviousPage;
                button12.Enabled = list.HasNextPage;
                dataGridView2.DataSource = list.ToList();
                label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            color();
            
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            list = await GetPagedListAsync(++pagenumber);
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            color();
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            list = await GetPagedListAsync(--pagenumber);
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            color();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                int cnt = 0;
                cnt = tbl.Length;

                if (cnt >= 2)
                {
                    Process.Start(val);
                }
            }
            color();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox1.Text = comboBox2.SelectedValue.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox3.SelectedValue.ToString();
        }

        private void citytablx()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "citystblx";
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = textBox4.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            DataView dv = new DataView(d.dt);
            int cnt = dv.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(dv[i][0].ToString(), dv[i][1].ToString(), dv[i][2].ToString(), DateTime.Parse(dv[i][3].ToString()),
                      double.Parse(dv[i][4].ToString()), double.Parse(dv[i][5].ToString()), double.Parse(dv[i][6].ToString()), double.Parse(dv[i][7].ToString()), dv[i][8].ToString(), dv[i][9].ToString(),
                      double.Parse(dv[i][10].ToString()), double.Parse(dv[i][11].ToString()), double.Parse(dv[i][12].ToString()), double.Parse(dv[i][13].ToString()), dv[i][14].ToString(), dv[i][15].ToString());


            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            citytablx();
            dtgrdvwcolor();
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



                    string[] tbl = str.Split(spearator, StringSplitOptions.None);
                    int cnt = 0;
                    cnt = tbl.Length;
                    int index = e.RowIndex;
                    string date = dataGridView1.Rows[index].Cells[3].Value.ToString();

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
            dtgrdvwcolor();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            d.cmdd = new SqlCommand("exec add_G_S", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec upd_g_s", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec upd_cmprgoogle", d.cn);
            d.cmdd.ExecuteNonQuery();
        }
    }
}
