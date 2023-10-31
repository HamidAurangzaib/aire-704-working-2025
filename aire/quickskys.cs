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

namespace aire
{
    public partial class quickskys : Form
    {
        ado d = new ado();
        public quickskys()
        {
            InitializeComponent();
        }

        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();

        public async void datagridvColor()
        {
            dshtl.Clear();
            dthtl.Rows.Clear();

            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
            try
            {
                await Task.Run(() =>
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToDouble(row.Cells[7].Value) < 0)
                    {
                        row.Cells[7].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[7].Value) > 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0 && Convert.ToDouble(row.Cells[6].Value) == 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Gray;

                    }
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

        public async void colors()
        {
            
            await Task.Run(() =>
            {
               
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (Convert.ToDouble(row.Cells[7].Value) < 0)
                    {
                        row.Cells[7].Style.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToDouble(row.Cells[7].Value) > 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) == 0 && Convert.ToDouble(row.Cells[6].Value) > 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Orange;
                    }
                    if (Convert.ToDouble(row.Cells[7].Value) == 0 && Convert.ToDouble(row.Cells[5].Value) > 0 && Convert.ToDouble(row.Cells[6].Value) == 0)
                    {
                        row.Cells[7].Style.BackColor = Color.Gray;

                    }
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
        DataSet ds1 = new DataSet();
        private void comb()
        {
            d.da = new SqlDataAdapter("select distinct [From] from quicksky", d.cn);
            d.da.Fill(d.ds, "com1");
            d.da = new SqlDataAdapter("select distinct [To] from quicksky", d.cn);
            d.da.Fill(ds1, "com2");
            comboBox2.DataSource = d.ds.Tables["com1"];
            comboBox2.DisplayMember = "From";
            comboBox2.ValueMember = "From";

            comboBox3.DataSource = ds1.Tables["com2"];
            comboBox3.DisplayMember = "To";
            comboBox3.ValueMember = "To";
        }

        private void quickskys_Load(object sender, EventArgs e)
        {
            button11.Visible = false;
            button12.Visible = false;
            d.connecter();
            comb();
            textBox1.Text = "";
            textBox2.Text = "";
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;

            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
        }
        public void searchFROMTO(string frm , string to,string nameproc)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproc;
            if(frm!="" && to!="")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            else if(frm != "" && to == "")
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
            }
            else if(frm=="" && to!="")
            {
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
            }
            d.cmdd.Connection = d.cn;
            
            d.dt.Load(d.cmdd.ExecuteReader());
           
            int cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(),
                double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));

            }


        }
       
      

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                searchFROMTO(textBox1.Text,textBox2.Text, "serchFromtoquick");

                datagridvColor();

            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                searchFROMTO(textBox1.Text,"", "serchFromquick");
                datagridvColor();

            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                searchFROMTO("", textBox2.Text, "serchToquick");

                datagridvColor();

            }
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            if (radioButton1.Checked == true)
            {
               
                price(float.Parse(minPrice.Text), 99999, "pricequick1");
                
            }
            else if (radioButton2.Checked == true)
            {
               

                price(float.Parse(minPrice.Text),99999, "pricequicklow");
            }
            else if (radioButton3.Checked == true)
            {
                
                price(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "pricequick");
               
            }
            datagridvColor();
        }
        public void price(float min,float max,string nameproc)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproc;
            if(min!=99999 && max!=99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = min;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = max;
            }
           else if (min != 99999 && max == 99999)
            {
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = min;
                
            }

            d.cmdd.Connection = d.cn;
            
            d.dt.Load(d.cmdd.ExecuteReader());
            
            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(),
                double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));

            }
        }

        double minP, maxP;
        private void myfunction()
        {
            if (min.Text != "" && max.Text != "")
            {
                minP = double.Parse(min.Text);
                maxP = double.Parse(max.Text);
            }
            else if (min.Text != "" && max.Text == "")
            {
                minP = double.Parse(min.Text);
                maxP = double.Parse(min.Text);
            }
            else
            {
                minP = double.Parse(max.Text);
                maxP = double.Parse(max.Text);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();

            if (min.Text=="" && max.Text=="")
            {
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "datequick";
                d.cmdd.Parameters.Add("@Date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@Date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");

                d.cmdd.Connection = d.cn;

                d.dt.Load(d.cmdd.ExecuteReader());

                int cnt = d.dt.Rows.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {
                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(),
                    double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));

                }

            }
            else
            {
                myfunction();
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "searchDatePricequicksky";
                d.cmdd.Parameters.Add("@dateA", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@dateB", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = minP;
                d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = maxP;

                d.cmdd.Connection = d.cn;

                d.dt.Load(d.cmdd.ExecuteReader());

                int cnt = d.dt.Rows.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {
                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(),
                    double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));

                }
            }
            min.Text = "";
            max.Text = "";

            datagridvColor();
        }

        public void price_frm_to(string frm,string to,float min,float max ,string nameproc)
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproc;
            if((frm != "" || to != "") && min!=99999 && max!=99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = min;
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = max;
            }
           else if((frm != "" || to != "") && min != 99999 && max == 99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = min;
            }


            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(),
                double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (textBox1.Text!="" || textBox2.Text!="")
            {
                if (radioButton1.Checked == true && minPrice.Text != "")
                {
                  
                    price_frm_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text),99999,"serchFromTopricequickbig");
                   
                }
               
                else if(radioButton2.Checked==true && minPrice.Text!="")
                {
                    
                    price_frm_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopricequick");
                    
                }
                
               else if (radioButton3.Checked==true && maxprice.Text!="" && minPrice.Text!="")
                {
                    price_frm_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopricequickbetween");
                   
                }
                else
                {
                    MessageBox.Show("You can only use the new price");
                }
            }
           
            datagridvColor();
            textBox1.Text = "";
            textBox2.Text = "";
            maxprice.Text = "";
            minPrice.Text = "";
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
        private async void button7_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            updatequick q = new updatequick();
            q.ShowDialog();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
            label4.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = false;
            label4.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            maxprice.Visible = true;
            label4.Visible = true;
        }
      
        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            colors();
        }


        int pagenumber = 1;
        IPagedList<quicksky> list;
        public async Task<IPagedList<quicksky>> GetPagedListAsync(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (quicknewEntities db = new quicknewEntities())
                {
                    return db.quickskies.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }


        private async void button10_Click(object sender, EventArgs e)
        {
            button11.Visible = true;
            button12.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;

            list = await GetPagedListAsync();
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            colors();
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            list = await GetPagedListAsync(++pagenumber);
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            colors();
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            list = await GetPagedListAsync(--pagenumber);
            button11.Enabled = list.HasPreviousPage;
            button12.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            dataGridView2.Columns.Remove("id");
            colors();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox2.SelectedValue.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox3.SelectedValue.ToString();
        }

        private void cityquick()
        {
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "citytquicksky";
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 50).Value = textBox4.Text;
           

            d.cmdd.Connection = d.cn;
            
            d.dt.Load(d.cmdd.ExecuteReader());
           
            int cnt = d.dt.Rows.Count;

            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()), d.dt.Rows[i][4].ToString(),
                double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            cityquick();
            datagridvColor();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string str = "9";
            Information_about_files inf = new Information_about_files(str);
            inf.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                string str = val;
                int index = e.RowIndex;
                string date = dataGridView1.Rows[index].Cells[3].Value.ToString();

                for (int i = 0; i < dthtl.Rows.Count; i++)
                {
                    if (str.Equals(dthtl.Rows[i][0].ToString()))
                    {
                        Hotel h = new Hotel(str,date);
                        h.Show();
                    }
                }
            }
            catch { }
            colors();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Old_Price = 0 and New_price > 0", "New_Price desc");
            foreach (DataRow dr in ligne)
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                dr[4].ToString(), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()),double.Parse(dr[8].ToString()));
            }

            datagridvColor();
            ligne = null;
            radioButton4.Checked = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int cntDt = d.dt.Rows.Count;
            for (int i = 0; i < cntDt; i++)
            {
                dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    d.dt.Rows[i][4].ToString(), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));
            }

            datagridvColor();
            radioButton5.Checked = false;
        }
        int cnt;
        private void FromToDates(string adrss, string from, string to, string fromdate, string todate)
        {
           
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = adrss;
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = from;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = to;
                d.cmdd.Parameters.Add("@Fromdate", SqlDbType.Date).Value = fromdate;
                d.cmdd.Parameters.Add("@Todate", SqlDbType.Date).Value = todate;
         
                d.cmdd.Connection = d.cn;

                d.dt.Load(d.cmdd.ExecuteReader());

                cnt = d.dt.Rows.Count;
            

                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    else
                    {

                        for (int i = 0; i < cnt; i++)
                        {
                         dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         d.dt.Rows[i][4].ToString(), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), double.Parse(d.dt.Rows[i][8].ToString()));
                        }
                    }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (date1.Value < date2.Value)
            {
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesquickSky", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesquickSky", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesquickSky", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
            }
            else
            {
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesquickSky", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesquickSky", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesquickSky", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), ""); }
            }
        }
    }
}
