using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using Z.Dapper.Plus;
using ExcelDataReader;

namespace aire
{
    public partial class add_hotel : Form
    {
        OleDbConnection con;
        
        public add_hotel()
        {
            InitializeComponent();
            
        }
        ado d = new ado();
        DataTableCollection tables;

        private void add_hotel_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            label1.Visible = false;
        
            comboBox1.Items.Add("TRIVAGO");
            comboBox1.Items.Add("TRIPADVISOR");
            d.connecter();
        }
        DataTable dt = new DataTable();

        int bb = 0;
        string namehotel, deletenamehtl;
       

     

  

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
                            comboBox2.Items.Clear();
                            foreach (DataTable table in tables)
                                comboBox2.Items.Add(table.TableName);
                        }
                    }
                }
            }
            button1.Enabled = false;
        }

        private async void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tables[comboBox2.SelectedItem.ToString()];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    bool isNameHotel = dt.Rows[i]["Hotel name"].ToString().StartsWith("'");
            //    if (isNameHotel == true)
            //    {
            //        dt.Rows[i]["Hotel name"].ToString().Replace("'", "''");
            //    }
            //    bool isCode = dt.Rows[i]["Code"].ToString().StartsWith("ï»¿\"");
            //    if (isCode == true)
            //    {
            //        dt.Rows[i]["Code"].ToString().Replace("ï»¿\"", "");
            //    }
            //    bool isCode2 = dt.Rows[i]["Code"].ToString().StartsWith("\"");
            //    if (isCode2 == true)
            //    {
            //        dt.Rows[i]["Code"].ToString().Replace("\"", "");
            //    }
            //    bool isCode3 = dt.Rows[i]["Code"].ToString().StartsWith("ï»¿");
                
            //    if (isCode3 == true)
            //      {
            //        dt.Rows[i]["Code"].ToString().Replace("ï»¿", "");
            //      }
            //    bool isPrice = dt.Rows[i]["Price"].ToString().StartsWith("Â£");
            //    if (isPrice == true)
            //    {
            //        dt.Rows[i]["Price"].ToString().Replace("Â£", "");
            //    }
            //    bool isPrice2 = dt.Rows[i]["Price"].ToString().StartsWith("£");
            //    if (isPrice2 == true)
            //    {
            //        dt.Rows[i]["Price"].ToString().Replace("£", "");
            //    }
            //    bool isPrice3 = dt.Rows[i]["Price"].ToString().StartsWith("N/A");
            //    if (isPrice3 == true)
            //    {
            //        dt.Rows[i]["Price"].ToString().Replace("N/A", "0");
            //    }
            //    bool isGuest = dt.Rows[i]["Guest"].ToString().StartsWith("Guest");
            //    if (isGuest == true)
            //    {
            //        dt.Rows[i]["Guest"].ToString().Replace("Guest", "");
            //    }

            //    bool isRating = dt.Rows[i]["Rating"].ToString().StartsWith("/");
            //    if (isRating == true)
            //    {
            //        dt.Rows[i]["Rating"].ToString().Replace("/", "0");
            //    }
            //    bool isReviews = dt.Rows[i]["Reviews"].ToString().StartsWith("N/A");
            //    if (isReviews == true)
            //    {
            //        dt.Rows[i]["Reviews"].ToString().Replace("N/A", "0");
            //    }
            //    bool isHotelInfo = dt.Rows[i]["Hotel info"].ToString().StartsWith("'");
            //    if (isHotelInfo == true)
            //    {
            //        dt.Rows[i]["Hotel info"].ToString().Replace("'", "''");
            //    }
            //    bool isSource = dt.Rows[i]["Source"].ToString().StartsWith("'");
            //    if (isSource == true)
            //    {
            //        dt.Rows[i]["Source"].ToString().Replace("'", "''");
            //    }

            //}


            if (dt != null)
            {
                if (comboBox1.Text == "TRIVAGO")
                {

                    List<ClassHotel> list = new List<ClassHotel>();
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ClassHotel obj = new ClassHotel();
                            obj.code = dt.Rows[i]["Code"].ToString();
                            obj.From = dt.Rows[i]["From"].ToString();
                            obj.Out_Date = Convert.ToDateTime(dt.Rows[i]["Out date"].ToString());
                            obj.In_Date = Convert.ToDateTime(dt.Rows[i]["In date"].ToString());
                            obj.Hotel_name = dt.Rows[i]["Hotel name"].ToString();
                            obj.Price = float.Parse(dt.Rows[i]["Price"].ToString());
                            obj.Guest = float.Parse(dt.Rows[i]["Guest"].ToString());
                            obj.Rating = dt.Rows[i]["Rating"].ToString();
                            obj.Reviews = int.Parse(dt.Rows[i]["Reviews"].ToString());
                            obj.Star = dt.Rows[i]["Star"].ToString();
                            obj.Hotel_Info = dt.Rows[i]["Hotel info"].ToString();
                            obj.Source = dt.Rows[i]["Source"].ToString();
                            obj.URL = dt.Rows[i]["URL"].ToString();
                            obj.Board = "";
                            list.Add(obj);
                        }

                    });

                    customerBindingSource.DataSource = list;
                }
                else if (comboBox1.Text == "TRIPADVISOR")
                {
                    List<ClassHotel> list = new List<ClassHotel>();
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ClassHotel obj = new ClassHotel();
                            obj.code = dt.Rows[i]["Code"].ToString();
                            obj.From = dt.Rows[i]["From"].ToString();
                            obj.In_Date = Convert.ToDateTime(dt.Rows[i]["In date"].ToString());
                            obj.Out_Date = Convert.ToDateTime(dt.Rows[i]["Out date"].ToString());
                            obj.Hotel_name = dt.Rows[i]["Hotel name"].ToString();
                            obj.Price = float.Parse(dt.Rows[i]["Price"].ToString());
                            obj.Rating = dt.Rows[i]["Rating"].ToString();
                            obj.Reviews = int.Parse(dt.Rows[i]["Reviews"].ToString());
                            obj.Hotel_Info = dt.Rows[i]["Hotel info"].ToString();
                            obj.Board = dt.Rows[i]["Board"].ToString();
                            obj.Guest = float.Parse(dt.Rows[i]["Guest"].ToString());
                            obj.URL = dt.Rows[i]["URL"].ToString();
                            obj.Star = "";
                            obj.Source = "";
                            list.Add(obj);
                        }

                    });
                    customerBindingSource.DataSource = list;
                }

            }
            button2.Enabled = true;
        }
        string name1, name2;

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(comboBox1.Text== "TRIVAGO")
            {
                name1 = "nameHotelTRIVAGO";
                name2 = "DELETnameHotelTRIVAGO";
            }
            else if(comboBox1.Text== "TRIPADVISOR")
            {
                name1 = "nameHotelTRIPADVISOR";
                name2 = "DELETnameHotelTRIPADVISOR";
            }
            button1.Enabled = true;
        }

      

        private async void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
                label1.Visible = true;
                if (comboBox1.Text == "TRIVAGO")
                {
                    d.cmdd.CommandType = CommandType.Text;
                    d.cmdd = new SqlCommand("EXEC dltTRIVAGOHotel", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }

                else if (comboBox1.Text == "TRIPADVISOR")
                {
                    d.cmdd.CommandType = CommandType.Text;
                    d.cmdd = new SqlCommand("EXEC dlttripadvisorHotel", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }
                await Task.Run(() =>
                {
                  
                DapperPlusManager.Entity<ClassHotel>().Table("hotel");
                List<ClassHotel> hotels = customerBindingSource.DataSource as List<ClassHotel>;
                    //using (IDbConnection db = new SqlConnection("Data Source=DESKTOP-9D4FM2N\\SQLEXPRESS;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    {

                        db.BulkInsert(hotels);

                }
                });
                button2.Enabled = false;
                label1.Visible = false;

                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();

               
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into " + name1 + " values('" + sqlA.ToString() + "')", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("EXEC " + name2 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                MessageBox.Show("Finished !");
            //}
            //catch (Exception ex)
            //{
            //  MessageBox.Show(ex.Message);
            //}
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
