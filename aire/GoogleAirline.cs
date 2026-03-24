using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace aire
{
    public partial class GoogleAirline : Form
    {
        public GoogleAirline()
        {
            InitializeComponent();
        }
        ado d = new ado();
        private void button9_Click(object sender, EventArgs e)
        {
            upload_FG_Airline GFAirline = new upload_FG_Airline();
            GFAirline.Show();
        }

        public async Task searchformultigroupcitydata(string frm, string to, bool isTargetOnly, string nameProc)
        {
            d.dt.Rows.Clear();

            d.dt.Clear();
            d.dt.Columns.Clear();
            d.cmdd.Parameters.Clear();
            
            try
            {
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "" + nameProc + "";

            //Get selected value of stops ddl
            string selectedStops = ddlStops.SelectedValue.ToString();
            selectedStops = selectedStops.Trim() == "Please Select" ? "" : selectedStops ;
            //Get selected value of stops ddl
            string selectedDays = ddlDays.SelectedValue.ToString();
            selectedDays = selectedDays.Trim() == "Please Select" ? "" : selectedDays;
            //Get selected value of cabin ddl
            string selectedCabin = ddlCabin.SelectedValue.ToString();
            selectedCabin = selectedCabin.Trim() == "Please Select" ? "" : selectedCabin;

            if (!string.IsNullOrEmpty(selectedDays))
            {
                // Remove non-numeric characters and keep only the first numeric value
                string numericValue = new string(selectedDays.Where(char.IsDigit).ToArray());

                // Update selectedDays with the numeric value
                selectedDays = string.IsNullOrEmpty(numericValue) ? selectedDays : numericValue;
            }

            // Access the selected value when a button or some other control is clicked
            //ComboBoxItem selectedDayValue = (ComboBoxItem)ddlDays.SelectedItem;
            //var selectedDays = (string)selectedDayValue.Value;

            string dateFromVar = "1997-01-01";
            string dateToVar = "1997-01-01";
            if (radioBtnDate.Checked )
            {
                dateFromVar = dateFrom.Value.ToString();
                dateToVar = dateTo.Value.ToString();
            }

            if (radioBetween.Checked)
            {
                if (string.IsNullOrEmpty(txtMinPrice.Text) || string.IsNullOrEmpty(txtMaxPrice.Text))
                {
                    radioBetween.Checked = false;
                }
                else
                {
                    radioBetween.Checked = true;
                }
            }
            if (radioGreater.Checked)
            {
                txtMaxPrice.Text = "";
                if (string.IsNullOrEmpty(txtMinPrice.Text))
                {
                    radioGreater.Checked = false;
                }
                else
                {
                    radioGreater.Checked = true;
                }
            }
            if (radioLess.Checked)
            {
                txtMaxPrice.Text = "";
                if (string.IsNullOrEmpty(txtMinPrice.Text))
                {
                    radioLess.Checked = false;
                }
                else
                {
                    radioLess.Checked = true;
                }
            }

            bool greenDiff = checkGreenDiff.Checked;
            bool redDiff = checkRedDiff.Checked;

            var varRadioBetween = radioBetween.Checked;
            var varRadioGreater = radioGreater.Checked;
            var varRadioLess = radioLess.Checked;
            float varMinPrice = txtMinPrice.Text == "" || txtMinPrice.Text == null ? 0 : float.Parse(txtMinPrice.Text);
            float varMaxPrice = txtMaxPrice.Text == "" || txtMaxPrice.Text == null ? 0 : float.Parse(txtMaxPrice.Text);

            bool shortStays = chkShortStays.Checked;

            bool everywhereFrom = false;
            bool everywhereTo = false;

            bool isFromFirstEverywhere = frm.Split(',').Select(e => e.Trim()).FirstOrDefault().Equals("Everywhere", StringComparison.OrdinalIgnoreCase);
            if (isFromFirstEverywhere)
            {
                everywhereFrom = frm.ToLower() == "everywhere" ? false : true;
                frm = string.Join(", ", frm.Split(',').Select(e => e.Trim()).Where(e => !e.Equals("Everywhere", StringComparison.OrdinalIgnoreCase)));
            }
            bool isToFirstEverywhere = to.Split(',').Select(e => e.Trim()).FirstOrDefault().Equals("Everywhere", StringComparison.OrdinalIgnoreCase);
            if (isToFirstEverywhere)
            {
                everywhereTo = to.ToLower() == "everywhere" ? false : true;
                to = string.Join(", ", to.Split(',').Select(e => e.Trim()).Where(e => !e.Equals("Everywhere", StringComparison.OrdinalIgnoreCase)));
            }

            if (frm != "" && to == "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 200).Value = frm;
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 200).Value = txtAirline.Text;
                d.cmdd.Parameters.Add("@Aircode", SqlDbType.VarChar, 50).Value = txtAircode.Text;
                d.cmdd.Parameters.Add("@Days", SqlDbType.VarChar, 10).Value = selectedDays;
                d.cmdd.Parameters.Add("@Cabin", SqlDbType.VarChar, 100).Value = selectedCabin;
                d.cmdd.Parameters.Add("@Shortstays", SqlDbType.Bit).Value = shortStays;
                d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = dateFromVar;
                d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = dateToVar;
                d.cmdd.Parameters.Add("@IsBetween", SqlDbType.Bit).Value = varRadioBetween;
                d.cmdd.Parameters.Add("@IsGreater", SqlDbType.Bit).Value = varRadioGreater;
                d.cmdd.Parameters.Add("@IsLess", SqlDbType.Bit).Value = varRadioLess;
                d.cmdd.Parameters.Add("@MinPrice", SqlDbType.Float).Value = varMinPrice;
                d.cmdd.Parameters.Add("@MaxPrice", SqlDbType.Float).Value = varMaxPrice;
                d.cmdd.Parameters.Add("@Stops", SqlDbType.VarChar, 10).Value = selectedStops;
                d.cmdd.Parameters.Add("@EverywhereFrom", SqlDbType.Bit).Value = everywhereFrom;
                d.cmdd.Parameters.Add("@GreenDiff", SqlDbType.Bit).Value = greenDiff;
                d.cmdd.Parameters.Add("@RedDiff", SqlDbType.Bit).Value = redDiff;
            }


            else if (frm == "" && to != "")
            {
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 200).Value = to;
                d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 200).Value = txtAirline.Text;
                d.cmdd.Parameters.Add("@Aircode", SqlDbType.VarChar, 50).Value = txtAircode.Text;
                d.cmdd.Parameters.Add("@Days", SqlDbType.VarChar, 10).Value = selectedDays;
                d.cmdd.Parameters.Add("@Cabin", SqlDbType.VarChar, 100).Value = selectedCabin;
                d.cmdd.Parameters.Add("@Shortstays", SqlDbType.Bit).Value = shortStays;
                d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = dateFromVar;
                d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = dateToVar;
                d.cmdd.Parameters.Add("@IsBetween", SqlDbType.Bit).Value = varRadioBetween;
                d.cmdd.Parameters.Add("@IsGreater", SqlDbType.Bit).Value = varRadioGreater;
                d.cmdd.Parameters.Add("@IsLess", SqlDbType.Bit).Value = varRadioLess;
                d.cmdd.Parameters.Add("@MinPrice", SqlDbType.Float).Value = varMinPrice;
                d.cmdd.Parameters.Add("@MaxPrice", SqlDbType.Float).Value = varMaxPrice;
                d.cmdd.Parameters.Add("@Stops", SqlDbType.VarChar, 10).Value = selectedStops;
                d.cmdd.Parameters.Add("@EverywhereTo", SqlDbType.Bit).Value = everywhereTo;
                d.cmdd.Parameters.Add("@GreenDiff", SqlDbType.Bit).Value = greenDiff;
                d.cmdd.Parameters.Add("@RedDiff", SqlDbType.Bit).Value = redDiff;
            }
            else if (frm != "" && to != "")
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 200).Value = frm;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 200).Value = to;
                d.cmdd.Parameters.Add("@IsTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 200).Value = txtAirline.Text;
                d.cmdd.Parameters.Add("@Aircode", SqlDbType.VarChar, 50).Value = txtAircode.Text;
                d.cmdd.Parameters.Add("@Days", SqlDbType.VarChar, 10).Value = selectedDays;
                d.cmdd.Parameters.Add("@Cabin", SqlDbType.VarChar, 100).Value = selectedCabin;
                d.cmdd.Parameters.Add("@Shortstays", SqlDbType.Bit).Value = shortStays;
                d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = dateFromVar;
                d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = dateToVar;
                d.cmdd.Parameters.Add("@IsBetween", SqlDbType.Bit).Value = varRadioBetween;
                d.cmdd.Parameters.Add("@IsGreater", SqlDbType.Bit).Value = varRadioGreater;
                d.cmdd.Parameters.Add("@IsLess", SqlDbType.Bit).Value = varRadioLess;
                d.cmdd.Parameters.Add("@MinPrice", SqlDbType.Float).Value = varMinPrice;
                d.cmdd.Parameters.Add("@MaxPrice", SqlDbType.Float).Value = varMaxPrice;
                d.cmdd.Parameters.Add("@Stops", SqlDbType.VarChar, 10).Value = selectedStops;
                d.cmdd.Parameters.Add("@EverywhereFrom", SqlDbType.Bit).Value = everywhereFrom;
                d.cmdd.Parameters.Add("@EverywhereTo", SqlDbType.Bit).Value = everywhereTo;
                d.cmdd.Parameters.Add("@GreenDiff", SqlDbType.Bit).Value = greenDiff;
                d.cmdd.Parameters.Add("@RedDiff", SqlDbType.Bit).Value = redDiff;
            }
            else if (frm == "" && to == "")
            {
                //d.cmdd.Parameters.Clear();
                d.cmdd.Parameters.Add("@IsTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 200).Value = txtAirline.Text;
                d.cmdd.Parameters.Add("@Aircode", SqlDbType.VarChar, 50).Value = txtAircode.Text;
                d.cmdd.Parameters.Add("@Days", SqlDbType.VarChar, 10).Value = selectedDays;
                d.cmdd.Parameters.Add("@Cabin", SqlDbType.VarChar, 100).Value = selectedCabin;
                d.cmdd.Parameters.Add("@Shortstays", SqlDbType.Bit).Value = shortStays;
                d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = dateFromVar;
                d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = dateToVar;
                d.cmdd.Parameters.Add("@IsBetween", SqlDbType.Bit).Value = varRadioBetween;
                d.cmdd.Parameters.Add("@IsGreater", SqlDbType.Bit).Value = varRadioGreater;
                d.cmdd.Parameters.Add("@IsLess", SqlDbType.Bit).Value = varRadioLess;
                d.cmdd.Parameters.Add("@MinPrice", SqlDbType.Float).Value = varMinPrice;
                d.cmdd.Parameters.Add("@MaxPrice", SqlDbType.Float).Value = varMaxPrice;
                d.cmdd.Parameters.Add("@Stops", SqlDbType.VarChar, 10).Value = selectedStops;
                d.cmdd.Parameters.Add("@GreenDiff", SqlDbType.Bit).Value = greenDiff;
                d.cmdd.Parameters.Add("@RedDiff", SqlDbType.Bit).Value = redDiff;
            }

            // Set a reasonable timeout (120 seconds = 2 minutes) for large database queries
            d.cmdd.CommandTimeout = 120;
            d.cmdd.Connection = d.cn;

            // Ensure connection is open
            if (d.cn.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    d.connecter();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to connect to database.\n\n" +
                                  "Please check your internet connection and database settings.\n\n" +
                                  $"Error: {ex.Message}", 
                                  "Database Connection Error", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Error);
                    return;
                }
            }

            try
            {
                // Run database query asynchronously to prevent UI blocking
                DataTable tempTable = new DataTable();
                await Task.Run(() =>
                {
                    using (SqlDataReader reader = d.cmdd.ExecuteReader())
                    {
                        tempTable.Load(reader);
                    }
                });
                
                // Update the main DataTable on UI thread
                d.dt = tempTable;
            }
            catch (SqlException ex)
            {
                // Check if it's a timeout error
                if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    MessageBox.Show($"Search timed out after 2 minutes.\n\n" +
                                  "The database query is taking too long. This could be due to:\n" +
                                  "1. Large amount of data matching your criteria\n" +
                                  "2. Slow internet connection\n" +
                                  "3. Database server load\n\n" +
                                  "Please try narrowing your search criteria (add FROM/TO locations, dates, etc.)", 
                                  "Search Timeout", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Warning);
                }
                // Stored procedure doesn't exist
                else if (ex.Message.Contains("Could not find stored procedure"))
                {
                    MessageBox.Show($"Stored procedure '{nameProc}' not found in database.\n\n" +
                                  "Please run 'CreatePlaceholderStoredProcedures.sql' to create placeholder procedures,\n" +
                                  "or restore your database from a backup.\n\n" +
                                  $"Error: {ex.Message}", 
                                  "Missing Stored Procedure", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Database error during search:\n\n{ex.Message}\n\n" +
                                  "SQL Error Number: {ex.Number}", 
                                  "Database Error", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Error);
                }
                return; // Exit early - don't crash the app
            }
            catch (Exception ex)
            {
                // Catch any other exceptions to prevent app crash
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg += "\n\nInner Error: " + ex.InnerException.Message;
                }
                
                MessageBox.Show($"Error executing search:\n\n{errorMsg}\n\n" +
                              "The application will continue running. Please try a different search.", 
                              "Search Error", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Error);
                return; // Exit early - don't crash the app
            }

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            
            // Check if new categorization columns exist in the result set
            bool hasIsOldTarget = d.dt.Columns.Contains("IsOldTarget");
            bool hasIsMonthTarget = d.dt.Columns.Contains("IsMonthTarget");
            bool hasTargetDeal = d.dt.Columns.Contains("TargetDeal");
            
            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;
                bool? IsOldTarget   = d.dt.Columns.Count > 20 ? d.dt.Rows[i][20] as bool? : null;
                bool? IsMonthTarget = d.dt.Columns.Count > 21 ? d.dt.Rows[i][21] as bool? : null;
                bool? IsTargetDeal  = d.dt.Columns.Count > 22 ? d.dt.Rows[i][22] as bool? : null;

                if (radioTargetDeals.Checked  && !(IsTargetDeal.HasValue  && IsTargetDeal.Value))  continue;
                if (radioTargetMonths.Checked && !(IsMonthTarget.HasValue && IsMonthTarget.Value)) continue;
                if (double.Parse(d.dt.Rows[i][5].ToString()) == 0) continue; // skip rows with no current price

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), !string.IsNullOrEmpty(d.dt.Rows[i][16].ToString()) ? double.Parse(d.dt.Rows[i][16].ToString()) : 0, !string.IsNullOrEmpty(d.dt.Rows[i][16].ToString()) ? double.Parse(d.dt.Rows[i][17].ToString()) : double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString(), DateTime.TryParse(d.dt.Rows[i][18]?.ToString(), out var dt) ? dt : (DateTime?)null);

                // Apply color based on target categorization (priority: Green > Purple > Yellow > Blue)
                bool isOldTarget = false;
                bool isMonthTarget = false;
                bool targetDeal = false;
                
                if (hasTargetDeal && d.dt.Rows[i]["TargetDeal"] != DBNull.Value)
                    bool.TryParse(d.dt.Rows[i]["TargetDeal"].ToString(), out targetDeal);
                if (hasIsMonthTarget && d.dt.Rows[i]["IsMonthTarget"] != DBNull.Value)
                    bool.TryParse(d.dt.Rows[i]["IsMonthTarget"].ToString(), out isMonthTarget);
                if (hasIsOldTarget && d.dt.Rows[i]["IsOldTarget"] != DBNull.Value)
                    bool.TryParse(d.dt.Rows[i]["IsOldTarget"].ToString(), out isOldTarget);
                
                // Apply color with priority: Green > Purple > Yellow > Blue
                if (targetDeal)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (isMonthTarget)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MediumPurple;
                }
                else if (isOldTarget)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }
            }
            catch (Exception ex)
            {
                // Handle any other errors
                MessageBox.Show($"Error executing search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        public int cnt = 0;


        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();
        public async void datagridvColor()
        {

            try
            {
                await Task.Run(() =>
                {
                    if (dataGridView1.InvokeRequired)
                    {
                        dataGridView1.Invoke(new Action(() =>
                        {
                            ApplyDatagridColors();
                        }));
                    }
                    else
                    {
                        ApplyDatagridColors();
                    }
                });
            }
            catch { }
        }
        
        private void ApplyDatagridColors()
        {
            try
            {
                // Color the Difference column (cell 8) based on value
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    if (row.Cells[8].Value != null)
                    {
                        double diffValue = Convert.ToDouble(row.Cells[8].Value);
                        
                        if (diffValue < 0)
                        {
                            row.Cells[8].Style.BackColor = Color.LightGreen;
                        }
                        else if (diffValue > 0)
                        {
                            row.Cells[8].Style.BackColor = Color.Red;
                        }
                        if (diffValue == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                        {
                            row.Cells[8].Style.BackColor = Color.Orange;
                        }
                        if (diffValue == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                        {
                            row.Cells[8].Style.BackColor = Color.Gray;
                        }
                    }
                }
                
                // Color the FROM column (cell 1) if it matches hotel data
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    if (row.Cells[1].Value != null)
                    {
                        for (int i = 0; i < dthtl.Rows.Count; i++)
                        {
                            if (Convert.ToString(row.Cells[1].Value).Equals(dthtl.Rows[i][0].ToString()))
                            {
                                row.Cells[1].Style.BackColor = Color.YellowGreen;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ApplyDatagridColors: {ex.Message}");
            }
        }

        string cbnB1, cbnB2, cbnB3, cbnB4;

        string price1, price2, price3, price4, price5, price6;

        private async void button4_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("Please enter the file name! ", "the file name", "", -1, -1);
            if (str != "")
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                int i = 0;
                int j = 0;
                int c = dataGridView1.RowCount;

                await Task.Run(() =>
                {
                    for (i = 0; i <= dataGridView1.RowCount - 1; i++)
                    {
                        for (j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                        {
                            DataGridViewCell cell = dataGridView1[j, i];
                            xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
                        }
                    }
                });
                str = str + ".xls";
                xlWorkBook.SaveAs(str, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                MessageBox.Show("Excel file created , you can find the file c:\\" + str);

            }
        }



        string pricecabin1, pricecabin2, pricecabin3, pricecabin4;
        double minP, maxP;

        private void txtMinPrice_TextChanged(object sender, EventArgs e)
        {
            if (!radioBetween.Checked && !radioGreater.Checked && !radioLess.Checked)
            {
                // None of the radio buttons is checked, show an error message
                MessageBox.Show("Please select a radio button before entering input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkGreenDiff_CheckedChanged(object sender, EventArgs e)
        {
            if (checkGreenDiff.Checked)
            {
                checkRedDiff.Checked = false;
            }
        }

        private void checkRedDiff_CheckedChanged(object sender, EventArgs e)
        {
            if (checkRedDiff.Checked)
            {
                checkGreenDiff.Checked = false;
            }
        }

        private void radioBtnDate_CheckedChanged(object sender, EventArgs e)
        {
            chkShortStays.Enabled = true;
        }

        private void radioBtnNoDate_CheckedChanged(object sender, EventArgs e)
        {
            chkShortStays.Checked = false;
            chkShortStays.Enabled = false;
        }

        private void FromToDates(string adrss, string from, string to, bool isTargetOnly, string fromdate, string todate)
        {
            dataGridView1.Rows.Clear();
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = adrss;
            d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = from;
            d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = to;
            d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = fromdate;
            d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = todate;
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            d.cmdd.CommandTimeout = 120; // 2 minute timeout
            d.cmdd.Connection = d.cn;

            try
            {
                d.dt.Load(d.cmdd.ExecuteReader());
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    MessageBox.Show("Search timed out after 2 minutes.\n\nPlease try narrowing your search criteria.", 
                                  "Search Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cnt = d.dt.Rows.Count;



            if (cnt == 0)
            {
                    MessageBox.Show("The information entered is not on the database!");

            }
            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;
                bool? IsOldTarget   = d.dt.Columns.Count > 20 ? d.dt.Rows[i][20] as bool? : null;
                bool? IsMonthTarget = d.dt.Columns.Count > 21 ? d.dt.Rows[i][21] as bool? : null;
                bool? IsTargetDeal  = d.dt.Columns.Count > 22 ? d.dt.Rows[i][22] as bool? : null;

                if (radioTargetDeals.Checked  && !(IsTargetDeal.HasValue  && IsTargetDeal.Value))  continue;
                if (radioTargetMonths.Checked && !(IsMonthTarget.HasValue && IsMonthTarget.Value)) continue;
                if (double.Parse(d.dt.Rows[i][5].ToString()) == 0) continue; // skip rows with no current price

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString(), DateTime.TryParse(d.dt.Rows[i][18]?.ToString(), out var dt) ? dt : (DateTime?)null);

                if (IsTargetDeal.HasValue && IsTargetDeal.Value)
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                else if (IsMonthTarget.HasValue && IsMonthTarget.Value)
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MediumPurple;
                else if (IsTargetFound.HasValue && IsTargetFound.Value)
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                else if (IsOldTarget.HasValue && IsOldTarget.Value)
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
            }
            datagridvColor();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Columns[11].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridView1.Columns[11].DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (e.RowIndex > -1)
                {
                    String[] spearator = { "https://" };

                    var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string str = val;
                    int index = e.RowIndex;
                    string date = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    string[] tbl = str.Split(spearator, StringSplitOptions.None);
                    cnt = 0;
                    cnt = tbl.Length;

                    if (cnt >= 2)
                    {
                        Process.Start(val);
                    }

                    for (int i = 0; i < dthtl.Rows.Count; i++)
                    {
                        if (str.Equals(dthtl.Rows[i][0].ToString()))
                        {

                            Hotel h = new Hotel(str, date);
                            h.Show();
                        }
                    }

                }
            }
            catch { }
            datagridvColor();
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            // Disable search button to prevent multiple clicks
            button13.Enabled = false;
            string originalButtonText = button13.Text;
            button13.Text = "Searching...";
            
            // Force UI update
            button13.Refresh();
            Application.DoEvents();
            
            try
            {
                label6.Text = "";
                dataGridView1.Visible = true;

                dataGridView2.Visible = false;
                dataGridView1.Rows.Clear();

                cbnB1 = "serchFromToMultiGroupCityGOOGleAirlineEverywhere";
                cbnB2 = "serchFromMultiGroupCityGOOGleAirlineEverywhere";
                cbnB3 = "serchToMultiGroupCityGOOGleAirlineEverywhere";
                cbnB4 = "serchWithoutFromToGOOGleAirline";
                string frm = textBox5.Text;
                string to = textBox6.Text;
                if (frm.ToLower() == "everywhere")
                {
                    frm = string.Empty;
                }
                if (to.ToLower() == "everywhere")
                {
                    to = string.Empty;
                }
                if (frm != "" && to != "")
                {
                    await searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB1);
                    datagridvColor();
                }
                else if (frm != "" && to == "")
                {
                    await searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB2);
                    datagridvColor();
                }
                else if (frm == "" && to != "")
                {
                    await searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB3);
                    datagridvColor();
                }
                else if(frm == "" && to == "")
                {
                    await searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB4);
                    datagridvColor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable button and restore original text
                button13.Enabled = true;
                button13.Text = originalButtonText;
            }
        }

        private void chkTarget_CheckedChanged(object sender, EventArgs e)
        {
            bool on = chkTarget.Checked;
            radioTargetAll.Enabled    = on;
            radioTargetDeals.Enabled  = on;
            radioTargetMonths.Enabled = on;
            if (!on) radioTargetAll.Checked = true;
        }

        private void radioGreater_CheckedChanged(object sender, EventArgs e)
        {
            txtMaxPrice.Visible = false;
            labelMaxPrice.Visible = false;
        }

        private void radioLess_CheckedChanged(object sender, EventArgs e)
        {
            txtMaxPrice.Visible = false;
            labelMaxPrice.Visible = false;
        }

        private void radioBetween_CheckedChanged(object sender, EventArgs e)
        {
            txtMaxPrice.Visible = true;
            labelMaxPrice.Visible = true;
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


        public void somme(float a, float b, bool isTargetOnly, string str)
        {
            dataGridView1.Rows.Clear();

            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;

            if (a != 99999 && b == 99999)
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;

            else if (a == 99999 && b != 99999)
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;

            else if (a != 99999 && b != 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = a;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = b;
            }
            d.cmdd.Parameters.Add("@isTargetOnly", SqlDbType.Bit).Value = isTargetOnly;
            d.cmdd.CommandTimeout = 120; // 2 minute timeout
            d.cmdd.Connection = d.cn;

            try
            {
                d.dt.Load(d.cmdd.ExecuteReader());
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2 || ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    MessageBox.Show("Search timed out after 2 minutes.\n\nPlease try narrowing your search criteria.", 
                                  "Search Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cnt = d.dt.Rows.Count;


                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {
                    bool? IsTargetFound = d.dt.Rows[i][14] as bool?;
                    bool? IsOldTarget   = d.dt.Columns.Count > 20 ? d.dt.Rows[i][20] as bool? : null;
                    bool? IsMonthTarget = d.dt.Columns.Count > 21 ? d.dt.Rows[i][21] as bool? : null;
                    bool? IsTargetDeal  = d.dt.Columns.Count > 22 ? d.dt.Rows[i][22] as bool? : null;

                    if (radioTargetDeals.Checked  && !(IsTargetDeal.HasValue  && IsTargetDeal.Value))  continue;
                    if (radioTargetMonths.Checked && !(IsMonthTarget.HasValue && IsMonthTarget.Value)) continue;
                    if (double.Parse(d.dt.Rows[i][5].ToString()) == 0) continue; // skip rows with no current price

                    int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][16].ToString()), double.Parse(d.dt.Rows[i][17].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString(), DateTime.TryParse(d.dt.Rows[i][18]?.ToString(), out var dt) ? dt : (DateTime?)null);

                    if (IsTargetDeal.HasValue && IsTargetDeal.Value)
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    else if (IsMonthTarget.HasValue && IsMonthTarget.Value)
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MediumPurple;
                    else if (IsTargetFound.HasValue && IsTargetFound.Value)
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                    else if (IsOldTarget.HasValue && IsOldTarget.Value)
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }

            datagridvColor();
        }


        private void comb()
        {
            d.ds.Clear();

            // Fill the DataTable with distinct Stops from the database
            d.da = new SqlDataAdapter("select distinct Stops from comprGOOGLAirline", d.cn);
            d.da.Fill(d.ds, "comST");

            // Create a new row for "Please Select"
            DataRow pleaseSelectRow = d.ds.Tables["comST"].NewRow();
            pleaseSelectRow["Stops"] = "Please Select";

            // Insert the "Please Select" row at the 0 index
            d.ds.Tables["comST"].Rows.InsertAt(pleaseSelectRow, 0);

            // Set up the ComboBox
            ddlStops.DataSource = d.ds.Tables["comST"];
            ddlStops.DisplayMember = "Stops";
            ddlStops.ValueMember = "Stops";


            //DAYS
            // Fill the DataTable with distinct Days from the database
            d.da = new SqlDataAdapter("select distinct Days from comprGOOGLAirline", d.cn);
            d.da.Fill(d.ds, "comDays");

            // Create a new row for "Please Select"
            DataRow pleaseSelectRowDays = d.ds.Tables["comDays"].NewRow();
            pleaseSelectRowDays["Days"] = "Please Select";

            // Insert the "Please Select" row at the 0 index
            d.ds.Tables["comDays"].Rows.InsertAt(pleaseSelectRowDays, 0);

            // Set up the ComboBox
            ddlDays.DataSource = d.ds.Tables["comDays"];
            ddlDays.DisplayMember = "Days";
            ddlDays.ValueMember = "Days";


            //CABIN
            // Fill the DataTable with distinct Cabin from the database
            d.da = new SqlDataAdapter("select distinct Cabin from comprGOOGLAirline", d.cn);
            d.da.Fill(d.ds, "comCabin");

            // Create a new row for "Please Select"
            DataRow pleaseSelectRowCabin = d.ds.Tables["comCabin"].NewRow();
            pleaseSelectRowCabin["Cabin"] = "Please Select";

            // Insert the "Please Select" row at the 0 index
            d.ds.Tables["comCabin"].Rows.InsertAt(pleaseSelectRowCabin, 0);

            // Set up the ComboBox
            ddlCabin.DataSource = d.ds.Tables["comCabin"];
            ddlCabin.DisplayMember = "Cabin";
            ddlCabin.ValueMember = "Cabin";
        }


        private void GoogleAirline_Load(object sender, EventArgs e)
        {
            // Add items to the ComboBox
            //ddlDays.Items.Add(new ComboBoxItem("Please Select Days", ""));
            //ddlDays.Items.Add(new ComboBoxItem("14 days", "14"));
            //ddlDays.Items.Add(new ComboBoxItem("13 days", "13"));
            //ddlDays.Items.Add(new ComboBoxItem("12 days", "12"));
            //ddlDays.Items.Add(new ComboBoxItem("8 days", "8"));
            //ddlDays.Items.Add(new ComboBoxItem("7 days", "7"));
            //ddlDays.Items.Add(new ComboBoxItem("4 days", "4"));
            //ddlDays.Items.Add(new ComboBoxItem("3 days", "3"));
            //ddlDays.Items.Add(new ComboBoxItem("2 days", "2"));

            //// Set the default selected index (optional)
            //ddlDays.SelectedIndex = 0; // Set to the index of the desired default item

            //set the no date radio as selected
            radioBtnNoDate.Checked = true;

            d.connecter();
            //load comobox ddl data
            comb();
            //label5.Visible = false;

            dataGridView2.Visible = false;
            dataGridView1.Visible = true;


            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
