using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using PagedList;
using System.Linq;
using System.Diagnostics;


namespace aire
{
    public partial class search_skys_flight_copy : Form
    {
        int domest;
        string cbnB1, cbnB2, cbnB3, cbnB4;
        public search_skys_flight_copy(int domestic)
        {
            InitializeComponent();
            domest = domestic;
        }
        ado d = new ado();
        DataTable dt = new DataTable();
        DataSet ds1 = new DataSet();
        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();


        private void comb()
        {
            d.ds.Clear();

            // Fill the DataTable with distinct Stops from the database
            d.da = new SqlDataAdapter("select distinct Stops from comprGOOGLCOPY", d.cn);
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
            d.da = new SqlDataAdapter("select distinct Days from comprGOOGLCOPY", d.cn);
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
            d.da = new SqlDataAdapter("select distinct Cabin from comprGOOGLCOPY", d.cn);
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

        private void search_skys_flight_copy_Load(object sender, EventArgs e)
        {
            //set the no date radio as selected
            radioBtnNoDate.Checked = true;

            chkNewPrice.Checked = true;

            chkShortStays.Enabled = false;

            d.connecter();
            comb();
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;


            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }
        public async void datagridvColor()
        {

            try
            {
                await Task.Run(() =>
                {

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {

                        if (Convert.ToDouble(row.Cells[6].Value) < 0)
                        {
                            row.Cells[6].Style.BackColor = Color.LightGreen;
                        }
                        else if (Convert.ToDouble(row.Cells[6].Value) > 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Red;
                        }
                        if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Orange;
                        }
                        if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                        {
                            row.Cells[6].Style.BackColor = Color.Gray;

                        }
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        for (int i = 0; i < dthtl.Rows.Count; i++)
                        {
                            if (Convert.ToString(row.Cells[1].Value).Equals(dthtl.Rows[i][0].ToString()))
                            {
                                row.Cells[1].Style.BackColor = Color.YellowGreen;
                            }
                        }
                    }
                });
            }
            catch { }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            google_copy ggl = new google_copy(domest);
            ggl.Show();
        }
        int pagenumber = 1;
        IPagedList<comprGOOGLCOPY> list;
        public async Task<IPagedList<comprGOOGLCOPY>> GetPagedListAsync(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (DB_A61545_andycomEntities12 db = new DB_A61545_andycomEntities12())
                {
                    return db.comprGOOGLCOPies.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }

        IPagedList<comprGOOGL2Days> list2;
        public async Task<IPagedList<comprGOOGL2Days>> GetPagedListAsync2(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (domestic db = new domestic())
                {
                    return db.comprGOOGL2Days.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        IPagedList<comprGOOGL3Days> list3;
        public async Task<IPagedList<comprGOOGL3Days>> GetPagedListAsync3(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (domestic db = new domestic())
                {
                    return db.comprGOOGL3Days.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        IPagedList<comprGOOGL4Days> list4;
        public async Task<IPagedList<comprGOOGL4Days>> GetPagedListAsync4(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (domestic db = new domestic())
                {
                    return db.comprGOOGL4Days.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }

        IPagedList<comprGOOGL14Days> list14;
        public async Task<IPagedList<comprGOOGL14Days>> GetPagedListAsync14(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (domestic db = new domestic())
                {
                    return db.comprGOOGL14Days.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }

        IPagedList<comprskyCOPY> list1;
        public async Task<IPagedList<comprskyCOPY>> GetPagedListAsync1(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (DB_A61545_andycomEntities12 db = new DB_A61545_andycomEntities12())
                {
                    return db.comprskyCOPies.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        private  void deleteclmn()
        {
            
                dataGridView2.Columns.Remove("id");
           
        }
        private async void color()
        {
           
            await Task.Run(() =>
            {
               
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (Convert.ToDouble(row.Cells[6].Value) < 0)
                    {
                        row.Cells[6].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[6].Value) == 0 && Convert.ToDouble(row.Cells[4].Value) > 0 && Convert.ToDouble(row.Cells[5].Value) == 0)
                    {
                        row.Cells[6].Style.BackColor = Color.Gray;

                    }
                }
            });
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Information_about_files i = new Information_about_files("2347");
            i.ShowDialog();
        }
        public int cnt = 0;
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Columns[13].DefaultCellStyle.SelectionForeColor = Color.Blue;
            dataGridView2.Columns[13].DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView2.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (e.RowIndex > -1)
            {
                String[] spearator = { "https://" };
               
                var val = this.dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();
                string str = val;



                string[] tbl = str.Split(spearator, StringSplitOptions.None);
                 cnt = 0;
                cnt = tbl.Length;

                if (cnt >= 2)
                {
                    Process.Start(val);
                }
            }
            color();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMinPrice_TextChanged(object sender, EventArgs e)
        {
            if ((!radioBetween.Checked && !radioGreater.Checked && !radioLess.Checked) || (!chkNewPrice.Checked && !chkDiffPrice.Checked))
            {
                // None of the radio buttons is checked, show an error message
                MessageBox.Show("Please select a radio button (between / greater / less) and a checkbox (new price / diff price) before entering input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void chkNewPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewPrice.Checked)
            {
                chkDiffPrice.Checked = false;
            }
        }

        private void chkDiffPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDiffPrice.Checked)
            {
                chkNewPrice.Checked = false;
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

                            Hotel h = new Hotel(str,date);
                            h.Show();
                        }
                    }

                }
            }
            catch { }
            datagridvColor();


        }

        private void button13_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();

            cbnB1 = "serchFromToMultiGroupCityGOOGleDomesticEverywhere";
            cbnB2 = "serchFromMultiGroupCityGOOGleDomesticEverywhere";
            cbnB3 = "serchToMultiGroupCityGOOGleDomesticEverywhere";
            cbnB4 = "serchWithoutFromToGOOGleDomestic";

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

                searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB1);

                datagridvColor();
            }
            else if (frm != "" && to == "")
            {

                searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB2);

                datagridvColor();
            }
            else if (frm == "" && to != "")
            {

                searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB3);

                datagridvColor();
            }
            else if (frm == "" && to == "")
            {
                searchformultigroupcitydata(frm, to, chkTarget.Checked, cbnB4);

                datagridvColor();
            }
        }
        public void searchformultigroupcitydata(string frm, string to, bool isTargetOnly, string nameProc)
        {
            d.dt.Rows.Clear();

            d.dt.Clear();
            d.dt.Columns.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;

            d.cmdd.CommandText = "" + nameProc + "";

            //Get selected value of stops ddl
            string selectedStops = ddlStops.SelectedValue.ToString();
            selectedStops = selectedStops.Trim() == "Please Select" ? "" : selectedStops;
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
            if (radioBtnDate.Checked)
            {
                dateFromVar = dateFrom.Value.ToString();
                dateToVar = dateTo.Value.ToString();
            }

            if (radioBetween.Checked)
            {
                if (string.IsNullOrEmpty(txtMinPrice.Text) || string.IsNullOrEmpty(txtMaxPrice.Text))
                {
                    radioBetween.Checked = false;
                    chkNewPrice.Checked = false;
                    chkDiffPrice.Checked = false;
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
                    chkNewPrice.Checked = false;
                    chkDiffPrice.Checked = false;
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
                    chkNewPrice.Checked = false;
                    chkDiffPrice.Checked = false;
                }
                else
                {
                    radioLess.Checked = true;
                }
            }

            bool greenDiff = checkGreenDiff.Checked;
            bool redDiff = checkRedDiff.Checked;

            bool newPrice = chkNewPrice.Checked;
            bool diffPrice = chkDiffPrice.Checked;

            var varRadioBetween = radioBetween.Checked;
            var varRadioGreater = radioGreater.Checked;
            var varRadioLess = radioLess.Checked;

            bool shortStays = chkShortStays.Checked;

            if((varRadioBetween || varRadioGreater || varRadioLess) && (!newPrice && !diffPrice))
            {
                //if any of the radio is checked and neither of checkbox is checked then make newPrice as checked
                chkNewPrice.Checked = true;
                newPrice = chkNewPrice.Checked;
            }
            float varMinPrice = txtMinPrice.Text == "" || txtMinPrice.Text == null ? 0 : float.Parse(txtMinPrice.Text);
            float varMaxPrice = txtMaxPrice.Text == "" || txtMaxPrice.Text == null ? 0 : float.Parse(txtMaxPrice.Text);

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
                d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = dateFromVar;
                d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = dateToVar;
                d.cmdd.Parameters.Add("@Shortstays", SqlDbType.Bit).Value = shortStays;
                d.cmdd.Parameters.Add("@ChkNewPrice", SqlDbType.Bit).Value = newPrice;
                d.cmdd.Parameters.Add("@ChkDiffPrice", SqlDbType.Bit).Value = diffPrice;
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
                d.cmdd.Parameters.Add("@ChkNewPrice", SqlDbType.Bit).Value = newPrice;
                d.cmdd.Parameters.Add("@ChkDiffPrice", SqlDbType.Bit).Value = diffPrice;
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
                d.cmdd.Parameters.Add("@ChkNewPrice", SqlDbType.Bit).Value = newPrice;
                d.cmdd.Parameters.Add("@ChkDiffPrice", SqlDbType.Bit).Value = diffPrice;
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
                d.cmdd.Parameters.Add("@ChkNewPrice", SqlDbType.Bit).Value = newPrice;
                d.cmdd.Parameters.Add("@ChkDiffPrice", SqlDbType.Bit).Value = diffPrice;
                d.cmdd.Parameters.Add("@IsBetween", SqlDbType.Bit).Value = varRadioBetween;
                d.cmdd.Parameters.Add("@IsGreater", SqlDbType.Bit).Value = varRadioGreater;
                d.cmdd.Parameters.Add("@IsLess", SqlDbType.Bit).Value = varRadioLess;
                d.cmdd.Parameters.Add("@MinPrice", SqlDbType.Float).Value = varMinPrice;
                d.cmdd.Parameters.Add("@MaxPrice", SqlDbType.Float).Value = varMaxPrice;
                d.cmdd.Parameters.Add("@Stops", SqlDbType.VarChar, 10).Value = selectedStops;
                d.cmdd.Parameters.Add("@GreenDiff", SqlDbType.Bit).Value = greenDiff;
                d.cmdd.Parameters.Add("@RedDiff", SqlDbType.Bit).Value = redDiff;
            }

            d.cmdd.CommandTimeout = 0;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                bool? IsTargetFound = d.dt.Rows[i][14] as bool?;

                int rowIndex = dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());

                if (IsTargetFound.HasValue && IsTargetFound.Value)
                {
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }


        }
    }
}
