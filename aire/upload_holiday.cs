using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using Z.Dapper.Plus;
using ExcelDataReader;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace aire
{
    public partial class upload_holiday : Form
    {
        string adrss;
        public upload_holiday(string adrs)
        {
            InitializeComponent();
            adrss = adrs;
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
                            foreach (System.Data.DataTable table in tables)
                                comboBox1.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }
        string name1, name2;
        ado d = new ado();
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlName.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select an option from Name Dropdown.");
                    return;
                }
                else
                {
                    label2.Visible = true;
                    string name = string.Empty;
                    if (adrss == "easyjet")
                    {
                        name = ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString();
                    }
                    else
                    {
                        name = ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString();
                    }
                    await Task.Run(() =>
                    {
                        if (adrss == "easyjet")
                        {
                            DapperPlusManager.Entity<easyjet>().Table(str);
                            List<easyjet> holidays = customerBindingSource.DataSource as List<easyjet>;
                            //using (IDbConnection db = new SqlConnection("Data Source=ALEEHYDER\\SQLEXPRESS; Database=DB_A61545_andycom;Integrated Security=true;"))
                            using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                            {
                                holidays.ForEach(x =>
                                {
                                    x.Name = name;
                                    x.NewUploadDate = DateTime.Now;
                                });

                                db.BulkInsert(holidays);

                            }
                        }
                        else
                        {
                            DapperPlusManager.Entity<JET2HOLIDAYS>().Table(str);
                            List<JET2HOLIDAYS> holidays = customerBindingSource.DataSource as List<JET2HOLIDAYS>;
                            //using (IDbConnection db = new SqlConnection("Data Source=ALEEHYDER\\SQLEXPRESS; Database=DB_A61545_andycom;Integrated Security=true;"))
                            using (IDbConnection db = new SqlConnection("Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;"))
                            {
                                holidays.ForEach(x =>
                                {
                                    x.Name = name;
                                    x.NewUploadDate = DateTime.Now;
                                });

                                db.BulkInsert(holidays);

                            }
                        }

                    });

                    string[] a;
                    int c;
                    a = textBox1.Text.Split('\\');
                    c = a.Length - 1;
                    string sqlA = a[c].ToString();

                    if (str == "easyjetolde")
                    {
                        label1.Text = sqlA;
                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + sqlA.ToString() + "','Old','" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    else if (str == "JET2HOLIDAYSOlde")
                    {
                        label1.Text = sqlA;
                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + sqlA.ToString() + "','Old','" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    else if (str == "easyjet")
                    {
                        label5.Text = sqlA;
                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + sqlA.ToString() + "','New','" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    else if (str == "JET2HOLIDAYS")
                    {
                        label5.Text = sqlA;
                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + sqlA.ToString() + "','New','" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    countRows();
                    d.cmdd.CommandType = CommandType.Text;

                    //d.cmdd = new SqlCommand("EXEC " + name2 + "", d.cn);
                    //d.cmdd.ExecuteNonQuery();
                    if (adrss == "easyjet")
                    {
                        d.cmdd = new SqlCommand("EXEC deleteRepet", d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    else
                    {
                        d.cmdd = new SqlCommand("EXEC deleteRepetJET2", d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    label2.Visible = false;
                    MessageBox.Show("Finished !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataTable dt = tables[comboBox1.SelectedItem.ToString()];
            if (dt != null)
            {
                if (adrss == "easyjet")
                {
                    List<easyjet> list = new List<easyjet>();
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            easyjet obj = new easyjet();
                            obj.From_Airpot = dt.Rows[i]["From Airport"].ToString();
                            obj.From = dt.Rows[i]["From"].ToString();
                            obj.To_Airpot = dt.Rows[i]["To Airport"].ToString();
                            obj.To = dt.Rows[i]["To"].ToString();
                            obj.Arrive = Convert.ToDateTime(dt.Rows[i]["Arrive date"].ToString());
                            obj.Depart = Convert.ToDateTime(dt.Rows[i]["Depart date"].ToString());
                            obj.Place = dt.Rows[i]["Place"].ToString();
                            obj.Nights = dt.Rows[i]["Nights"].ToString();
                            //obj.Total_Price = Convert.ToDouble(dt.Rows[i]["Total"].ToString());
                            if (dt.Rows[i]["Total"] == null || string.IsNullOrEmpty(dt.Rows[i]["Total"].ToString()))
                            {
                                obj.Total_Price = 0; // Set to a default value when the "Total" column is null or empty
                            }
                            else
                            {
                                if (double.TryParse(dt.Rows[i]["Total"].ToString(), out double totalValue))
                                {
                                    if (double.IsNaN(totalValue) || double.IsInfinity(totalValue))
                                    {
                                        obj.Total_Price = 0;
                                    }
                                    else
                                    {
                                        obj.Total_Price = totalValue;
                                    }
                                }
                                else
                                {
                                    obj.Total_Price = 0;
                                }
                            }
                            obj.Hotel_name = dt.Rows[i]["Hotel"].ToString();
                            obj.Board = dt.Rows[i]["Board"].ToString();
                            //obj.Star = Convert.ToInt16(dt.Rows[i]["Star"].ToString());
                            if (dt.Rows[i]["Star"] == null || string.IsNullOrEmpty(dt.Rows[i]["Star"].ToString()))
                            {
                                obj.Star = 0; // Set to a default value when the "Star" column is null or empty
                            }
                            else
                            {
                                if (Int16.TryParse(dt.Rows[i]["Star"].ToString(), out Int16 starValue))
                                {
                                    obj.Star = starValue;
                                }
                                else
                                {
                                    obj.Star = 0;
                                }
                            }
                            //obj.Guest = int.Parse(dt.Rows[i]["Guest"].ToString());
                            if (dt.Rows[i]["Guest"] == null || string.IsNullOrEmpty(dt.Rows[i]["Guest"].ToString()))
                            {
                                obj.Guest = 0; // Set to a default value when the "Guest" column is null or empty
                            }
                            else
                            {
                                if (int.TryParse(dt.Rows[i]["Guest"].ToString(), out int guestValue))
                                {
                                    obj.Guest = guestValue; // Parse and assign the int value
                                }
                                else
                                {
                                    obj.Guest = 0; // Set to a default value if parsing fails
                                }
                            }
                            obj.baggage_Amount = dt.Rows[i]["baggage"].ToString();
                            obj.Hotel_info = dt.Rows[i]["Hotel info"].ToString();
                            obj.Transfers = dt.Rows[i]["Transfer included"].ToString();
                            obj.Image = dt.Rows[i]["Image"].ToString();
                            obj.URL = dt.Rows[i]["URL"].ToString();
                            obj.Extras = dt.Rows[i]["Extras"].ToString();
                            obj.Discount = dt.Rows[i]["Discount"].ToString();
                            list.Add(obj);
                        }

                    });
                    customerBindingSource.DataSource = list;
                }
                if (adrss == "JET2HOLIDAYS")
                {
                    List<JET2HOLIDAYS> list = new List<JET2HOLIDAYS>();
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            JET2HOLIDAYS obj = new JET2HOLIDAYS();
                            obj.From_Airport = dt.Rows[i]["From Airport"].ToString();
                            obj.From = dt.Rows[i]["From"].ToString();
                            obj.To_Airport = dt.Rows[i]["To Airport"].ToString();
                            obj.To = dt.Rows[i]["To"].ToString();
                            if(!string.IsNullOrEmpty(dt.Rows[i]["Arrive date"].ToString()))
                            {
                                try
                                {
                                    obj.Arrive = Convert.ToDateTime(dt.Rows[i]["Arrive date"].ToString());
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["Depart date"].ToString()))
                            {
                                try
                                {
                                    obj.Depart = Convert.ToDateTime(dt.Rows[i]["Depart date"].ToString());
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            obj.Place = dt.Rows[i]["Place"].ToString();
                            obj.Nights = dt.Rows[i]["Nights"].ToString();
                            if (dt.Rows[i]["Total"] == null || string.IsNullOrEmpty(dt.Rows[i]["Total"].ToString()))
                            {
                                obj.Total_Price = 0; // Set to a default value when the "Total" column is null or empty
                            }
                            else
                            {
                                if (double.TryParse(dt.Rows[i]["Total"].ToString(), out double totalValue))
                                {
                                    if (double.IsNaN(totalValue) || double.IsInfinity(totalValue))
                                    {
                                        obj.Total_Price = 0;
                                    }
                                    else
                                    {
                                        obj.Total_Price = totalValue;
                                    }
                                }
                                else
                                {
                                    obj.Total_Price = 0;
                                }
                            }
                            obj.Hotel_name = dt.Rows[i]["Hotel"].ToString();
                            obj.Board = dt.Rows[i]["Board"].ToString();
                            if (dt.Rows[i]["Star"] == null || string.IsNullOrEmpty(dt.Rows[i]["Star"].ToString()))
                            {
                                obj.Star = 0; // Set to a default value when the "Star" column is null or empty
                            }
                            else
                            {
                                if (Int16.TryParse(dt.Rows[i]["Star"].ToString(), out Int16 starValue))
                                {
                                    obj.Star = starValue;
                                }
                                else
                                {
                                    obj.Star = 0;
                                }
                            }
                            if (dt.Rows[i]["Guest"] == null || string.IsNullOrEmpty(dt.Rows[i]["Guest"].ToString()))
                            {
                                obj.Guest = 0; // Set to a default value when the "Guest" column is null or empty
                            }
                            else
                            {
                                if (float.TryParse(dt.Rows[i]["Guest"].ToString(), out float guestValue))
                                {
                                    obj.Guest = guestValue; // Parse and assign the float value
                                }
                                else
                                {
                                    obj.Guest = 0; // Set to a default value if parsing fails
                                }
                            }
                            obj.baggage_Amount = dt.Rows[i]["baggage"].ToString();
                            obj.Hotel_info = dt.Rows[i]["Hotel info"].ToString();
                            obj.Transfers = dt.Rows[i]["Transfer included"].ToString();
                            obj.Image = dt.Rows[i]["Image"].ToString();
                            obj.URL = dt.Rows[i]["URL"].ToString();
                            obj.Extras = dt.Rows[i]["Extras"].ToString();
                            obj.Discount = dt.Rows[i]["Discount"].ToString();
                            list.Add(obj);
                        }

                    });
                    customerBindingSource.DataSource = list;
                }
                button2.Enabled = true;
            }
        }
        string str;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if(adrss == "easyjet")
            {
                str = "easyjetolde";
            }
            else if(adrss == "JET2HOLIDAYS")
            {
                str = "JET2HOLIDAYSOlde";
            }
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (adrss == "easyjet")
            {
                str = "easyjet";
            }
            else if (adrss == "JET2HOLIDAYS")
            {
                str = "JET2HOLIDAYS";
            }
            button1.Enabled = true;
        }
        string insrt;
        private void button3_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            if (adrss == "easyjet")
            {
                string Name = ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString();
                insrt = "InsertOlde";
                d.cmdd.CommandType = CommandType.StoredProcedure;
                // Add the @Name parameter to the SqlCommand
                d.cmdd.Parameters.AddWithValue("@Name", Name);

                // Set the SqlConnection for the SqlCommand
                d.cmdd.Connection = d.cn; // Assign the existing connection
                // Set the stored procedure name as the command text
                d.cmdd.CommandText = insrt;
                d.cmdd.ExecuteNonQuery();
            }
            else if (adrss == "JET2HOLIDAYS")
            {
                string Name = ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString();
                //insrt = "InsertOldeJET2";
                insrt = "InsertOldeJET2WithName";
                d.cmdd.CommandType = CommandType.StoredProcedure;
                // Add the @Name parameter to the SqlCommand
                d.cmdd.Parameters.AddWithValue("@Name", Name);

                // Set the SqlConnection for the SqlCommand
                d.cmdd.Connection = d.cn; // Assign the existing connection
                // Set the stored procedure name as the command text
                d.cmdd.CommandText = insrt;
                d.cmdd.ExecuteNonQuery();
            }
            radioButton1.Enabled = false;
            radioButton2.Enabled = true;
            label1.Text = label5.Text;
            label5.Text = "";
            button1.Enabled = true;
            button3.Visible = false;

            countRows();
        }
        string dlt1, dlt2;
        private void button4_Click(object sender, EventArgs e)
        {
            if (adrss == "easyjet")
            {
                dlt1 = "deleteRepet";
                dlt2 = "deleteRepetolde";

            }
            else if (adrss == "JET2HOLIDAYS")
            {
                dlt1 = "deleteRepetJET2";
                dlt2 = "deleteRepetoldeJET2";
            }
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("EXEC "+ dlt1 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("EXEC "+ dlt2 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            if (adrss == "easyjet")
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete nameEasyjet where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete easyjetolde where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete easyjet where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            else
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete nameJET2HOLIDAYS where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete JET2HOLIDAYSOlde where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete JET2HOLIDAYS where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            label1.Text = "";
            label5.Text = "";
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            if (adrss == "easyjet")
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete nameEasyjet where OldOrNew = 'Old' and Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete easyjetolde where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            else
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete nameJET2HOLIDAYS where OldOrNew = 'Old' and Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete JET2HOLIDAYSOlde where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            label1.Text = "";
            radioButton1.Enabled = true;
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            if (adrss == "easyjet")
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete nameEasyjet where OldOrNew = 'New' and Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete easyjet where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            else
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete nameJET2HOLIDAYS where OldOrNew = 'New' and Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete JET2HOLIDAYS where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
            }
            label5.Text = "";
            radioButton2.Enabled = true;
            countRows();
            MessageBox.Show("Finish!!!!");
        }
        private void btnNameSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please Enter Name");
            }
            else
            {
                try
                {
                    d.connecter(); // Connect to the database

                    string name = txtName.Text;
                    string insertQuery = string.Empty;
                    if (adrss == "easyjet")
                    {
                        insertQuery = "INSERT INTO EasyjetDDL (EasyjetDDLName) VALUES (@Name)";
                    }
                    else
                    {
                        insertQuery = "INSERT INTO Jet2DDL (Jet2DDLName) VALUES (@Name)";
                    }

                    using (SqlCommand cmd = new SqlCommand(insertQuery, d.cn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Name saved successfully.");

                        txtName.Clear();

                        // Refresh the ComboBox
                        PopulateDDLName();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        public void PopulateDDLName()
        {
            string table = string.Empty;
            string name = string.Empty;
            if (adrss == "easyjet")
            {
                table = "EasyjetDDL";
                name = "EasyjetDDLName";
            }
            else
            {
                table = "Jet2DDL";
                name = "Jet2DDLName";
            }
            string query = $"SELECT {name} FROM {table}";
            using (SqlCommand cmd = new SqlCommand(query, d.cn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        // Fill the DataSet with data from the database
                        da.Fill(ds, table);

                        // Create a DataTable for the ComboBox data source
                        System.Data.DataTable comboBoxData = ds.Tables[table];

                        // Add a default item to the DataTable
                        DataRow defaultRow = comboBoxData.NewRow();
                        defaultRow[name] = "Select Name";
                        comboBoxData.Rows.InsertAt(defaultRow, 0);

                        // Set the ComboBox data source to the modified DataTable
                        ddlName.DataSource = comboBoxData;
                        ddlName.DisplayMember = name;

                        // Refresh the ComboBox to display the new data
                        ddlName.Refresh();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the ComboBox
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            try
            {
                string table = string.Empty;
                string name = string.Empty;
                if (adrss == "easyjet")
                {
                    table = "EasyjetDDL";
                    name = "EasyjetDDLName";
                }
                else
                {
                    table = "Jet2DDL";
                    name = "Jet2DDLName";
                }
                // Get the selected item
                string selectedItemText = ((DataRowView)ddlName.SelectedItem)[name].ToString();

                // Delete the item from the ComboBox
                ddlName.Items.Remove(selectedItemText);

                // Delete the item from the database using the same connection
                using (SqlCommand cmd = new SqlCommand())
                {
                    d.connecter(); // Reuse the existing database connection
                    cmd.Connection = d.cn;

                    string deleteQuery = $"DELETE FROM {table} WHERE {name} = @Name";
                    cmd.CommandText = deleteQuery;
                    cmd.Parameters.AddWithValue("@Name", selectedItemText);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item deleted successfully.");
                        //refresh DDL
                        PopulateDDLName();
                    }
                    else
                    {
                        MessageBox.Show("Item not found in the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ddlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string table = string.Empty;
            string name = string.Empty;
            if (adrss == "easyjet")
            {
                name = "EasyjetDDLName";
            }
            else
            {
                name = "Jet2DDLName";
            }
            if (ddlName.SelectedIndex == 0)
            {
                button3.Visible = false;
                button1.Enabled = false;
                button2.Enabled = false;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }
            else
            {
                countRows();
                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select * from " + name1 + " where OldOrNew = 'Old' and Name = '" + ((DataRowView)ddlName.SelectedItem)[name].ToString() + "'", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "hldyOld");
                int count = 0;
                count = d.ds.Tables["hldyOld"].Rows.Count;
                if (count > 0)
                    label1.Text = d.ds.Tables["hldyOld"].Rows[0][1].ToString();
                else
                    label1.Text = "";

                count = 0;
                d.da = new SqlDataAdapter("select * from " + name1 + " where OldOrNew = 'New' and Name = '" + ((DataRowView)ddlName.SelectedItem)[name].ToString() + "'", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "hldyNew");
                count = d.ds.Tables["hldyNew"].Rows.Count;
                if (count > 0)
                    label5.Text = d.ds.Tables["hldyNew"].Rows[0][1].ToString();
                else
                    label5.Text = "";

                if (label1.Text != "" && label5.Text != "")
                {
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    button3.Visible = true;
                }
            }
        }

        void countRows()
        {
            string vrO, vrN;
            if(adrss== "easyjet") { 
                vrO = "easyjetolde"; vrN = "easyjet";

                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select count(*) from " + vrO + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "eold");
                label6.Text = "count rows in old data is: " + d.ds.Tables["eold"].Rows[0][0].ToString();

                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select count(*) from " + vrN + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["EasyjetDDLName"].ToString() + "'", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "easyjetNew");
                label7.Text = "count rows in new data is: " + d.ds.Tables["easyjetNew"].Rows[0][0].ToString();
            }
            else { 
                vrO = "JET2HOLIDAYSOlde"; vrN = "JET2HOLIDAYS";

                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select count(*) from " + vrO + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "eold");
                label6.Text = "count rows in old data is: " + d.ds.Tables["eold"].Rows[0][0].ToString();

                d.dt.Rows.Clear();
                d.da = new SqlDataAdapter("select count(*) from " + vrN + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["Jet2DDLName"].ToString() + "'", d.cn);
                d.ds = new DataSet();
                d.da.Fill(d.ds, "easyjetNew");
                label7.Text = "count rows in new data is: " + d.ds.Tables["easyjetNew"].Rows[0][0].ToString();
            }

        }
        private void upload_holiday_Load(object sender, EventArgs e)
        {
            d.connecter();
            if (adrss== "easyjet")
            {
                name1 = "nameEasyjet";
                name2 = "DeleteNameEasyjet";
            }
            else if(adrss== "JET2HOLIDAYS")
            {
                name1 = "nameJET2HOLIDAYS";
                name2 = "DeleteNameJET2HOLIDAYS";
            }
            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            
            //load the DDLName
            PopulateDDLName();
        }
    }
}
