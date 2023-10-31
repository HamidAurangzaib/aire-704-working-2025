using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using Z.Dapper.Plus;
using ExcelDataReader;
using System.Text;
using System.Globalization;

namespace aire
{
    public partial class upload_Tripadvisor : Form
    {
        string deleteOldFiles = "deleteTripadvisorOld";
        string deleteNewFiles = "deleteTripadvisorNew";
        public upload_Tripadvisor()
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
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            d.cmdd.CommandType = CommandType.Text;
            d.cmdd = new SqlCommand("delete " + name1 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn3 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn6 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete cmprtripadvisor where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            //d.cmdd = new SqlCommand("delete " + cbn1 + "", d.cn);
            //d.cmdd.ExecuteNonQuery();
            //d.cmdd = new SqlCommand("delete " + cbn5 + "", d.cn);
            //d.cmdd.ExecuteNonQuery();
            label1.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            countRows();
            label6.Text = "count rows in old data is: 0";
            label7.Text = "count rows in new data is: 0";
            button3.Visible = false;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            MessageBox.Show("Finish!!!!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            radioButton1.Enabled = false;
            radioButton2.Enabled = true;
            label1.Text = label5.Text;
            label5.Text = "";
            label3.Text = label4.Text;
            label4.Text = "";
            string ddlValue = ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString();
            d.cmdd.Parameters.Clear(); // Clear existing parameters
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
            d.cmdd.Connection = d.cn;
            d.cmdd.CommandText = cbn2;
            d.cmdd.CommandTimeout = 0;
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
                List<ClassTripadvisor> list = new List<ClassTripadvisor>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ClassTripadvisor obj = new ClassTripadvisor();
                        obj.Code = dt.Rows[i]["Code"].ToString();
                        obj.From = dt.Rows[i]["From"].ToString();

                        obj.InDate = dt.Rows[i]["In Date"]?.ToString()?.Trim() != null && dt.Rows[i]["In Date"]?.ToString()?.Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["In Date"]?.ToString()?.Trim()) : DateTime.MinValue;

                        obj.OutDate = dt.Rows[i]["Out Date"]?.ToString()?.Trim() != null && dt.Rows[i]["Out Date"]?.ToString()?.Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["Out Date"]?.ToString()?.Trim()) : DateTime.MinValue;

                        obj.Hotel_name = dt.Rows[i]["HotelName"].ToString();
                        obj.Stars = dt.Rows[i]["stars"].ToString() == "N/A" ? 0 : float.Parse(dt.Rows[i]["stars"].ToString());
                        obj.Ratings = dt.Rows[i]["Rating"].ToString() == "N/A" ? 0 : float.Parse(dt.Rows[i]["Rating"].ToString());
                        obj.Reviews = dt.Rows[i]["Reviews"].ToString() == "N/A" ? 0 : float.Parse(dt.Rows[i]["Reviews"].ToString());
                        obj.Hotel_info = dt.Rows[i]["deal_description"].ToString();
                        obj.Hotel_info_2 = dt.Rows[i]["deal_description 2"].ToString();
                        obj.Board = dt.Rows[i]["Board"].ToString();
                        obj.Board_2 = dt.Rows[i]["Board 2"].ToString();
                        obj.Hotel_img = dt.Rows[i]["HotelIMG"].ToString();
                        obj.Hotel_url = dt.Rows[i]["HotelURL"].ToString();
                        // Extract numeric value from the input string
                        string numericValueString = ExtractNumericValue(dt.Rows[i]["Guest"].ToString());

                        if (int.TryParse(numericValueString, out int guestCount))
                        {
                            // Successfully parsed the numeric value as an integer
                            obj.Guest = guestCount;
                        }
                        else
                        {
                            obj.Guest = 0;
                        }
                        //obj.Guest_info = dt.Rows[i]["Guest Info"].ToString();
                        string price1String = dt.Rows[i]["price"]?.ToString();
                        float price1;

                        if (!string.IsNullOrWhiteSpace(price1String) && float.TryParse(price1String, out price1))
                        {
                            obj.Price_1 = price1;
                        }
                        else
                        {
                            // Parsing failed, handle the error as needed
                            obj.Price_1 = 0; // Set a default value or handle the error in another way
                        }
                        obj.PriceSiteName_1 = dt.Rows[i]["SiteName"].ToString();
                        string price2String = dt.Rows[i][" price 2"]?.ToString();
                        float price2;

