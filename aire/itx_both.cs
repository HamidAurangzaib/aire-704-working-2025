using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using ExcelDataReader;
using Z.Dapper.Plus;

namespace aire
{
    public partial class itx_both : Form
    {
        ado d = new ado();
       
      
        string cabin1;
        public itx_both(string cabin)
        {
            InitializeComponent();
            button3.Visible = true;
          
            cabin1 = cabin;
        }
        
        DataTable dt;
        string name1, adrss;
        string cbn1, cbn2, cbn3, cbn4, cbn5, cbn6, dltname;
        private void itx_both_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            if (cabin1 == "normal")
            {

                name1 = "namefilesitx";
                dltname = "DELETnamefilesITX";
                cbn1 = "each_itx_both";
                cbn2 = "insertitx_both_old";
                cbn3 = "itx_both_new";
                cbn4 = "Cheapestitx_both2";
                cbn5 = "itx_bothchea";
                cbn6 = "itx_both_old";

            }
            else if (cabin1 == "all")
            {
                name1 = "namefilesitxallcabin";
                dltname = "DELETnamefilesITXallcabin";
                cbn1 = "each_itx_bothallcabin";
                cbn2 = "insertitx_both_oldallcabin";
                cbn3 = "itx_both_newallcabin";
                cbn4 = "Cheapestitx_both2allcabin";
                cbn5 = "itx_bothcheaallcabin";
                cbn6 = "itx_both_oldallcabin";
            }
           
           



            int count = 0;
            d.dt.Rows.Clear();
            d.connecter();
            countRows();
            d.da = new SqlDataAdapter("select count(*) from " + name1 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "countboth");

            count = int.Parse(d.ds.Tables["countboth"].Rows[0][0].ToString());
           
            if (count == 2)
            {
               nameFileQuick(count);

            }
            else if (count == 1)
            {
                nameFileQuick(count);
            }

