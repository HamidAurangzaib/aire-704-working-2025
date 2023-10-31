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
    public partial class addDATA : Form
    {
        OleDbConnection con;
        private readonly SynchronizationContext synchronizationcontext;
        DataTable dt;
        ado d = new ado();
        public addDATA()
        {
            InitializeComponent();
            
            synchronizationcontext = SynchronizationContext.Current;

        }

        private void addDATA_Load(object sender, EventArgs e)
        {
            d.connecter();
            btnAdd.Enabled = false;
            btnUpload.Enabled = false;
            btnFnsh.Enabled = false;
            label2.Visible = false;
            MessageBox.Show("for upload new data, click button Delete for delete old data,thanks");
            int count = 0;
            d.dt.Rows.Clear();
           
            countRows();
            d.da = new SqlDataAdapter("select count(*) from namefilesTAX", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "countITX");

            count = int.Parse(d.ds.Tables["countITX"].Rows[0][0].ToString());

           
            
                nameFileQuick();
           
        }

        void countRows()
        {
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from Tax_csv", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "tax");
            label6.Text = "count rows in old data is: " + d.ds.Tables["tax"].Rows[0][0].ToString();

        }
        private void nameFileQuick()
        {
            d.da = new SqlDataAdapter("select * from namefilesTAX", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "tx");
          
                label1.Text = d.ds.Tables["tx"].Rows[0][1].ToString();
         
        }



       
     

        private void btndelete_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete Tax_csv", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete namefilesTAX", d.cn);
            d.cmdd.ExecuteNonQuery();
            nameFileQuick();
            countRows();
            MessageBox.Show("Finish!!!!");
            btnAdd.Enabled = true;
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
                List<ClassTAX> list = new List<ClassTAX>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ClassTAX obj = new ClassTAX();
                        obj.Code = dt.Rows[i]["Code"].ToString();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.Via = dt.Rows[i]["Via"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Airline = dt.Rows[i]["Airline"].ToString();
                        obj.Cabin = dt.Rows[i]["Cabin"].ToString();
                        obj.Tax1 = Convert.ToDouble(dt.Rows[i]["Tax1"].ToString());
                        obj.Tcode1 = dt.Rows[i]["Tcode1"].ToString();
                        obj.Tax2 = Convert.ToDouble(dt.Rows[i]["Tax2"].ToString());
                        obj.Tcode2 = dt.Rows[i]["Tcode2"].ToString();
                        obj.Tax3 = Convert.ToDouble(dt.Rows[i]["Tax3"].ToString());
                        obj.Tcode3 = dt.Rows[i]["Tcode3"].ToString();
                        obj.Total_tax = Convert.ToDouble(dt.Rows[i]["Total_tax"].ToString());
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            
            try
            {
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<ClassITXAirlinOutput>().Table("Tax_csv");
                    List<ClassITXAirlinOutput> holidays = customerBindingSource.DataSource as List<ClassITXAirlinOutput>;
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
                label1.Text = sqlA;
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into namefilesTAX values('" + label1.Text.ToString() + "')", d.cn);
                d.cmdd.ExecuteNonQuery();
                countRows();
                MessageBox.Show("Finished !");

            }
            catch (Exception ex)
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete Tax_csv", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
            btnFnsh.Enabled = true;
        }

        private void btnFnsh_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
