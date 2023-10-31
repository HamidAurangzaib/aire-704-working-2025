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

            d.da = new SqlDataAdapter("select distinct Stops from comprGOOGLCOPY", d.cn);
            d.da.Fill(d.ds, "comSTC");

            comboBox3.DataSource = d.ds.Tables["comSTC"];
            comboBox3.DisplayMember = "Stops";
            comboBox3.ValueMember = "Stops";

        }

        private void search_skys_flight_copy_Load(object sender, EventArgs e)
        {
            checkBox2.Visible = false;
            checkBox5.Visible = false;
            SKYS.Visible = false;
            button8.Visible = false;


            d.connecter();
            comb();
            label5.Visible = false;
            comboBox1.Items.Add("google");
            comboBox1.Items.Add("skyscanner");
            comboBox2.Items.Add("all");
            comboBox2.Items.Add("2 day");
            comboBox2.Items.Add("3 day");
            comboBox2.Items.Add("4 day");
            comboBox2.Items.Add("7 day");
            //comboBox2.Items.Add("14 day");
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
        public int cnt = 0;
        public void searchfordata(string frm, string to, string nameProc)
        {
            d.dt.Rows.Clear();
            if (comboBox2.Text != "")
            {

                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;

                d.cmdd.CommandText = "" + nameProc + "";

                if (frm != "" && textBox2.Text == "")
                {
                    d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                }


                else if (frm == "" && to != "")
                {
                    d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
                }
                else if (frm != "" && to != "")
                {
                    d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = frm;
                    d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = to;
                }
                d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
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
            else { MessageBox.Show("combobox DAYS is empty"); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            
            if(domest==1)
            {
                if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
                {

                    searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOGOOGleCOPY");

                    datagridvColor();
                }
                else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
                {

                    searchfordata(textBox1.Text, "", "serchFROMGOOGleCOPY");
                    datagridvColor();
                }
                else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
                {

                    searchfordata("", textBox2.Text, "serchTOGOOGleCOPY");
                    datagridvColor();
                }
                else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
                {

                    searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOskyCOPY");

                    datagridvColor();
                }
                else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
                {


                    searchfordata(textBox1.Text, "", "serchFROMskyCOPY");
                    datagridvColor();
                }
                else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
                {
                    searchfordata("", textBox2.Text, "serchTOskyCOPY");
                    datagridvColor();

                }
            }
            //else if(domest==2)
            //{
            //    if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOGOOGle2Days");

            //        datagridvColor();
            //    }
            //    else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, "", "serchFROMGOOGle2Days");
            //        datagridvColor();
            //    }
            //    else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
            //    {

            //        searchfordata("", textBox2.Text, "serchTOGOOGle2Days");
            //        datagridvColor();

            //    }
            //    else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOsky2Days");

            //        datagridvColor();


            //    }
            //    else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
            //    {


            //        searchfordata(textBox1.Text, "", "serchFROMsky2Days");
            //        datagridvColor();



            //    }
            //    else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
            //    {

            //        searchfordata("", textBox2.Text, "serchTOsky2Days");
            //        datagridvColor();

            //    }
            //}

            //else if(domest==3)
            //{
            //    if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOGOOGle3Days");

            //        datagridvColor();
            //    }
            //    else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, "", "serchFROMGOOGle3Days");
            //        datagridvColor();
            //    }
            //    else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
            //    {

            //        searchfordata("", textBox2.Text, "serchTOGOOGle3Days");
            //        datagridvColor();

            //    }
            //    else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOsky3Days");

            //        datagridvColor();


            //    }
            //    else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
            //    {


            //        searchfordata(textBox1.Text, "", "serchFROMsky3Days");
            //        datagridvColor();



            //    }
            //    else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
            //    {

            //        searchfordata("", textBox2.Text, "serchTOsky3Days");
            //        datagridvColor();

            //    }
            //}
            //else if(domest==4)
            //{
            //    if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOGOOGle4Days");

            //        datagridvColor();
            //    }
            //    else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, "", "serchFROMGOOGle4Days");
            //        datagridvColor();
            //    }
            //    else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
            //    {

            //        searchfordata("", textBox2.Text, "serchTOGOOGle4Days");
            //        datagridvColor();

            //    }
            //    else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
            //    {

            //        searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOsky4Days");

            //        datagridvColor();


            //    }
            //    else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
            //    {


            //        searchfordata(textBox1.Text, "", "serchFROMsky4Days");
            //        datagridvColor();



            //    }
            //    else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
            //    {

            //        searchfordata("", textBox2.Text, "serchTOsky4Days");
            //        datagridvColor();

            //    }
            //}
            else if(domest==14)
            {
                if (textBox1.Text != "" && textBox2.Text != "" && checkBox1.Checked == true)
                {

                    searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOGOOGle14Days");

                    datagridvColor();
                }
                else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
                {

                    searchfordata(textBox1.Text, "", "serchFROMGOOGle14Days");
                    datagridvColor();
                }
                else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
                {

                    searchfordata("", textBox2.Text, "serchTOGOOGle14Days");
                    datagridvColor();

                }
                else if (textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
                {

                    searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOsky14Days");

                    datagridvColor();


                }
                else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
                {


                    searchfordata(textBox1.Text, "", "serchFROMsky14Days");
                    datagridvColor();



                }
                else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
                {

                    searchfordata("", textBox2.Text, "serchTOsky14Days");
                    datagridvColor();

                }
            }
            
        }

        public void somme(float a, float b, string str)
        {
            if (comboBox2.Text != "") { 
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
            d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;

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
            else { MessageBox.Show("combobox DAYS is empty"); }
        }
        string proc1, proc2, proc3, proc4, proc5, proc6, proc7, proc8, proc9, proc10, proc11, proc12;
       
          private  void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;
           
            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();

            int nbr;
           
            //if(domest==2)
            //        {
            //            proc1 = "priceGOOGL12Days";
            //            proc2 = "googlebig2Days";
            //            proc3 = "googlelos2Days";
            //            proc4 = "priceskysc12Days";
            //            proc5 = "skyscannerlow2Days";
            //            proc6 = "skyscannerbig2Days";
            //            proc7 = "btwnOlde_pricericeGF2Days";
            //            proc8 = "difgooglebig2Days";
            //            proc9 = "difgooglelos2Days";
            //            proc10 = "btwnoldpriceskys2Days";
            //            proc11 = "difskylos2Days";
            //            proc12 = "difskybig2Days";
                        
            //        }
            //   else if(domest==3)
            //        {
            //            proc1 = "priceGOOGL13Days";
            //            proc2 = "googlebig3Days";
            //            proc3 = "googlelos3Days";
            //            proc4 = "priceskysc13Days";
            //            proc5 = "skyscannerlow3Days";
            //            proc6 = "skyscannerbig3Days";
            //            proc7 = "btwnOlde_pricericeGF3Days";
            //            proc8 = "difgooglebig3Days";
            //            proc9 = "difgooglelos3Days";
            //            proc10 = "btwnoldpriceskys3Days";
            //            proc11 = "difskylos3Days";
            //            proc12 = "difskybig3Days";
                       
            //        }
            //    else if(domest==4)
            //        {
            //            proc1 = "priceGOOGL14Days";
            //            proc2 = "googlebig4Days";
            //            proc3 = "googlelos4Days";
            //            proc4 = "priceskysc14Days";
            //            proc5 = "skyscannerlow4Days";
            //            proc6 = "skyscannerbig4Days";
            //            proc7 = "btwnOlde_pricericeGF4Days";
            //            proc8 = "difgooglebig4Days";
            //            proc9 = "difgooglelos4Days";
            //            proc10 = "btwnoldpriceskys4Days";
            //            proc11 = "difskylos4Days";
            //            proc12 = "difskybig4Days";
                        
            //        }
             if (domest == 1)
            {
                        proc1 = "priceGOOGL1COPY";
                        proc2 = "googlebigCOPY";
                        proc3 = "googlelosCOPY";
                        proc4 = "priceskysc1COPY";
                        proc5 = "skyscannerlowCOPY";
                        proc6 = "skyscannerbigCOPY";
                        proc7 = "btwnOlde_pricericeGFCOPY";
                        proc8 = "difgooglebigCOPY";
                        proc9 = "difgooglelosCOPY";
                        proc10 = "btwnoldpriceskysCOPY";
                        proc11 = "difskylosCOPY";
                        proc12 = "difskybigCOPY";
                        
                    }
            //else if (domest == 14)
            //{
            //            proc1 = "priceGOOGL114Days";
            //            proc2 = "googlebig14Days";
            //            proc3 = "googlelos14Days";
            //            proc4 = "priceskysc114Days";
            //            proc5 = "skyscannerlow14Days";
            //            proc6 = "skyscannerbig14Days";
            //            proc7 = "btwnOlde_pricericeGF14Days";
            //            proc8 = "difgooglebig14Days";
            //            proc9 = "difgooglelos14Days";
            //            proc10 = "btwnoldpriceskys14Days";
            //            proc11 = "difskylos14Days";
            //            proc12 = "difskybig14Days";
                        

            //        }

            
            if (checkBox7.Checked==true && checkBox8.Checked==false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {
                       

                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text),proc1);
                        
                        datagridvColor();
                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text),99999, proc2);
                      
                        datagridvColor();
                    }
                    else if (radioButton2.Checked && minPrice.Text != "" )
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text), 99999, proc3);
                       

                        datagridvColor();
                    }
                }
                if (comboBox1.Text.Equals("skyscanner"))
                { 
                 if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {
                        dataGridView1.Rows.Clear();


                       
                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text),proc4);
                       

                        datagridvColor();


                    }
                    else if (radioButton2.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text), 99999,proc5);
                       

                        datagridvColor();

                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                       
                        somme(float.Parse(minPrice.Text), 99999,proc6);
                       

                        datagridvColor();
                    }

                }
            }
            if(checkBox8.Text.Equals("Difference price")&& checkBox7.Checked==false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked==true && minPrice.Text != "" && maxprice.Text != "")
                    {
                       
                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text),proc7);
                       

                        datagridvColor();
                    }
                    else if (radioButton1.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                       
                        somme(float.Parse(minPrice.Text), 99999,proc8);
                       

                        datagridvColor();
                    }
                    else if (radioButton2.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                       
                        somme(float.Parse(minPrice.Text), 99999,proc9);
                        

                        datagridvColor();
                    }
                }

                else if(comboBox1.Text.Equals("skyscanner"))
                {
                    if (radioButton3.Checked==true && minPrice.Text != "" && maxprice.Text != "")
                    {
                       
                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text),proc10);
                       
                        datagridvColor();
                    }
                    else if (radioButton2.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text), 99999,proc11);
                      

                        datagridvColor();

                    }
                    else if (radioButton1.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                       
                        somme(float.Parse(minPrice.Text), 99999, proc12);
                       
                        datagridvColor();
                    }

                }
            }
        }
        public void search4(string str)
        {
            if (comboBox2.Text != "")
            {
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = str;
                d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
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
            else { MessageBox.Show("combobox DAYS is empty"); }
        }
        public void searchAll()
        {
            if (comboBox2.Text != "")
            {
                if(checkBox7.Checked == true && checkBox8.Checked == false)
                {
                    if (radioButton1.Checked == true && radioButton2.Checked == false && radioButton3.Checked == false)
                    {
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "searchGFDomesticAllPriceGreater";
                        d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = textBox1.Text.ToString();
                        d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = textBox2.Text.ToString();
                        d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                        d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                        d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                        d.cmdd.Connection = d.cn;
                    }
                    else if (radioButton1.Checked == false && radioButton2.Checked == true && radioButton3.Checked == false)
                    {
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "searchGFDomesticAllPriceLesser";
                        d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = textBox1.Text.ToString();
                        d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = textBox2.Text.ToString();
                        d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                        d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                        d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                        d.cmdd.Connection = d.cn;
                    }
                    else if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == true)
                    {
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "searchGFDomesticAllPriceBetween";
                        d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = textBox1.Text.ToString();
                        d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = textBox2.Text.ToString();
                        d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                        d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                        d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);
                        d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                        d.cmdd.Connection = d.cn;
                    }
                }
                else
                {
                    if (radioButton1.Checked == true && radioButton2.Checked == false && radioButton3.Checked == false)
                    {
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "searchGFDomesticAllDifferenceGreater";
                        d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = textBox1.Text.ToString();
                        d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = textBox2.Text.ToString();
                        d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                        d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                        d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                        d.cmdd.Connection = d.cn;
                    }
                    else if (radioButton1.Checked == false && radioButton2.Checked == true && radioButton3.Checked == false)
                    {
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "searchGFDomesticAllDifferenceLesser";
                        d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = textBox1.Text.ToString();
                        d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = textBox2.Text.ToString();
                        d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                        d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                        d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                        d.cmdd.Connection = d.cn;
                    }
                    else if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == true)
                    {
                        d.dt.Rows.Clear();
                        d.cmdd.Parameters.Clear();
                        d.cmdd.CommandType = CommandType.StoredProcedure;
                        d.cmdd.CommandText = "searchGFDomesticAllDifferenceBetween";
                        d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = textBox1.Text.ToString();
                        d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = textBox2.Text.ToString();
                        d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                        d.cmdd.Parameters.Add("@Airline", SqlDbType.VarChar, 100).Value = textBox4.Text.ToString();
                        d.cmdd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = textBox3.Text.ToString();
                        d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                        d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                        d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);
                        d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                        d.cmdd.Connection = d.cn;
                    }
                }
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
                datagridvColor();
            }
            else { MessageBox.Show("combobox DAYS is empty"); }
        }
        public void dates(string str)
        {
            if (comboBox2.Text != "") {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
            d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
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
            else { MessageBox.Show("combobox DAYS is empty"); }
        }
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
        public void datesPrice(string str)
        {
            if (comboBox2.Text != "")
            {
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = str;
                d.cmdd.Parameters.Add("@dateA", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@dateB", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
                d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value = minP;
                d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value = maxP;
                d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
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
            else { MessageBox.Show("combobox DAYS is empty"); }
        }
        double maxP, minP;
        private  void button3_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;

            dataGridView1.Rows.Clear();
            if(min.Text == "" && max.Text == "")
            {
                if (domest == 1)
                    dates("serchGGl1COPY");

                else if (domest == 14)
                    dates("serchGGl114Days");
            }
            else
            {
                myfunction();
                datesPrice("searchDatePriceCOPY");
            }
            
                datagridvColor();
            min.Text = "";
            max.Text = "";
        }

       

        private void groupBox2_Enter(object sender, EventArgs e)
        {

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
            dataGridView1.Rows.Clear();
        }

        


      
       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
           
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
           
        }

       
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

           
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string str = "2";
            Information_about_files inf = new Information_about_files(str);
            inf.ShowDialog();
        }

        private async void button4_Click_1(object sender, EventArgs e)
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

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void pricewithfrom_to(string frm,string to,float price1,float price2,string nameproce)
        {
            
            if (comboBox2.Text != "") { 
            dataGridView1.Rows.Clear();
            if (d.dt.Rows.Count != 0)
            {
                d.dt.Rows.Clear();
            }
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = nameproce;
            if((frm!="" || to!="") && price1!=99999 && price2!=99999)
            {
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);
            }
            else if((frm != "" || to != "") && price1 != 99999 && price2 == 99999)
            {
                  
                    d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 50).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 50).Value = textBox2.Text;
                d.cmdd.Parameters.Add("@price", SqlDbType.Float).Value =price1;
            }
            d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;

            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

             cnt = d.dt.Rows.Count;

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
            else { MessageBox.Show("combobox DAYS is empty"); }
        }

        string procPrix1, procPrix2, procPrix3, procPrix4, procPrix5, procPrix6;
        private void button7_Click(object sender, EventArgs e)
        {
         if(domest==1)
            {
                procPrix1 = "serchFromTopriceGOOGlebigCOPY";
                procPrix2 = "serchFromTopriceGOOGleCOPY";
                procPrix3 = "serchFromTopriceGOOGlebetweenCOPY";
                procPrix4 = "serchFromTopriceskysbigCOPY";
                procPrix5 = "serchFromTopriceskyCOPY";
                procPrix6 = "serchFromTopriceskysbetweenCOPY";
            }
         
         else if(domest==14)
            {
                procPrix1 = "serchFromTopriceGOOGlebig14Days";
                procPrix2 = "serchFromTopriceGOOGle14Days";
                procPrix3 = "serchFromTopriceGOOGlebetween14Days";
                procPrix4 = "serchFromTopriceskysbig14Days";
                procPrix5 = "serchFromTopricesky14Days";
                procPrix6 = "serchFromTopriceskysbetween14Days";
            }
            if (checkBox1.Checked == true && checkBox2.Checked == false && comboBox1.Text.Equals("google"))
            {
               
                if (textBox1.Text != "" || textBox2.Text != "")
                {
                    if (checkBox7.Checked==true && checkBox8.Checked==false)
                    {
                        
                        if (radioButton1.Checked==true && minPrice.Text!="")
                        {
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999,procPrix1);
                            

                            datagridvColor();
                        }
                        else if(radioButton2.Checked == true && minPrice.Text != "")
                        {
                           
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999,procPrix2);
                           
                            datagridvColor();
                        }
                        else if(radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text!="")
                        {
                           
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text),procPrix3);
                            

                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field "); }
                    }
                   
                    else if (checkBox7.Checked == false && checkBox8.Checked == true)
                    {
                        MessageBox.Show("You can only use the new price");
                    }
                }
                
            }
            else if(checkBox1.Checked == false && checkBox2.Checked == true && comboBox1.Text.Equals("skyscanner"))
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (checkBox7.Checked == true && checkBox8.Checked == false)
                    {
                        if (radioButton1.Checked == true && minPrice.Text != "")
                        {
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, procPrix4);

                           

                            datagridvColor();
                        }
                        else if (radioButton2.Checked == true && minPrice.Text != "")
                        {
                          
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, procPrix5);
                            

                            datagridvColor();
                        }
                       
                        else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                        {
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text),procPrix6);

                           

                            datagridvColor();
                        }
                        else { MessageBox.Show("You must fill in the blank field "); }
                    }
                    else if(checkBox7.Checked==false && checkBox8.Checked==true)
                    {
                        MessageBox.Show("You can only use the new price");
                    }
                }
                else { MessageBox.Show("You must fill in the blank field FROM and TO"); }
            }
            //textBox1.Text = "";
            //textBox2.Text = "";
            //minPrice.Text = "";
            //maxprice.Text = "";
        }

       

        private void button9_Click(object sender, EventArgs e)
        {
            google_copy ggl = new google_copy(domest);
            ggl.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SkysCopy SK = new SkysCopy(domest);
             SK.Show();
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

        private void buttonSrch4_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;

            dataGridView1.Rows.Clear();

            search4("searchGFDomestic4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;

            dataGridView1.Rows.Clear();

            searchAll();
        }

        private void FromToDates(string adrss, string from, string to, string fromdate, string todate)
        {
            if (comboBox2.Text != "")
            {
                dataGridView1.Rows.Clear();
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = adrss;
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 20).Value = from;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 20).Value = to;
                d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = fromdate;
                d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = todate;
                d.cmdd.Parameters.Add("@day", SqlDbType.VarChar, 20).Value = comboBox2.Text;
                d.cmdd.Parameters.Add("@shortstays", SqlDbType.Int).Value = checkBoxST.Checked ? 1 : 0;
                d.cmdd.Connection = d.cn;

                d.dt.Load(d.cmdd.ExecuteReader());

                cnt = d.dt.Rows.Count;


                if (checkBox1.Checked == true)
                {






                    if (cnt == 0)
                    {
                        MessageBox.Show("The information entered is not on the database!");
                    }
                    else
                    {

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
                        datagridvColor();
                    }



                }
            }
            else { MessageBox.Show("combobox DAYS is empty"); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (date1.Value < date2.Value)
            {
                //if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleCOPY", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("search1With3", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("search1With3", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("search1With3", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
            }
            else
            {
                if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("search1With3", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("search1With3", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("search1With3", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Stops = '" + comboBox3.SelectedValue.ToString() + "'", "New_price desc");
            foreach (DataRow dr in ligne)
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), DateTime.Parse(dr[15].ToString()), dr[13].ToString());
            }
            datagridvColor();
            ligne = null;
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            button8.Visible = true;
            button9.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
            if (GF.Checked == true && SKYS.Checked == false) { 
            list = await GetPagedListAsync();
            button8.Enabled = list.HasPreviousPage;
            button9.Enabled = list.HasNextPage;
            dataGridView2.DataSource = list.ToList();
            label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
            color();
                deleteclmn();
            }
            if (SKYS.Checked == true && GF.Checked == false)
            {
                list1 = await GetPagedListAsync1();
                button8.Enabled = list1.HasPreviousPage;
                button9.Enabled = list1.HasNextPage;
                dataGridView2.DataSource = list1.ToList();
                label6.Text = string.Format("page {0}/{1}", pagenumber, list1.PageCount);
                color();
                deleteclmn();
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            if (list.HasNextPage)
            {
                if (GF.Checked == true)
                {
                    list = await GetPagedListAsync(++pagenumber);
                    button8.Enabled = list.HasPreviousPage;
                    button9.Enabled = list.HasNextPage;
                    dataGridView2.DataSource = list.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                    color();
                    deleteclmn();
                }
                if (SKYS.Checked == true)
                {
                    list1 = await GetPagedListAsync1(++pagenumber);
                    button8.Enabled = list1.HasPreviousPage;
                    button9.Enabled = list1.HasNextPage;
                    dataGridView2.DataSource = list1.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list1.PageCount);
                    color();
                    deleteclmn();
                }
            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (list.HasPreviousPage)
            {
                if (GF.Checked == true)
                {
                    list = await GetPagedListAsync(--pagenumber);
                    button8.Enabled = list.HasPreviousPage;
                    button9.Enabled = list.HasNextPage;
                    dataGridView2.DataSource = list.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                    color();
                    deleteclmn();
                }
                if (SKYS.Checked == true)
                {
                    list1 = await GetPagedListAsync1(--pagenumber);
                    button8.Enabled = list1.HasPreviousPage;
                    button9.Enabled = list1.HasNextPage;
                    dataGridView2.DataSource = list1.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list1.PageCount);
                    color();
                    deleteclmn();
                }
            }
        }

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

        private void GF_CheckedChanged(object sender, EventArgs e)
        {
            SKYS.Checked = false;
        }

        private void SKYS_CheckedChanged(object sender, EventArgs e)
        {
            GF.Checked = false;
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
        
   

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DataRow[] ligne;
            dataGridView1.Rows.Clear();
            ligne = d.dt.Select("Olde_price = 0 and New_price > 0", "New_price desc");
            foreach (DataRow dr in ligne)
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(),dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), DateTime.Parse(dr[15].ToString()), dr[13].ToString());
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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString(), d.dt.Rows[i][12].ToString(), DateTime.Parse(d.dt.Rows[i][15].ToString()), d.dt.Rows[i][13].ToString());
            }

            datagridvColor();
            radioButton5.Checked = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
