using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using Z.Dapper.Plus;

namespace aire
{
    public partial class output : Form
    {
        OleDbConnection con;
        private readonly SynchronizationContext synchronizationcontext;
        string cabin;
        public output(string ITXcabin)
        {
            InitializeComponent();
            button3.Visible = true;
            synchronizationcontext = SynchronizationContext.Current;
            cabin = ITXcabin;
        }
        ado d = new ado();
        string name1,adrss;
        string cbn1, cbn2, cbn3, cbn4,cbn6,cbn5,dltname;
        private void output_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            if (cabin == "Business")
            {

                name1 = "namefilesoutputB";
                dltname = "DELETnamefilesOTB";
                cbn1 = "each_outputB";
                cbn2 = "insertoutput_oldB";
                cbn3 = "outpu_newB";
                cbn4 = "Cheapestoutput2B";
                cbn5 = "outputcheaB";
                cbn6 = "outpu_oldB";

            }
            else if (cabin == "Premium")
            {
                name1 = "namefilesoutputP";
                dltname = "DELETnamefilesOTP";
                cbn1 = "each_outputP";
                cbn2 = "insertoutput_oldP";
                cbn3 = "outpu_newP";
                cbn4 = "Cheapestoutput2P";
                cbn5 = "outputcheaP";
                cbn6 = "outpu_oldP";
            }
            else if (cabin == "Economy")
            {
                name1 = "namefilesoutput";
                dltname = "DELETnamefilesOT";
                cbn1 = "each_output";
                cbn2 = "insertoutput_old";
                cbn3 = "outpu_new";
                cbn4 = "Cheapestoutput2";
                cbn5 = "outputchea";
                cbn6 = "outpu_old";

            }



            int count = 0;
            d.dt.Rows.Clear();
            d.connecter();
            countRows();
            d.da = new SqlDataAdapter("select count(*) from " + name1 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "countITX");

            count = int.Parse(d.ds.Tables["countITX"].Rows[0][0].ToString());
            
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
        void countRows()
        {


            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from " + cbn6 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFOld");
            label6.Text = "count rows in old data is: " + d.ds.Tables["GFOld"].Rows[0][0].ToString();


            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from " + cbn3 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFNew");
            label7.Text = "count rows in new data is: " + d.ds.Tables["GFNew"].Rows[0][0].ToString();


        }
        private void nameFileQuick(int nbr)
        {
            d.da = new SqlDataAdapter("select * from " + name1 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "ITX");
            if (nbr == 2)
            {
                label1.Text = d.ds.Tables["ITX"].Rows[0][1].ToString();
                label3.Text = d.ds.Tables["ITX"].Rows[0][2].ToString();
                label5.Text = d.ds.Tables["ITX"].Rows[1][1].ToString();
                label4.Text = d.ds.Tables["ITX"].Rows[1][2].ToString();
            }
            else if (nbr == 1)
            {
                label1.Text = d.ds.Tables["ITX"].Rows[0][1].ToString();
                label3.Text = d.ds.Tables["ITX"].Rows[0][2].ToString();

            }

        }
        public void FunctionName(string str, int nbr)
        {
            if (nbr == 1)
            {
                switch (adrss)
                {
                    case "outpu_old":
                        {
                            label1.Text = str;
                            MessageBox.Show("old1");
                        }
                        break;
                    case "outpu_oldB":
                        {
                            label1.Text = str;
                        }
                        break;
                    case "outpu_oldP":
                        {
                            label1.Text = str;
                        }
                        break;
                    case "outpu_new":
                        {
                            MessageBox.Show("new1");
                            label5.Text = str;
                        }
                        break;
                    case "outpu_newB":
                        {
                            label5.Text = str;
                        }
                        break;
                    case "outpu_newP":
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
                    case "outpu_old":
                        {
                            MessageBox.Show("old2");
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC Cheapestoutput", d.cn);
                            d.cmdd.ExecuteNonQuery();

                        }
                        break;
                    case "outpu_oldB":
                        {
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CheapestoutputB", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                    case "outpu_oldP":
                        {
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CheapestoutputP", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                    case "outpu_new":
                        {
                            MessageBox.Show("new1");
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0output", d.cn);
                            d.cmdd.ExecuteNonQuery();


                        }
                        break;
                    case "outpu_newB":
                        {
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0outputB", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                    case "outpu_newP":
                        {
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0outputP", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                }
                button3.Visible = true;
                b = 0;

            }
        }

      
        DataTable dt ;


       

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn3;
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

     

        private void button5_Click(object sender, EventArgs e)
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void customerBindingSource_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            DataTable dt = tables[comboBox1.SelectedItem.ToString()];
            if (dt != null)
            {
                List<ClassCalendarOutput> list = new List<ClassCalendarOutput>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ClassCalendarOutput obj = new ClassCalendarOutput();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Dates = Convert.ToDateTime(dt.Rows[i]["Dates"].ToString());
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
                    DapperPlusManager.Entity<classGF>().Table(adrss);
                    List<classGF> holidays = customerBindingSource.DataSource as List<classGF>;
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
        string  exe2, exe4, exe6, exe7;
        private void button4_Click(object sender, EventArgs e)
        {
            if (cabin == "Business")
            {

                exe2 = "deleteOldDateInoutputcheaB";
                exe4 = "doblerowsoutputcheaB";

                exe6 = "insert_each_outputB";
                exe7 = "doblerowseach_outputB";

            }
            else if (cabin == "Premium")
            {

                exe2 = "deleteOldDateInoutputcheaP";
                exe4 = "doblerowsoutputcheaP";

                exe6 = "insert_each_outputP";
                exe7 = "doblerowseach_outputP";


            }
            else if (cabin == "Economy")
            {
               
                exe2 = "deleteOldDateInoutputcheaP";
                exe4 = "doblerowsoutputcheaP";

                exe6 = "insert_each_outputP";
                exe7 = "doblerowseach_outputP";
                

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

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn6;
        }

        

        
        
      
      
    }
}
