using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using Z.Dapper.Plus;
using ExcelDataReader;

namespace aire
{
    public partial class sky : Form
    {
        string deleteOldFiles = "deleteOldSkyscannerSkys";
        string deleteNewFiles = "deleteNewSkyscannerSkys";

        string adrss;
        DataTable dt;
        DataTable dt1;
        ado d = new ado();
       
       
        public sky()
        {
            InitializeComponent();
            
        }
        int count = 0;
        private void sky_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            d.connecter();
            countRows();
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select * from namefilesSKYS", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "sky");
            count = d.ds.Tables["sky"].Rows.Count;
            if (count > 1)
            {
                if(d.ds.Tables["sky"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["sky"].Rows[0][1].ToString();
                    label3.Text = d.ds.Tables["sky"].Rows[0][2].ToString();
                    label5.Text = d.ds.Tables["sky"].Rows[1][1].ToString();
                    label4.Text = d.ds.Tables["sky"].Rows[1][2].ToString();
                }
                else
                {
                    label1.Text = d.ds.Tables["sky"].Rows[1][1].ToString();
                    label3.Text = d.ds.Tables["sky"].Rows[1][2].ToString();
                    label5.Text = d.ds.Tables["sky"].Rows[0][1].ToString();
                    label4.Text = d.ds.Tables["sky"].Rows[0][2].ToString();
                }
            }
            else if(count==1)
            {
                if(d.ds.Tables["sky"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["sky"].Rows[0][1].ToString();
                    label3.Text = d.ds.Tables["sky"].Rows[0][2].ToString();
                }
                else
                {
                    label5.Text = d.ds.Tables["sky"].Rows[0][1].ToString();
                    label4.Text = d.ds.Tables["sky"].Rows[0][2].ToString();
                }
            }
            if(label1.Text!=""&& label3.Text != "" && label5.Text != "" && label5.Text != "" )
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                button3.Visible = true;
            }
            
        }
        DataTableCollection tables;
        private async void button1_Click(object sender, EventArgs e)
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
                List<Skayskanner> list = new List<Skayskanner>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Skayskanner obj = new Skayskanner();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Dates = Convert.ToDateTime(dt.Rows[i]["Dates"].ToString());
                        obj.Montant = Convert.ToDouble(dt.Rows[i]["PRICE"].ToString());
                        obj.Cabin = dt.Rows[i]["Cabin"].ToString();
                        obj.Stop = dt.Rows[i]["Stop"].ToString();
                        obj.web = dt.Rows[i]["URL"].ToString();
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }
       

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = "skys2";
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = "skys1";
        }


        int b =0;
        public void FunctionNameSkay(string str,int nbr)
        {
            countRows();
            if(adrss== "skys2" && nbr==1)
            {
                label1.Text = str;
               

            }
            else if(adrss == "skys2" && nbr == 2)
            {
                label3.Text = str;
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into namefilesSKYS values('" + label1.Text.ToString() + "','"+label3.Text.ToString() + "','Old')", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec price2old", d.cn);
                d.cmdd.ExecuteNonQuery();
                b = 0;
                d.cmdd = new SqlCommand("exec CheapestSky1", d.cn);
                d.cmdd.ExecuteNonQuery();

            }
           else if (adrss == "skys1" && nbr == 1)
            {
                label5.Text = str;

            }
            else if (adrss == "skys1" && nbr == 2)
            {
                label4.Text = str;
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into namefilesSKYS values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "','New')", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("EXEC DELETnamefilesSKYS", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec price2new", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec CheapestSky", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("EXEC delete0and0", d.cn);
                d.cmdd.ExecuteNonQuery();


                b = 0;
                button3.Visible = true;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = true;
            label1.Text = label5.Text;
            label5.Text = "";
            label3.Text = label4.Text;
            label4.Text = "";
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("EXEC insertskys2", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;

            d.cmdd = new SqlCommand("EXEC any5", d.cn);
            d.cmdd.ExecuteNonQuery();
           
            d.cmdd = new SqlCommand("EXEC doblerowschsky", d.cn);
            d.cmdd.ExecuteNonQuery();
           
            d.cmdd = new SqlCommand("EXEC cmprS", d.cn);
            d.cmdd.ExecuteNonQuery();
            MessageBox.Show("Finish");
         
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            b = b+1;
            try
            {
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<Skayskanner>().Table(adrss);
                    List<Skayskanner> holidays = customerBindingSource.DataSource as List<Skayskanner>;
                    //using (IDbConnection db = new SqlConnection("Data Source=DESKTOP-9D4FM2N\\SQLEXPRESS; Database=DB_A61545_andycom;Integrated Security=true;"))
                    using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    {

                        db.BulkInsert(holidays);

                    }
                });

                label2.Visible = false;
                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();
               
                FunctionNameSkay(sqlA, b);

                MessageBox.Show("Finished !");
            }
            catch (Exception ex)
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete "+adrss+"", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
           
        }
        void countRows()
        {
            

            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from skys2", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "skyOld");
            label6.Text= "count rows in old data is: " + d.ds.Tables["skyOld"].Rows[0][0].ToString();
           
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from skys1", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "skyNew");
            label7.Text = "count rows in new data is: " + d.ds.Tables["skyNew"].Rows[0][0].ToString();
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete namefilesSKYS", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete skys1", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete skys2", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete cheapskys", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete comprsky", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;

            d.cmdd = new SqlCommand("EXEC " + deleteNewFiles + "", d.cn);
            d.cmdd.CommandTimeout = 0;
            d.cmdd.ExecuteNonQuery();

            label5.Text = "";
            label4.Text = "";
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
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

}