                        if (!string.IsNullOrWhiteSpace(price2String) && float.TryParse(price2String, out price2))
                        {
                            obj.Price_2 = price2;
                        }
                        else
                        {
                            // Parsing failed, handle the error as needed
                            obj.Price_2 = 0; // Set a default value or handle the error in another way
                        }
                        obj.PriceSiteName_2 = dt.Rows[i]["SiteName 2"].ToString();
                        obj.Price_Difference = obj.Price_2 == 0 ? 0 : obj.Price_2 - obj.Price_1;
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }
        // Function to extract numeric value from a string
        private string ExtractNumericValue(string input)
        {
            StringBuilder numericValue = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    numericValue.Append(c);
                }
                else
                {
                    // Break loop if a non-digit character is encountered
                    break;
                }
            }

            return numericValue.ToString();
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            label2.Visible = true;
            b = b + 1;
            try
            {
                string name = ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString();
                //deleteing existing data before entering new
                if (adrss == cbn6)
                {
                    d.cmdd.Parameters.Clear(); // Clear existing parameters
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    // Add the @Name parameter to the SqlCommand
                    d.cmdd.Parameters.AddWithValue("@Name", name);
                    // Set the SqlConnection for the SqlCommand
                    d.cmdd.Connection = d.cn; // Assign the existing connection
                                              // Set the stored procedure name as the command text
                    d.cmdd.CommandText = deleteOldFiles;
                    d.cmdd.CommandTimeout = 0;
                    d.cmdd.ExecuteNonQuery();
                }
                else if (adrss == cbn3)
                {
                    d.cmdd.Parameters.Clear(); // Clear existing parameters
                    d.cmdd.CommandType = CommandType.StoredProcedure;
                    // Add the @Name parameter to the SqlCommand
                    d.cmdd.Parameters.AddWithValue("@Name", name);
                    // Set the SqlConnection for the SqlCommand
                    d.cmdd.Connection = d.cn; // Assign the existing connection
                                              // Set the stored procedure name as the command text
                    d.cmdd.CommandText = deleteNewFiles;
                    d.cmdd.CommandTimeout = 0;
                    d.cmdd.ExecuteNonQuery();
                }
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<ClassTripadvisor>().Table(adrss);
                    List<ClassTripadvisor> holidays = customerBindingSource.DataSource as List<ClassTripadvisor>;
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
                });

                button2.Enabled = false;
                string[] a;
                int c;
                a = textBox1.Text.Split('\\');
                c = a.Length - 1;
                string sqlA = a[c].ToString();

                FunctionName(sqlA);

                MessageBox.Show("Finished !");
                label2.Visible = false;
            }
            catch (Exception ex)
            {
                d.cmdd.CommandType = CommandType.Text;
                d.cmdd = new SqlCommand("delete " + adrss + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
        }

        int b = 0;
        public void FunctionName(string str)
        {
            string ddlValue = ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString();
            switch (adrss)
            {
                case "tripadvisorOld":
                    {
                        MessageBox.Show("old");
                        label1.Text = str;
                        d.cmdd.CommandType = CommandType.Text;

                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','','Old','" + ddlValue + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd.Parameters.Clear(); // Clear existing parameters
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
                        d.cmdd.Connection = d.cn;
                        d.cmdd.CommandText = "DELETnamefilesTripadvisorOldAfterInsert";
                        d.cmdd.CommandTimeout = 0;
                        d.cmdd.ExecuteNonQuery();
                    }
                    break;


                case "tripadvisorNew":
                    {
                        MessageBox.Show("new");
                        label5.Text = str;
                        d.cmdd.CommandType = CommandType.Text;

                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','','New','" + ddlValue + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd.Parameters.Clear(); // Clear existing parameters
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
                        d.cmdd.Connection = d.cn;
                        d.cmdd.CommandText = "DELETnamefilesTripadvisorNewAfterInsert";
                        d.cmdd.CommandTimeout = 0;
                        d.cmdd.ExecuteNonQuery();


                    }
                    break;
            }
            if (label1.Text != "" && label5.Text != "")
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

                    string insertQuery = "INSERT INTO TripadvisorDDL (TripadvisorDDLName) VALUES (@Name)";

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
            string table = "TripadvisorDDL";
            string name = "TripadvisorDDLName";

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
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }
            try
            {
                string table = "TripadvisorDDL";
                string name = "TripadvisorDDLName";
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
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                button3.Visible = false;
            }
            //make all file name labels to empty
            label1.Text = "";
            label3.Text = "";
            label5.Text = "";
            label4.Text = "";

            int count;
            if (d != null && d.dt != null && d.dt.Rows != null)
            {
                d.dt.Rows.Clear();
            }
            countRows();
            d.da = new SqlDataAdapter("select * from " + name1 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "GF");
            count = d.ds.Tables["GF"].Rows.Count;
            if (count > 1)
            {
                if (d.ds.Tables["GF"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    //label3.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                    label5.Text = d.ds.Tables["GF"].Rows[1][1].ToString();
                    //label4.Text = d.ds.Tables["GF"].Rows[1][2].ToString();
                }
                else
                {
                    label1.Text = d.ds.Tables["GF"].Rows[1][1].ToString();
                    //label3.Text = d.ds.Tables["GF"].Rows[1][2].ToString();
                    label5.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    //label4.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                }
            }
            else if (count == 1)
            {
                if (d.ds.Tables["GF"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    //label3.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                }
                else
                {
                    label5.Text = d.ds.Tables["GF"].Rows[0][1].ToString();
                    //label4.Text = d.ds.Tables["GF"].Rows[0][2].ToString();
                }
            }

            if (label1.Text != "" && label5.Text != "")
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                button3.Visible = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            string Name = ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString();
            d.cmdd.Parameters.Clear(); // Clear existing parameters
            d.cmdd.CommandType = CommandType.StoredProcedure;
            // Add the @Name parameter to the SqlCommand
            d.cmdd.Parameters.AddWithValue("@Name", Name);
            // Set the SqlConnection for the SqlCommand
            d.cmdd.Connection = d.cn; // Assign the existing connection
            // Set the stored procedure name as the command text
            d.cmdd.CommandText = deleteOldFiles;
            d.cmdd.CommandTimeout = 0;
            d.cmdd.ExecuteNonQuery();

            label1.Text = "";
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
            string Name = ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString();
            d.cmdd.Parameters.Clear(); // Clear existing parameters

            d.cmdd.CommandType = CommandType.StoredProcedure;
            // Add the @Name parameter to the SqlCommand
            d.cmdd.Parameters.AddWithValue("@Name", Name);
            // Set the SqlConnection for the SqlCommand
            d.cmdd.Connection = d.cn; // Assign the existing connection
            // Set the stored procedure name as the command text
            d.cmdd.CommandText = deleteNewFiles;
            d.cmdd.CommandTimeout = 0;
            d.cmdd.ExecuteNonQuery();

            label5.Text = "";
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            string ddlValue = ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString();
            if (label1.Text != "" || label5.Text != "")
            {
                d.cmdd.Parameters.Clear(); // Clear existing parameters
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
                d.cmdd.Connection = d.cn;
                d.cmdd.CommandText = "finishClick_InsertTripadvisor";
                d.cmdd.CommandTimeout = 0;
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

        private void upload_File_Tripadvisor(object sender, EventArgs e)
        {
            d.connecter();

            button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
      

                cbn2 = "insertTripadvisorOld";
                cbn3 = "tripadvisorNew";
                cbn4 = "CheapestG1Airline";
                cbn5 = "googleAirlinech";
                cbn1 = "comprGOOGLAirline";
                cbn6 = "tripadvisorOld";
                name1 = "namefilesTripadvisor";
                dltname = "DELETnamefilesGFAirline";
        
            //load the DDLName
            PopulateDDLName();
        }
        void countRows()
        {
            if (d != null && d.dt != null && d.dt.Rows != null)
            {
                d.dt.Rows.Clear();
            }
            d.da = new SqlDataAdapter("select count(*) from " + cbn6 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFOldA");
            label6.Text = "count rows in old data is: " + d.ds.Tables["GFOldA"].Rows[0][0].ToString();

            d.da = new SqlDataAdapter("select count(*) from " + cbn3 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["TripadvisorDDLName"].ToString() + "'", d.cn);
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
