using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using Z.Dapper.Plus;
using ExcelDataReader;
using System.Threading;


namespace aire
{
    public partial class upload_FG_Airline : Form
    {
        string deleteOldFiles = "deleteOldGFAirline";
        string deleteNewFiles = "deleteNewGFAirline";
        public upload_FG_Airline()
        {
            InitializeComponent();
        }

        string name1, dltname;
        string cbn6;
        DataTable dt;

        ado d = new ado();
        ado d2 = new ado();
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
            d.cmdd = new SqlCommand("delete " + name1 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn3 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn6 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn1 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("delete " + cbn5 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.cmdd.ExecuteNonQuery();
            countRows();
            MessageBox.Show("Finish!!!!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            
            try
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = true;
                label1.Text = label5.Text;
                label5.Text = "";
                label3.Text = label4.Text;
                label4.Text = "";
                string ddlValue = ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString();
                d.cmdd.Parameters.Clear(); // Clear existing parameters
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
                d.cmdd.Connection = d.cn;
                d.cmdd.CommandText = cbn2;
                d.cmdd.CommandTimeout = 600; // 10 minutes for data insertion
                d.cmdd.ExecuteNonQuery();
                countRows();
                button3.Visible = false;
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    MessageBox.Show("Operation timed out after 10 minutes.\n\nPlease try again later.",
                                  "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<classGFAirline> list = new List<classGFAirline>();
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        classGFAirline obj = new classGFAirline();
                        obj.From = dt.Rows[i]["From"].ToString();
                        obj.To = dt.Rows[i]["To"].ToString();
                        obj.Dates = Convert.ToDateTime(dt.Rows[i]["Dates"].ToString());
                        obj.Montant = Convert.ToDouble(dt.Rows[i]["Price"].ToString());
                        obj.Airline = dt.Rows[i]["Airline"].ToString();
                        obj.Aircode = dt.Rows[i]["AIRCODE"].ToString();
                        obj.Cabin = dt.Rows[i]["Cabin"].ToString();
                        obj.Stops = dt.Rows[i]["STOPS"].ToString();
                        obj.Days = dt.Rows[i]["Days"].ToString();
                        obj.web = dt.Rows[i]["URL"].ToString();
                        list.Add(obj);
                    }

                });
                customerBindingSource.DataSource = list;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }
            label2.Visible = true;
            try
            {
                string name = ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString();
                await Task.Run(() =>
                {
                    DapperPlusManager.Entity<classGFAirline>().Table(adrss);
                    List<classGFAirline> holidays = customerBindingSource.DataSource as List<classGFAirline>;
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
                d.cmdd = new SqlCommand("delete " + adrss + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
                d.cmdd.ExecuteNonQuery();
                MessageBox.Show(ex.Message);
            }
        }
        public void FunctionName(string str)
        {
            string ddlValue = ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString();
            switch (adrss)
            {
                case "googleAirlinef1old":
                    {
                        MessageBox.Show("old2");
                        label1.Text = str;

                        d.cmdd.CommandType = CommandType.Text;
                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label1.Text.ToString() + "','','Old','" + ddlValue + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();

                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
                        d.cmdd.Connection = d.cn;
                        d.cmdd.CommandText = "CheapestGAirline";
                        d.cmdd.CommandTimeout = 600; // 10 minutes for data processing
                        d.cmdd.ExecuteNonQuery();

                    }
                    break;


                case "googleAirlineFnew":
                    {
                        MessageBox.Show("new1");
                        label5.Text = str;
                        d.cmdd.CommandType = CommandType.Text;

                        d.cmdd = new SqlCommand("insert into " + name1 + " values('" + label5.Text.ToString() + "','','New','" + ddlValue + "')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
                        d.cmdd.Connection = d.cn;
                        d.cmdd.CommandText = dltname;
                        d.cmdd.CommandTimeout = 600; // 10 minutes for data processing
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd.CommandText = cbn4;
                        d.cmdd.CommandTimeout = 600; // 10 minutes for data processing
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd.CommandText = "delete0and0GFAirline";
                        d.cmdd.CommandTimeout = 600; // 10 minutes for data processing
                        d.cmdd.ExecuteNonQuery();

                    }
                    break;
            }
            if (label1.Text != "" && label5.Text != "")
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                button3.Visible = true;
            }
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
                    
                    string insertQuery = "INSERT INTO GFAirlineDDL (GFAirlineDDLName) VALUES (@Name)";

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
            string table = "GFAirlineDDL";
            string name = "GFAirlineDDLName";

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
                string table = "GFAirlineDDL";
                string name = "GFAirlineDDLName";
                // Get the selected item
                string selectedItemText = ((DataRowView)ddlName.SelectedItem)[name].ToString();

                // Delete the item from the ComboBox
                ddlName.Items.Remove(selectedItemText);

                // Delete the item from the database using the same connection
                using (SqlCommand cmd = new SqlCommand())
                {

                    // old
                    d.connecter(); // Reuse the existing database connection
                    cmd.Connection = d.cn;

                    string deleteQuery = $"DELETE FROM {table} WHERE {name} = @Name";
                    cmd.CommandText = deleteQuery;
                    cmd.Parameters.AddWithValue("@Name", selectedItemText);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    //new
                    //cmd.Parameters.Clear();
                    //d.connecter2(); // Reuse the existing database connection
                    //cmd.Connection = d.cn;

                    //string deleteQuery2 = $"DELETE FROM {table} WHERE {name} = @Name";
                    //cmd.CommandText = deleteQuery2;
                    //cmd.Parameters.AddWithValue("@Name", selectedItemText);

                    //int rowsAffected2 = cmd.ExecuteNonQuery();

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

			int count = 0;
			if (d != null && d.dt != null && d.dt.Rows != null)
			{
				d.dt.Rows.Clear();
			}
			countRows();
			d.da = new SqlDataAdapter("select count(*) from " + name1 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
			d.ds = new DataSet();
			d.da.Fill(d.ds, "countGF");

			count = int.Parse(d.ds.Tables["countGF"].Rows[0][0].ToString());
			//MessageBox.Show(count.ToString());
			if (count == 2)
			{
				nameFileQuick(count);

			}
			else if (count == 1)
			{
				nameFileQuick(count);
			}

			if (label1.Text != "" && label5.Text != "")
			{
				radioButton1.Enabled = false;
				radioButton2.Enabled = false;
				button3.Visible = true;
			}
		}

        private async void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            label8.Text = "Starting transfer...";
            label1.Text = "";

            string connNEWStr1 = "Data Source=SQL8010.site4now.net;Initial Catalog=db_a61545_bobs;User Id=db_a61545_bobs_admin;Password=b0bsfl1gh7;";
            string connOLDStr2 = "Data Source=SQL5096.site4now.net;Initial Catalog=DB_A61545_andycom;User Id=DB_A61545_andycom_admin;Password=goodb0b5;";

            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection connOLD = new SqlConnection(connOLDStr2))
                    using (SqlConnection connNew = new SqlConnection(connNEWStr1))
                    {
                        connOLD.Open();
                        connNew.Open();

                        long totalRows = 0;
                        using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM comprGOOGLAirline", connOLD))
                            totalRows = (int)cmdCount.ExecuteScalar();

                        if (totalRows == 0)
                        {
                            this.Invoke((MethodInvoker)(() => label8.Text = "No rows to transfer."));
                            return;
                        }

                        using (SqlCommand cmdTrunc = new SqlCommand("TRUNCATE TABLE comprGOOGLAirline", connNew))
                        {
                            cmdTrunc.CommandTimeout = 120;
                            cmdTrunc.ExecuteNonQuery();
                        }

                        // Get destination column names so we only map columns that exist in Bob's DB
                        var destColumns = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        using (SqlCommand cmdSchema = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'comprGOOGLAirline'", connNew))
                        using (SqlDataReader schemaReader = cmdSchema.ExecuteReader())
                            while (schemaReader.Read())
                                destColumns.Add(schemaReader.GetString(0));

                        using (SqlCommand cmdSrc = new SqlCommand("SELECT * FROM comprGOOGLAirline", connOLD))
                        {
                            cmdSrc.CommandTimeout = 0;
                            using (SqlDataReader reader = cmdSrc.ExecuteReader())
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connNew))
                            {
                                bulkCopy.DestinationTableName = "comprGOOGLAirline";
                                bulkCopy.BatchSize = 10000;
                                bulkCopy.BulkCopyTimeout = 0;

                                // Only map columns that exist in the destination — skips any new
                                // columns not yet added to Bob's DB (e.g. IsTargetDealOld)
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string col = reader.GetName(i);
                                    if (destColumns.Contains(col))
                                        bulkCopy.ColumnMappings.Add(col, col);
                                }

                                bulkCopy.NotifyAfter = 10000;
                                long transferred = 0;
                                bulkCopy.SqlRowsCopied += (s, ev) =>
                                {
                                    transferred = ev.RowsCopied;
                                    this.Invoke((MethodInvoker)(() =>
                                        label8.Text = "Transferring: " + transferred.ToString("N0") + " / " + totalRows.ToString("N0")));
                                };

                                bulkCopy.WriteToServer(reader);
                            }
                        }
                    }
                });

