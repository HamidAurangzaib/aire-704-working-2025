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
    public partial class updatequick : Form
    {

        string deleteOldFiles = "deleteOldQuickSkys";
        string deleteNewFiles = "deleteNewQuickSkys";

        ado d = new ado();
        public updatequick()
        {
            InitializeComponent();
        }
        int count=0;
        void countRows()
        {

        
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from quicksky2", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "quickOld");
            label6.Text = "count rows in old data is: " + d.ds.Tables["quickOld"].Rows[0][0].ToString();
            
          
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from quicksky1", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "quickNew");
            label7.Text = "count rows in new data is: " + d.ds.Tables["quickNew"].Rows[0][0].ToString();
           
           
        }
        private void nameFileQuick(int nbr)
        {
            d.da = new SqlDataAdapter("select * from namefilesquickSKYS", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "sky");
            if (nbr==2)
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
            else if(nbr==1)
            {
                if (d.ds.Tables["sky"].Rows[0][3].ToString() == "Old")
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
           
        }
        private void updatequick_Load(object sender, EventArgs e)
        {
            count = 0;
           
            d.connecter();
            
           
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            countRows();

            d.dt.Rows.Clear();
           

           
            d.da = new SqlDataAdapter("select count(*) from namefilesquickSKYS", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "countnamequicksky");
            count =int.Parse(d.ds.Tables["countnamequicksky"].Rows[0][0].ToString());
            MessageBox.Show(count.ToString());
            if (count ==2)
            {
                nameFileQuick(count);

            }
            else if(count==1)
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
        int bb = 0;
       
       

        
        string adrss;
        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = "quicksky2";
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = "quicksky1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                d.cmdd = new SqlCommand("exec any6", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec doblerowsquickcheapset", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec deleteOldDateInQuickcheapset", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete quicksky", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec cmprtouquick", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec doblerowsquicksky", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show("Finish");

            }
            button4.Enabled = false;
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
            d.cmdd = new SqlCommand("EXEC insertquicksky2", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            button3.Visible = false;
        }
        DataTableCollection tables;
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
                List<quickSkayskanner> list = new List<quickSkayskanner>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        quickSkayskanner obj = new quickSkayskanner();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.City = dt.Rows[i]["To"].ToString();
                        obj.Dates = Convert.ToDateTime(dt.Rows[i]["Dates"].ToString());
                        obj.Stops = dt.Rows[i]["Stops"].ToString();
                        obj.Price = Convert.ToDouble(dt.Rows[i]["Price"].ToString());
                        
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }
        int b = 0;
        public void FunctionNameSkay(string str, int nbr)
        {
            if (adrss == "quicksky2" && nbr == 1)
            {
                label1.Text = str;
                countRows();
            }
            else if (adrss == "quicksky2" && nbr == 2)
            {
                label3.Text = str;
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into namefilesquickSKYS values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "','Old')", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec insertquickcheapset", d.cn);
                d.cmdd.ExecuteNonQuery();
                b = 0;
                countRows();
            }
            else if (adrss == "quicksky1" && nbr == 1)
            {
                label5.Text = str;
                countRows();
            }
            else if (adrss == "quicksky1" && nbr == 2)
            {
                label4.Text = str;
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into namefilesquickSKYS values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "','New')", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("EXEC DELETnamefilesQSKYS", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("EXEC insertquickcheapset1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("EXEC delete0and0quick", d.cn);
                d.cmdd.ExecuteNonQuery();

                b = 0;
                button3.Visible = true;
                countRows();
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            b = b + 1;
            try
            {
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<quickSkayskanner>().Table(adrss);
                    List<quickSkayskanner> holidays = customerBindingSource.DataSource as List<quickSkayskanner>;
                    using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    {

                        db.BulkInsert(holidays);

                    }
                });
               
                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();

                FunctionNameSkay(sqlA, b);

                MessageBox.Show("Finished !");
                button4.Enabled = true;
                label2.Visible = false;
            }
            catch (Exception ex)
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete " + adrss + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete namefilesquickSKYS", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete quicksky1", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete quicksky2", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete quickcheapset", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete quicksky", d.cn);
            d.cmdd.ExecuteNonQuery();
            label1.Text ="";
            label3.Text ="";
            label5.Text = "";
            label4.Text = "";
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
