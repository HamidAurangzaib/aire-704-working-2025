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
    public partial class googleF : Form
    {
        string deleteOldFiles = "deleteOldSkyscannerGF";
        string deleteNewFiles = "deleteNewSkyscannerGF";


        DataTable dt;

        
        ado d = new ado();

        public object DataSate { get; private set; }
        int bb = 0;
        string cbn;
        string adrss;
        public googleF(string cabin)
        {
            InitializeComponent();
            button3.Visible = true;
            
            cbn = cabin;
        }
        string cbn1, cbn2, cbn3, cbn4,cbn5;
     
        


        string exe1, exe2, exe3, exe4, exe5, exe6, exe7,exe8;
        int b = 0;
        public void FunctionName(string str, int nbr)
        {
            if(nbr==1)
            {
                switch (adrss)
                {
                    case "googlef1old":
                        {
                            label1.Text = str;
                            MessageBox.Show("old1");
                        }
                        break;
                    case "googlef1oldBusiness":
                        {
                            label1.Text = str;
                        }
                        break;
                    case "googlef1oldPremium":
                        {
                            label1.Text = str;
                        }
                        break;
                    case "googleFnew":
                        {
                            MessageBox.Show("new1");
                            label5.Text = str;
                        }
                        break;
                    case "googleFnewBusiness":
                        {
                            label5.Text = str;
                        }
                        break;
                    case "googleFnewPremium":
                        {
                            label5.Text = str;
                        }
                        break;
                }
            }
            else if(nbr==2)
            {
                switch (adrss)
                {
                    case "googlef1old":
                        {
                            MessageBox.Show("old2");
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "','Old')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CheapestG", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            
                        }
                        break;
                    case "googlef1oldBusiness":
                        {
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CheapestGBusiness", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                    case "googlef1oldPremium":
                        {
                            label3.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC CheapestGPremium", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                    case "googleFnew":
                        {
                            MessageBox.Show("new1");
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "','New')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0GF", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            
                           
                        }
                        break;
                    case "googleFnewBusiness":
                        {
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0GFBusiness", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                    case "googleFnewPremium":
                        {
                            label4.Text = str;
                            d.cmdd.CommandType = CommandType.Text;

                            d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "')", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                            d.cmdd.ExecuteNonQuery();
                            d.cmdd = new SqlCommand("EXEC delete0and0GFPremium", d.cn);
                            d.cmdd.ExecuteNonQuery();
                        }
                        break;
                }
                button3.Visible = true;
                b = 0;
          
            }
        }


        public void FunctionNameSkay(string str, int nbr)
        {
            MessageBox.Show(adrss);
            if (adrss == "googlef1old" || adrss == "googlef1oldBusiness" || adrss == "googlef1oldPremium")
            {
                if(b==1)
                label1.Text = str;

            }
             if (adrss == "googlef1old" || adrss == "googlef1oldBusiness" || adrss == "googlef1oldPremium")
            {
                MessageBox.Show("a");
                if (b == 2)
                {
                 MessageBox.Show("b");
                 label3.Text = str;
                d.cmdd.CommandType = CommandType.Text;

                d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','" + label3.Text.ToString() + "')", d.cn);
                d.cmdd.ExecuteNonQuery();


                b = 0;
                }
            }
             if (adrss == "googleFnew" || adrss == "googleFnewBusiness" || adrss == "googleFnewPremium")
            {
                if(b == 1)
                label5.Text = str;

            }
             if (adrss == "googleFnew" || adrss == "googleFnewBusiness" || adrss == "googleFnewPremium")
            {
                if (b == 2)
                {
                    label4.Text = str;
                    d.cmdd.CommandType = CommandType.Text;

                    d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','" + label4.Text.ToString() + "')", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC " + dltname + "", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC " + cbn4 + "", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    b = 0;
                    button3.Visible = true;
                }
            }
            countRows();
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            DataTable dt = tables[comboBox1.SelectedItem.ToString()];
            if (dt != null)
            {
                List<classGF> list = new List<classGF>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        classGF obj = new classGF();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Dates = Convert.ToDateTime(dt.Rows[i]["Dates"].ToString());
                        obj.Montant = Convert.ToDouble(dt.Rows[i]["Price"].ToString());
                        obj.Cabin = dt.Rows[i]["Cabin"].ToString();
                        obj.Days = dt.Rows[i]["Days"].ToString();
                        obj.Stops = dt.Rows[i]["STOPS"].ToString();
                        obj.web = dt.Rows[i]["URL"].ToString();
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }

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

        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = true;
            label1.Text = label5.Text;
            label5.Text = "";
            label3.Text = label4.Text;
            label4.Text = "";
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("EXEC "+cbn2+"", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            button3.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn6;
        }

        DataTableCollection tables;

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            adrss = cbn3;
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

        private void button5_Click_1(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete "+name1+"", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete "+ cbn3 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete "+ cbn6 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete "+ cbn1 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete "+ cbn5 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            MessageBox.Show("Finish!!!!");
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (cbn == "Business")
            {
                exe1 = "modifBusiness";
                exe2 = "deleteOldDateIngooglechBusiness";         
                exe4 = "doblerowschBusiness";
               
                exe6 = "cmprGBusiness";
                exe7 = "doblerowsBusiness";
                exe8 = "allcomprgoogleforCabinBus";

            }
            else if (cbn == "Premium")
            {
                exe1 = "modifPremium";
                exe2 = "deleteOldDateIngooglechPremium";
                exe4 = "doblerowschPremium";
               
                exe6 = "cmprGPremiumPremium";
                exe7 = "doblerowsPremium";
                exe8 = "allcomprgoogleforCabinBusPre";

            }
            else if (cbn == "Economy")
            {
                exe1 = "modif";
                exe2 = "deleteOldDateIngooglech";     
                exe4 = "doblerowsch";
            
                exe6 = "cmprG";
                exe7 = "doblerows";
                exe8 = "allcomprgoogleforCabinEco";

            }
            if (textBox1.Text != "")
            {
                d.cmdd = new SqlCommand("exec " + exe1 + "", d.cn);
                d.cmdd.ExecuteNonQuery();


                d.cmdd = new SqlCommand("exec " + exe4 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe2 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe6 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe7 + "", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + exe8 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
               
                if (cbn == "Economy")
                {
                    d.cmdd = new SqlCommand("exec upd_cmprgoogle", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }
                dt = null;
                d.dt = null;
                MessageBox.Show("finish");

            }

            // Execute cleanup procedure if it exists (optional)
            try
            {
                d.cmdd = new SqlCommand("exec dlltGF0", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Stored procedure doesn't exist - this is OK for new/local databases
                System.Diagnostics.Debug.WriteLine("dlltGF0 stored procedure not found: " + ex.Message);
            }
          
        }

        private void nameFileQuick(int nbr)
        {
            d.da = new SqlDataAdapter("select * from "+name1+"", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GF");
            if (nbr == 2)
            {
                if(d.ds.Tables["GF"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    label3.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                    label5.Text = d.ds.Tables["GF"].Rows[1][1].ToString();
                    label4.Text = d.ds.Tables["GF"].Rows[1][2].ToString();
                }
                else
                {
                    label1.Text = d.ds.Tables["GF"].Rows[1][1].ToString();
                    label3.Text = d.ds.Tables["GF"].Rows[1][2].ToString();
                    label5.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    label4.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                }
            }
            else if (nbr == 1)
            {
                if(d.ds.Tables["GF"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    label3.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                }
                else
                {
                    label5.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    label4.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                }
            }

        }

        string name1,dltname;
        string cbn6;
        private void googleF_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            if (cbn == "Business")
            {
                
                cbn2 = "insertgoogloldBusiness";
                cbn3 = "googleFnewBusiness";
                cbn4 = "cheapestGBusiness1";
                cbn5 = "googlechBusiness";
                cbn1 = "comprGOOGLBusiness";
                cbn6 = "googlef1oldBusiness";
                name1 = "namefilesGFBusiness";
                dltname = "DELETnamefilesGFBusiness";
                
            }
            else if (cbn == "Premium")
            {
                name1 = "namefilesGFPremium";
                dltname = "DELETnamefilesGFPremium";
                cbn1 = "comprGOOGLPremium";
                cbn2 = "insertgoogloldPremium";
                cbn3 = "googleFnewPremium";
                cbn4 = "cheapestGPremium1";
                cbn5 = "googlechPremium";
                cbn6 = "googlef1oldPremium";
            }
            else if (cbn == "Economy")
            {
                name1 = "namefilesGF";
                dltname = "DELETnamefilesGF";
                cbn1 = "comprGOOGL";
                cbn2 = "insertgooglold";
                cbn3 = "googleFnew";
                cbn4 = "cheapestG1";
                cbn5 = "googlech";
                cbn6 = "googlef1old";

            }

            
           
            int count=0;
            d.dt.Rows.Clear();
            d.connecter();
            countRows();
            d.da = new SqlDataAdapter("select count(*) from "+name1+"", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "countGF");

            count = int.Parse(d.ds.Tables["countGF"].Rows[0][0].ToString());
            MessageBox.Show(count.ToString());
            if (count == 2)
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

        void countRows()
        {
           

            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from "+ cbn6 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFOld");
            label6.Text = "count rows in old data is: " + d.ds.Tables["GFOld"].Rows[0][0].ToString();
           
           
            d.dt.Rows.Clear();
            d.da = new SqlDataAdapter("select count(*) from "+ cbn3 + "", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFNew");
            label7.Text = "count rows in new data is: " + d.ds.Tables["GFNew"].Rows[0][0].ToString();
           
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

      

       

        
        private void button5_Click(object sender, EventArgs e)
        {

            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