            if (label1.Text != "" && label3.Text != "" && label5.Text != "" && label5.Text != "")
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                button3.Visible = true;
            }

        }
        private void nameFileQuick(int nbr)
        {
            d.da = new SqlDataAdapter("select * from " + name1 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "ITX");
            if (nbr == 2)
            {
                if(d.ds.Tables["ITX"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["ITX"].Rows[0][1].ToString();
                    label3.Text = d.ds.Tables["ITX"].Rows[0][2].ToString();
                    label5.Text = d.ds.Tables["ITX"].Rows[1][1].ToString();
                    label4.Text = d.ds.Tables["ITX"].Rows[1][2].ToString();
                }
                else
                {
                    label1.Text = d.ds.Tables["ITX"].Rows[1][1].ToString();
                    label3.Text = d.ds.Tables["ITX"].Rows[1][2].ToString();
                    label5.Text = d.ds.Tables["ITX"].Rows[0][1].ToString();
                    label4.Text = d.ds.Tables["ITX"].Rows[0][2].ToString();
                }
            }
            else if (nbr == 1)
            {
                if(d.ds.Tables["ITX"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["ITX"].Rows[0][1].ToString();
                    label3.Text = d.ds.Tables["ITX"].Rows[0][2].ToString();
                }
                else
                {
                    label5.Text = d.ds.Tables["ITX"].Rows[0][1].ToString();
                    label4.Text = d.ds.Tables["ITX"].Rows[0][2].ToString();
                }
            }

        }
        void countRows()
        {


            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from " + cbn6 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "bothold");
            label6.Text = "count rows in old data is: " + d.ds.Tables["bothold"].Rows[0][0].ToString();


            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from " + cbn3 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "bothnew");
            label7.Text = "count rows in new data is: " + d.ds.Tables["bothnew"].Rows[0][0].ToString();


        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn6;
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn3;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete " + name1 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn3 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn6 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn1 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn5 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = true;
            label1.Text = label5.Text;
            label5.Text = "";
            label3.Text = label4.Text;
            label4.Text = "";
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("EXEC " + cbn2 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            button3.Visible = false;
        }
        DataTableCollection tables;

        private void button6_Click(object sender, EventArgs e)
        {
            if (cabin1 == "normal")
            {
                string deleteNewFiles = "deleteNewItxBoth";

                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("EXEC " + deleteNewFiles + "", d.cn);
                d.cmdd.CommandTimeout = 0;
                d.cmdd.ExecuteNonQuery();

                label5.Text = "";
                label4.Text = "";
                countRows();
                MessageBox.Show("Finish!!!!");

            }
            else if (cabin1 == "all")
            {
                string deleteNewFiles = "deleteNewItxBothAll";

                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("EXEC " + deleteNewFiles + "", d.cn);
                d.cmdd.CommandTimeout = 0;
                d.cmdd.ExecuteNonQuery();

                label5.Text = "";
                label4.Text = "";
                countRows();
                MessageBox.Show("Finish!!!!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (cabin1 == "normal")
            {
                string deleteOldFiles = "deleteOldItxBoth";

                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("EXEC " + deleteOldFiles + "", d.cn);
                d.cmdd.CommandTimeout = 0;
                d.cmdd.ExecuteNonQuery();

                label1.Text = "";
                label3.Text = "";
                countRows();
                MessageBox.Show("Finish!!!!");

            }
            else if (cabin1 == "all")
            {
                string deleteOldFiles = "deleteOldItxBothAll";

                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("EXEC " + deleteOldFiles + "", d.cn);
                d.cmdd.CommandTimeout = 0;
                d.cmdd.ExecuteNonQuery();

                label1.Text = "";
                label3.Text = "";
                countRows();
                MessageBox.Show("Finish!!!!");
            }
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xlsx|Excel Workbook|*.xlsx" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = ofd.FileName;
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            await Task.Run(() =>
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                    {
                                        UseHeaderRow = true
                                    }
                                });
                                tables = result.Tables;

                            });
                            comboBox1.Items.Clear();
                            foreach (DataTable table in tables)
                                comboBox1.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            DataTable dt = tables[comboBox1.SelectedItem.ToString()];
            if (dt != null)
            {
                List<ClassITXAirlinOutput> list = new List<ClassITXAirlinOutput>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ClassITXAirlinOutput obj = new ClassITXAirlinOutput();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Dates = Convert.ToDateTime(dt.Rows[i]["Dates"].ToString());
                        obj.Airline = dt.Rows[i]["Airline"].ToString();
                        obj.Price = Convert.ToDouble(dt.Rows[i]["Price"].ToString());
                        obj.Cabin = dt.Rows[i]["Cabin"].ToString();
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }
        int b = 0;
        private async void button2_Click_1(object sender, EventArgs e)
        {
            label2.Visible = true;
            b = b + 1;
            try
            {
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<ClassITXAirlinOutput>().Table(adrss);
                    List<ClassITXAirlinOutput> holidays = customerBindingSource.DataSource as List<ClassITXAirlinOutput>;
                    using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    {

                        db.BulkInsert(holidays);

                    }
                });
                label2.Visible = false;
                button2.Enabled = false;
                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();

                FunctionName(sqlA, b);
                countRows();
                MessageBox.Show("Finished !");

            }
            catch (Exception ex)
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete " + adrss + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
        }

        public void FunctionName(string str, int nbr)
        {
            if (nbr == 1)
            {
                switch (adrss)
                {
                    case "itx_both_old":
                        {
                            label1.Text = str;
                            
                        }
                        break;
                    case "itx_both_oldallcabin":
                        {
                            label1.Text = str;
                        }
                        break;
                
                    case "itx_both_new":
                        {
                          
                            label5.Text = str;
                        }
                        break;
                    case "itx_both_newallcabin":
                        {
                            label5.Text = str;
                        }
                        break;
                   
                }
            }
            else if (nbr == 2)
            {
                switch (adrss)
                {
                    case "itx_both_old":
                        {
                            
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "','Old')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC Cheapestitx_both", d.cn);
                            d.cmdd.ExecuteNonQuery();

                        }
                        break;
                    case "itx_both_oldallcabin":
                        {
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "','Old')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC Cheapestitx_bothallcabin", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                 
                    case "itx_both_new":
                        {
                            
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "','New')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0itx_bothchea", d.cn);
                            d.cmdd.ExecuteNonQuery();


                        }
                        break;
                    case "itx_both_newallcabin":
                        {
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "','New')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0itx_bothcheaallcabin", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                  
                }
                button3.Visible = true;
                b = 0;

            }
        }
        string exe2, exe4, exe6, exe7;
        private void button4_Click(object sender, EventArgs e)
        {
            if (cabin1 == "normal")
            {

                exe2 = "deleteOldDateInitx_bothchea";
                exe4 = "doblerowsitx_bothchea";

                exe6 = "RunAllProc";
                exe7 = "doblerowseach_itx_botht";

            }
            else if (cabin1 == "all")
            {

                exe2 = "deleteOldDateInitx_bothcheaallcabin";
                exe4 = "doblerowsitx_bothcheaallcabin";

                exe6 = "insert_each_itx_bothallcabin";
                exe7 = "doblerowseach_itx_bothtallcabin";


            }
           
            if (textBox1.Text != "")
            {



                d.cmdd = new SqlCommand("exec " + exe4 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe2 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe6 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe7 + "", d.cn);
                d.cmdd.ExecuteNonQuery();




                dt = null;
                d.dt = null;
                MessageBox.Show("finish");

            }
        }

       
        
       
    }
}
