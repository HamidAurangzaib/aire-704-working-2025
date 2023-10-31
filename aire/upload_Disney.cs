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
    public partial class upload_Disney : Form
    {
        string deleteOldFiles = "deleteDisneyOld";
        string deleteNewFiles = "deleteDisneyNew";
        public upload_Disney()
        {
            InitializeComponent();
        }

        string name1, dltname;
        string cbn6;
        DataTable dt;

        ado d = new ado();
        public object DataSate { get; private set; }
        int bb = 0;
        string adrss;
        string cbn1, cbn2, cbn3, cbn4, cbn5;

        private void button5_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete " + name1 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn3 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn6 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            //d.cmdd = new SqlCommand("delete " + cbn1 + "", d.cn);
            //d.cmdd.ExecuteNonQuery();
            //d.cmdd = new SqlCommand("delete " + cbn5 + "", d.cn);
            //d.cmdd.ExecuteNonQuery();
            label1.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            //countRows();
            label6.Text = "count rows in old data is: 0";
            label7.Text = "count rows in new data is: 0";
            button3.Visible = false;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            MessageBox.Show("Finish!!!!");
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
            d.cmdd = new SqlCommand("EXEC " + cbn2 + "", d.cn);
            d.cmdd.CommandTimeout = 0; //in seconds
            d.cmdd.ExecuteNonQuery();
            countRows();
            button3.Visible = false;
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
                                tables = null;
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
                List<ClassDisney> list = new List<ClassDisney>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ClassDisney obj = new ClassDisney();
                        obj.Code = dt.Rows[i]["Code"].ToString();
                        obj.Date = Convert.ToDateTime(dt.Rows[i]["Date"].ToString());
                        obj.Nights = float.Parse(dt.Rows[i]["Nights"].ToString());
                        obj.Adults = float.Parse(dt.Rows[i]["adults"].ToString());
                        obj.Children = float.Parse(dt.Rows[i]["children"].ToString());
                        obj.Age1 = float.Parse(dt.Rows[i]["age1"].ToString());
                        obj.Age2 = float.Parse(dt.Rows[i]["age2"].ToString());
                        obj.Hotel_dropdown = dt.Rows[i]["Hotel dropdown"].ToString();
                        obj.Hotel_resort = dt.Rows[i]["Hotel resort"].ToString();
                        obj.Offers = dt.Rows[i]["Offers"].ToString();
                        obj.Room_name = dt.Rows[i]["room name"].ToString();
                        obj.Sleeps = dt.Rows[i]["sleeps"].ToString();
                        obj.Room_price = dt.Rows[i]["room price"].ToString();
                        obj.Ticket_name = dt.Rows[i]["ticket name"].ToString();
                        obj.Ticket_price = dt.Rows[i]["ticket price"].ToString() != "" && dt.Rows[i]["ticket price"].ToString() != "#EANF#" ? float.Parse(dt.Rows[i]["ticket price"].ToString()) : 0;
                        obj.Total = dt.Rows[i]["Total"].ToString();
                        obj.Free = dt.Rows[i]["FREE"].ToString();
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            b = b + 1;
            try
            {
                //deleteing existing data before entering new
                if(adrss == cbn6)
                {
                    d.cmdd = new SqlCommand("EXEC " + deleteOldFiles + "", d.cn);
                    d.cmdd.CommandTimeout = 0;
                    d.cmdd.ExecuteNonQuery();
                }
                else if (adrss == cbn3)
                {
                    d.cmdd = new SqlCommand("EXEC " + deleteNewFiles + "", d.cn);
                    d.cmdd.CommandTimeout = 0;
                    d.cmdd.ExecuteNonQuery();
                }
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<ClassDisney>().Table(adrss);
                    List<ClassDisney> holidays = customerBindingSource.DataSource as List<ClassDisney>;
                    //using (IDbConnection db = new SqlConnection("Data Source=DESKTOP-9D4FM2N\\SQLEXPRESS; Database=DB_A61545_andycom;Integrated Security=true;"))
                    using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    {

                        db.BulkInsert(holidays);

                    }
                });

                button2.Enabled = false;
                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();

                FunctionName(sqlA, b);

                MessageBox.Show("Finished !");
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

        int b = 0;
        public void FunctionName(string str, int nbr)
        {
            switch (adrss)
            {
                case "disneyOld":
                    {
                        MessageBox.Show("old");
                        label3.Text = str;
                        d.cmdd.CommandType = CommandType.Text;

                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label3.Text.ToString() + "','" + string.Empty + "','Old')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd = new SqlCommand("EXEC DELETnamefilesDisneyOldAfterInsert", d.cn);
                        d.cmdd.ExecuteNonQuery();

                    }
                    break;


                case "disneyNew":
                    {
                        MessageBox.Show("new");
                        label4.Text = str;
                        d.cmdd.CommandType = CommandType.Text;

                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label4.Text.ToString() + "','" + string.Empty + "','New')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd = new SqlCommand("EXEC DELETnamefilesDisneyNewAfterInsert", d.cn);
                        d.cmdd.ExecuteNonQuery();


                    }
                    break;
            }
            if (label3.Text != "" && label4.Text != "")
            {
                button3.Visible = true;
            }
            b = 0;
            countRows();
        }


        string exe1, exe2, exe4, exe6, exe7;

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;

            d.cmdd = new SqlCommand("EXEC " + deleteOldFiles + "", d.cn);
            d.cmdd.CommandTimeout = 0;
            d.cmdd.ExecuteNonQuery();

            label1.Text = "";
            label3.Text = "";
            label6.Text = "count rows in old data is: 0";
            label7.Text = "count rows in new data is: 0";
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
            label6.Text = "count rows in old data is: 0";
            label7.Text = "count rows in new data is: 0";
            MessageBox.Show("Finish!!!!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label3.Text != "" || label4.Text != "")
            {
                d.cmdd = new SqlCommand("exec finishClick_InsertDisney", d.cn);
                d.cmdd.CommandTimeout = 0; //in seconds
                d.cmdd.ExecuteNonQuery();

                dt = null;
                d.dt = null;
                MessageBox.Show("finish");

            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn3;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn6;
        }

        private void upload_File_Disney(object sender, EventArgs e)
        {
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
      

                cbn2 = "insertDisneyOld";
                cbn3 = "disneyNew";
                cbn4 = "CheapestG1Airline";
                cbn5 = "googleAirlinech";
                cbn1 = "comprGOOGLAirline";
                cbn6 = "disneyOld";
                name1 = "namefilesDisney";
                dltname = "DELETnamefilesGFAirline";
        
            
            int count = 0;
            d.dt.Rows.Clear();
            d.connecter();
            countRows();
            d.da = new SqlDataAdapter("select count(*) from " + name1 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "countGF");

            count = int.Parse(d.ds.Tables["countGF"].Rows[0][0].ToString());
            MessageBox.Show(count.ToString());
            if (count == 2)
            {
                nameFileQuick(count);

            }
            else if (count == 1)
            {
                nameFileQuick(count);
            }

            if (label3.Text != "" && label4.Text != "")
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                button3.Visible = true;
            }
        }
        void countRows()
        {


            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from " + cbn6 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFOldA");
            label6.Text = "count rows in old data is: " + d.ds.Tables["GFOldA"].Rows[0][0].ToString();


            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from " + cbn3 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFNewA");
            label7.Text = "count rows in new data is: " + d.ds.Tables["GFNewA"].Rows[0][0].ToString();


        }
        private void nameFileQuick(int nbr)
        {
            d.da = new SqlDataAdapter("select * from " + name1 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFAirline");
            if (nbr == 2)
            {
                if(d.ds.Tables["GFAirline"].Rows[0][3].ToString() == "Old")
                {
                    label3.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label3.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                    label4.Text = d.ds.Tables["GFAirline"].Rows[1][1].ToString();
                    //label4.Text = d.ds.Tables["GFAirline"].Rows[1][2].ToString();
                }
                else
                {
                    label3.Text = d.ds.Tables["GFAirline"].Rows[1][1].ToString();
                    //label3.Text = d.ds.Tables["GFAirline"].Rows[1][2].ToString();
                    label4.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label4.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                }
            }
            else if (nbr == 1)
            {
                if(d.ds.Tables["GFAirline"].Rows[0][3].ToString() == "Old")
                {
                    label3.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label3.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                }
                else
                {
                    label4.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label4.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                }
            }

        }
    }
}
