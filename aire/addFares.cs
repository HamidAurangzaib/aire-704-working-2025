using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using ExcelDataReader;
using System.IO;
using Z.Dapper.Plus;

namespace aire
{
    public partial class addFares : Form
    {
     
        DataTable dt;

        ado d = new ado();
        public addFares()
        {
            InitializeComponent();
           

        }

      

        private void addFares_Load(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnUpload.Enabled = false;
            btnFnsh.Enabled = false;
            label2.Visible = false;
            button3.Visible = false;
            d.connecter();
            d.dt.Rows.Clear();
            int count;
            d.da = new SqlDataAdapter("select * from namefilesFrs", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "FRS");
            d.dt = d.ds.Tables["FRS"];
            count = d.dt.Rows.Count;
            if (count > 0)
            {
                try
                {
                    label1.Text = d.dt.Rows[0][1].ToString();
                    label3.Text = d.dt.Rows[1][1].ToString();
                }
                catch
                {

                }
            }
            countRows();
        }
        void countRows()
        {
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from FaresOld", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "FOld");
            label6.Text = "count rows in old data is: " + d.ds.Tables["FOld"].Rows[0][0].ToString();
            
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from FaresNew", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "FNew");
            label4.Text = "count rows in new data is: " + d.ds.Tables["FNew"].Rows[0][0].ToString();
            if (double.Parse(d.ds.Tables["FNew"].Rows[0][0].ToString()) > 0)
            { button3.Visible = true; }

        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete FaresNew", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete FaresOld", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete namefilesFrs", d.cn);
            d.cmdd.ExecuteNonQuery();
            nameFileQuick();
            countRows();
            MessageBox.Show("Finish!!!!");
           
        }
        private void nameFileQuick()
        {
            d.da = new SqlDataAdapter("select * from namefilesFrs", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "tx");

            label1.Text = d.ds.Tables["tx"].Rows[0][1].ToString();
            label3.Text = d.ds.Tables["tx"].Rows[1][1].ToString();
        }
        DataTableCollection tables;
        private async void btnAdd_Click(object sender, EventArgs e)
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
            btnUpload.Enabled = true;
            DataTable dt = tables[comboBox1.SelectedItem.ToString()];
            if (dt != null)
            {
                List<ClassFares> list = new List<ClassFares>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ClassFares obj = new ClassFares();
                        obj.Code = dt.Rows[i]["Code"].ToString();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Airline = dt.Rows[i]["Airline"].ToString();
                        obj.Dates = dt.Rows[i]["Dates"].ToString();
                        obj.Cabin = dt.Rows[i]["Cabin"].ToString();
                        obj.Lineno = dt.Rows[i]["Lineno"].ToString();
                        obj.Farebasis = dt.Rows[i]["Farebasis"].ToString();
                        obj.Price = Convert.ToDouble(dt.Rows[i]["GBP"].ToString());
                        obj.Class = dt.Rows[i]["Class"].ToString();
                        obj.Season = dt.Rows[i]["Season"].ToString();
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }
        public void FunctionName(string str)
        {
           
          
                switch (adrs)
                {
                    case "FaresOld":
                        {
                           
                            label1.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into namefilesFrs values('" + label1.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC UpdateAirlin3lttrTo2lttrFaresOld", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CHFOld", d.cn);
                            d.cmdd.ExecuteNonQuery();

                        }
                        break;
                   
                    case "FaresNew":
                        {
                           
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;
                            d.cmdd = new SqlCommand("insert into namefilesFrs values('" + label3.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC UpdateAirlin3lttrTo2lttrFaresNew", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CHF", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            button3.Visible = false;
                        }
                        break;
                   
                   
                }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            label2.Visible = true;

            try
            {
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<ClassFares>().Table(adrs);
                    List<ClassFares> holidays = customerBindingSource.DataSource as List<ClassFares>;
                    using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                    {

                        db.BulkInsert(holidays);

                    }
                });
                label2.Visible = false;
                btnUpload.Enabled = false;
                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();

                FunctionName(sqlA);
                countRows();
                MessageBox.Show("Finished !");

            }
            catch (Exception ex)
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete "+adrs+"", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
            btnFnsh.Enabled = true;
        }

        private void btnFnsh_Click(object sender, EventArgs e)
        {
            
            d.cmdd = new SqlCommand("exec tbl4", d.cn);
            d.cmdd.ExecuteNonQuery();
           
            d.cmdd = new SqlCommand("delete from tx", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec curxxx", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec deletetx", d.cn);
            d.cmdd.ExecuteNonQuery();
          
            d.cmdd = new SqlCommand("exec doblerow_t", d.cn);
            d.cmdd.ExecuteNonQuery();
            MessageBox.Show("Finish!!!");
            this.Close();
        }
        string adrs;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            adrs = "FaresOld";
            btnAdd.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            adrs = "FaresNew";
            btnAdd.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = label3.Text;
            label3.Text = "";
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("EXEC namefilesFrs", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            button3.Visible = false;

          
        }
    }
}