                label8.Text = "Transfer complete!";
                label1.Text = "Done!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transfer error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label8.Text = "Transfer failed.";
            }
            finally
            {
                button8.Enabled = true;
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

            try
            {
                string Name = ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString();

                d.cmdd.Parameters.Clear(); // Clear existing parameters
                d.cmdd.CommandType = CommandType.StoredProcedure;
                // Add the @Name parameter to the SqlCommand
                d.cmdd.Parameters.AddWithValue("@Name", Name);
                // Set the SqlConnection for the SqlCommand
                d.cmdd.Connection = d.cn; // Assign the existing connection
                // Set the stored procedure name as the command text
                d.cmdd.CommandText = deleteOldFiles;
                d.cmdd.CommandTimeout = 300; // 5 minutes for delete operation
                d.cmdd.ExecuteNonQuery();



                d2.cmdd.CommandText = "delete from comprGOOGLAirline where name='";

                //d.cmdd = new SqlCommand("EXEC " + deleteOldFiles + "", d.cn);
                //d.cmdd.CommandTimeout = 300;
                //d.cmdd.ExecuteNonQuery();

                label1.Text = "";
                label3.Text = "";
                countRows();
                MessageBox.Show("Finish!!!!");
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    MessageBox.Show("Delete operation timed out after 5 minutes.\n\nPlease try again later.",
                                  "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ddlName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select an option from Name Dropdown.");
                return;
            }

            try
            {
                string Name = ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString();

                d.cmdd.Parameters.Clear(); // Clear existing parameters

                d.cmdd.CommandType = CommandType.StoredProcedure;
                // Add the @Name parameter to the SqlCommand
                d.cmdd.Parameters.AddWithValue("@Name", Name);
                // Set the SqlConnection for the SqlCommand
                d.cmdd.Connection = d.cn; // Assign the existing connection
                // Set the stored procedure name as the command text
                d.cmdd.CommandText = deleteNewFiles;
                d.cmdd.CommandTimeout = 300; // 5 minutes for delete operation
                d.cmdd.ExecuteNonQuery();

                label5.Text = "";
                label4.Text = "";
                countRows();
                MessageBox.Show("Finish!!!!");
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    MessageBox.Show("Delete operation timed out after 5 minutes.\n\nPlease try again later.",
                                  "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
			if (ddlName.SelectedIndex == 0)
			{
				MessageBox.Show("Please select an option from Name Dropdown.");
				return;
			}

			// Disable button to prevent multiple clicks
			button4.Enabled = false;
			string originalButtonText = button4.Text;
			button4.Text = "Processing...";
			
			try
			{
				string ddlValue = ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString();
                
				exe1 = "modifAirline";
				exe2 = "deleteOldDateIngoogleAirlinech";
				exe4 = "doblerowschAirline";
				exe6 = "cmprGAirline";
				exe7 = "doblerowsAirline";
       
				if (textBox1.Text != "")
				{
					// Run database operations asynchronously to prevent UI blocking
					await Task.Run(() =>
					{
						try
						{
							// Ensure connection is open
							if (d.cn.State != System.Data.ConnectionState.Open)
							{
								d.connecter();
							}

							// Set 10 minute timeout for bulk data processing operations (600 seconds)
							// These operations process thousands of rows and legitimately take time
							int processingTimeout = 600;

							d.cmdd = new SqlCommand("exec " + exe1 + "", d.cn);
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd = new SqlCommand("exec " + exe4 + "", d.cn);
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd.Parameters.Clear();
							d.cmdd.CommandType = CommandType.StoredProcedure;
							d.cmdd.Parameters.AddWithValue("@Name", ddlValue);
							d.cmdd.Connection = d.cn;

							d.cmdd.CommandText = exe2;
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd.CommandText = exe6;
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd.CommandText = "updatecmprGAirline";
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd.Parameters.Clear();
							d.cmdd.CommandType = CommandType.Text;
							d.cmdd = new SqlCommand("exec " + exe7 + "", d.cn);
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd = new SqlCommand("exec upd_cmprgoogleAirline", d.cn);
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							d.cmdd = new SqlCommand("exec UpdateIsFoundStatusForGFAirline", d.cn);
							d.cmdd.CommandTimeout = processingTimeout;
							d.cmdd.ExecuteNonQuery();

							// Categorise all target colours: Yellow, Purple, Green, Orange
							ClassTargetCategorization.CalculateAllTargetCategories(d.cn, ddlValue);
						}
						catch (SqlException ex)
						{
							this.Invoke((MethodInvoker)delegate
							{
								if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
								{
									MessageBox.Show("Processing timed out after 10 minutes.\n\n" +
												  "The data processing is taking too long. This could be due to:\n" +
												  "1. Very large dataset (5000+ rows)\n" +
												  "2. Slow internet connection\n" +
												  "3. Database server load\n\n" +
												  "Please try again later or contact support if the problem persists.",
												  "Processing Timeout",
												  MessageBoxButtons.OK,
												  MessageBoxIcon.Warning);
								}
								else
								{
									MessageBox.Show("Database error during processing:\n\n" + ex.Message + "\n\n" +
												  "SQL Error Number: " + ex.Number,
												  "Database Error",
												  MessageBoxButtons.OK,
												  MessageBoxIcon.Error);
								}
							});
							throw;
						}
						catch (Exception ex)
						{
							this.Invoke((MethodInvoker)delegate
							{
								MessageBox.Show("Error during database operations: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							});
							throw;
						}
					});


					dt = null;
					d.dt = null;
					
					MessageBox.Show("Finish! Data processing and target categorisation complete.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				// Execute cleanup procedure if it exists (optional)
				try
				{
					if (d.cn.State == System.Data.ConnectionState.Open)
					{
						d.cmdd = new SqlCommand("exec dlltGF0", d.cn);
						d.cmdd.CommandTimeout = 300; // 5 minutes for cleanup
						d.cmdd.ExecuteNonQuery();
					}
				}
				catch (SqlException ex)
				{
					// Stored procedure doesn't exist or timed out - this is OK for cleanup
					System.Diagnostics.Debug.WriteLine("dlltGF0 cleanup error: " + ex.Message);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				// Re-enable button
				button4.Enabled = true;
				button4.Text = "Finish";
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

        private void upload_FG_Airline_Load(object sender, EventArgs e)
        {
			d.dt.Rows.Clear();
			d.connecter();
            d2.connecter2();

			button3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
      

                cbn2 = "insertgoogloldAirline";
                cbn3 = "googleAirlineFnew";
                cbn4 = "CheapestG1Airline";
                cbn5 = "googleAirlinech";
                cbn1 = "comprGOOGLAirline";
                cbn6 = "googleAirlinef1old";
                name1 = "namefilesGFAirline";
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
            d.da = new SqlDataAdapter("select count(*) from " + cbn6 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFOldA");
            label6.Text = "count rows in old data is: " + d.ds.Tables["GFOldA"].Rows[0][0].ToString();

            if (d != null && d.dt != null && d.dt.Rows != null)
			{
				d.dt.Rows.Clear();
			}

            d.da = new SqlDataAdapter("select count(*) from " + cbn3 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFNewA");
            label7.Text = "count rows in new data is: " + d.ds.Tables["GFNewA"].Rows[0][0].ToString();


        }
        private void nameFileQuick(int nbr)
        {
            d.da = new SqlDataAdapter("select * from " + name1 + " where Name = '" + ((DataRowView)ddlName.SelectedItem)["GFAirlineDDLName"].ToString() + "'", d.cn);
            d.ds = new DataSet();
            d.da.Fill(d.ds, "GFAirline");
            if (nbr == 2)
            {
                if(d.ds.Tables["GFAirline"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label3.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                    label5.Text = d.ds.Tables["GFAirline"].Rows[1][1].ToString();
                    //label4.Text = d.ds.Tables["GFAirline"].Rows[1][2].ToString();
                }
                else
                {
                    label1.Text = d.ds.Tables["GFAirline"].Rows[1][1].ToString();
                    //label3.Text = d.ds.Tables["GFAirline"].Rows[1][2].ToString();
                    label5.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label4.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                }
            }
            else if (nbr == 1)
            {
                if(d.ds.Tables["GFAirline"].Rows[0][3].ToString() == "Old")
                {
                    label1.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label3.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                }
                else
                {
                    label5.Text = d.ds.Tables["GFAirline"].Rows[0][1].ToString();
                    //label4.Text = d.ds.Tables["GFAirline"].Rows[0][2].ToString();
                }
            }

        }
    }
}
