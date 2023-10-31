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
    public partial class search_skys_flight : Form
    {
        ado d = new ado();
        private readonly SynchronizationContext synchronizationcontext;
        string cabin;
        public search_skys_flight(string cbn)
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;
            cabin = cbn;
        }

      

        public int cnt = 0,b=0,c=0;
        public void searchfordata(string frm,string to,string nameProc)
        {
            d.dt.Rows.Clear();

            d.dt.Clear();
            d.dt.Columns.Clear();
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
            d.cmdd.Connection = d.cn;

            b =b+ 1;
            if (checkBox2.Checked == true)
            {
                if (b==1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }
              

                d.dt.Load(d.cmdd.ExecuteReader());

                cnt = d.dt.Rows.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if(checkBox1.Checked==true)
            {
                if(c==1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }
               

                d.dt.Load(d.cmdd.ExecuteReader());

                cnt = d.dt.Rows.Count;
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }
        }
        string cbnB1,cbnB2,cbnB3;
        private  void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;
         
            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if(cabin=="Business")
            {
                cbnB1 = "serchFromToGOOGleBusiness";
                cbnB2 = "serchFROMGOOGleBusiness";
                cbnB3 = "serchTOGOOGleBusiness";
            }
            else if(cabin== "Premium")
            {
                cbnB1 = "serchFromToGOOGlePremium";
                cbnB2 = "serchFROMGOOGlePremium";
                cbnB3 = "serchTOGOOGlePremium";
            }
            else if(cabin== "Economy")
            {
                cbnB1 = "serchFROMTOGOOGle";
                cbnB2 = "serchFROMGOOGle";
                cbnB3 = "serchTOGOOGle";
            }
            if (textBox1.Text!="" && textBox2.Text!=""  && checkBox1.Checked==true)
            {
               
                searchfordata(textBox1.Text,textBox2.Text,cbnB1);

                datagridvColor();
            }
            else if (textBox1.Text != "" && textBox2.Text == "" && checkBox1.Checked == true)
            {
                
                searchfordata(textBox1.Text,"", cbnB2);
                datagridvColor();
            }
           else if (textBox1.Text == "" && textBox2.Text != "" && checkBox1.Checked == true)
            {
                
                searchfordata("",textBox2.Text, cbnB3);
                datagridvColor();

            }
            else if(textBox1.Text != "" && textBox2.Text != "" && checkBox2.Checked == true)
            {

                searchfordata(textBox1.Text, textBox2.Text, "serchFROMTOsky");
                
                datagridvColor();


            }
            else if (textBox1.Text != "" && textBox2.Text == "" && checkBox2.Checked == true)
            {


                searchfordata(textBox1.Text, "", "serchFROMsky");
                datagridvColor();



            }
            else if (textBox1.Text == "" && textBox2.Text != "" && checkBox2.Checked == true)
            {


                searchfordata("", textBox2.Text, "serchTOsky");
                datagridvColor();



            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
        private void comb(string str)
        {
            d.ds.Clear();
           
            d.da = new SqlDataAdapter("select distinct " + stp + " from " + str+"", d.cn);
            d.da.Fill(d.ds, ""+str+"comSTOPS");
           
            comboBox2.DataSource = d.ds.Tables[""+str+"comSTOPS"];
            comboBox2.DisplayMember = ""+stp+"";
            comboBox2.ValueMember = "" + stp + "";

        }

        private void search_skys_flight_Load(object sender, EventArgs e)
        {
            checkBox9.Visible = false;
            checkBox10.Visible = false;
            d.connecter();
            label5.Visible = false;
            comboBox1.Items.Add("google");
            if (cabin == "Economy")
            { comboBox1.Items.Add("skyscanner"); }

            dataGridView2.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.Columns.RemoveAt(10);

            dshtl.Clear();
            dthtl.Rows.Clear();
            d.da = new SqlDataAdapter("select DISTINCT code from hotel", d.cn);
            d.da.Fill(dshtl, "code");
            dthtl = dshtl.Tables["code"];
           
            if(cabin== "Business" || cabin== "Premium")
            {
                comboBox1.Visible = true;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox9.Visible = false;
                checkBox10.Visible = false;
                checkBox5.Visible = false;
                button8.Visible = false;
                SKYS.Visible = false;
                stp = "Stops";
                switch (cabin)
                {

                    case "Business":comb("comprGOOGLBusiness");break;
                    case "Premium": comb("comprGOOGLPremium"); break;
                }
                checkBox1.Checked = true;
                
                checkBox4.Checked = true;
                checkBox6.Checked = true;
                GF.Checked = true;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            
        }
         
        public void somme(float a,float b,string str)
        {
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
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

             cnt = d.dt.Rows.Count;

            b = b + 1;
            if (checkBox2.Checked == true)
            {
                if (b == 1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }

                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if (checkBox1.Checked == true)
            {
                if (c == 1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }


               
                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }

        }
        string price1, price2, price3, price4, price5, price6;
        private  void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;
           
            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (cabin == "Business")
            {
                price1 = "priceGOOGL1Business";
                price2 = "googlebigBusiness";
                price3 = "googlelosBusiness";
                price4 = "btwnOlde_pricericeGFBusiness";
                price5 = "difgooglebigBusiness";
                price6 = "difgooglelosBusiness";
            }
            else if (cabin == "Premium")
            {
                price1 = "priceGOOGL1Premium";
                price2 = "googlebigPremium";
                price3 = "googlelosPremium";
                price4 = "btwnOlde_pricericeGFPremium";
                price5 = "difgooglebigPremium";
                price6 = "difgooglelosPremium";
            }
            else if (cabin == "Economy")
            {
                price1 = "priceGOOGL1";
                price2 = "googlebig";
                price3 = "googlelos";
                price4 = "btwnOlde_pricericeGF";
                price5 = "difgooglebig";
                price6 = "difgooglelos";

            }
            if (checkBox7.Checked==true && checkBox8.Checked==false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {

                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text),price1);
                        
                        datagridvColor();
                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text),99999, price2);
                      
                        datagridvColor();
                    }
                    else if (radioButton2.Checked && minPrice.Text != "" )
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text), 99999, price3);
                       

                        datagridvColor();
                    }
                }
                //end google new price
                if (comboBox1.Text.Equals("skyscanner"))
                { 
                    if (radioButton3.Checked && minPrice.Text != "" && maxprice.Text != "")
                    {
                        dataGridView1.Rows.Clear();


                       
                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "priceskysc1");
                       

                        datagridvColor();


                    }
                    else if (radioButton2.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text), 99999, "skyscannerlow");
                       

                        datagridvColor();

                    }
                    else if (radioButton1.Checked && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                       
                        somme(float.Parse(minPrice.Text), 99999, "skyscannerbig");
                       

                        datagridvColor();
                    }

                }
                // end skys new price
            }
            //end new price

            if(checkBox8.Text.Equals("Difference price")&& checkBox7.Checked==false)
            {
                if (comboBox1.Text.Equals("google"))
                {
                    if (radioButton3.Checked==true && minPrice.Text != "" && maxprice.Text != "")
                    {
                       
                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), price4);
                       

                        datagridvColor();
                    }
                    else if (radioButton1.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                       
                        somme(float.Parse(minPrice.Text), 99999, price5);
                       

                        datagridvColor();
                    }
                    else if (radioButton2.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();

                       
                        somme(float.Parse(minPrice.Text), 99999, price6);
                        

                        datagridvColor();
                    }
                }
                //end difference google
                else if(comboBox1.Text.Equals("skyscanner"))
                {
                    if (radioButton3.Checked==true && minPrice.Text != "" && maxprice.Text != "")
                    {
                       
                        somme(float.Parse(minPrice.Text), float.Parse(maxprice.Text), "btwnoldpriceskys");
                       
                        datagridvColor();
                    }
                    else if (radioButton2.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                        
                        somme(float.Parse(minPrice.Text), 99999, "difskylos");
                      

                        datagridvColor();

                    }
                    else if (radioButton1.Checked==true && minPrice.Text != "")
                    {
                        dataGridView1.Rows.Clear();
                       
                        somme(float.Parse(minPrice.Text), 99999, "difskybig");
                       
                        datagridvColor();
                    }

                }
                // end difference skys
            }
        }
        DataSet dshtl = new DataSet();
        DataTable dthtl = new DataTable();
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
        public void dates(string str)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@date1", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@date2", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");

            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

             cnt = d.dt.Rows.Count;
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            b = b + 1;
            if (checkBox5.Checked == true)
            {
                if (b == 1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }

               
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if (checkBox6.Checked == true)
            {
                if (c == 1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }
  
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }

        }
        public void datePrice(string str)
        {
           
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@dateA", SqlDbType.Date).Value = date1.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@dateB", SqlDbType.Date).Value = date2.Value.ToString("yyyy/MM/dd");
            d.cmdd.Parameters.Add("@min", SqlDbType.Float).Value =minP;
            d.cmdd.Parameters.Add("@max", SqlDbType.Float).Value =maxP;
           
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;
           
            if (cnt == 0)
            {
                MessageBox.Show("The information entered is not on the database!");
            }
            b = b + 1;
            if (checkBox5.Checked == true)
            {
                if (b == 1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }
                MessageBox.Show("C1");

                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if (checkBox6.Checked == true)
            {
               
                if (c == 1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }

                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }

        }
        string datecabin;
        double minP, maxP;
        private  void button3_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            dataGridView1.Visible = true;
           
            dataGridView2.Visible = false;
            if(min.Text=="" && max.Text=="")
            {
                if (cabin == "Business")
                {
                    datecabin = "serchGGl1Business";
                }
                else if (cabin == "Premium")
                {
                    datecabin = "serchGGl1Premium";
                }
                else if (cabin == "Economy")
                {
                    datecabin = "serchGGl1";
                }
                if (checkBox6.Checked == true && checkBox5.Checked == false)
                {
                    dataGridView1.Rows.Clear();


                    dates(datecabin);


                    datagridvColor();
                }
                else if (checkBox6.Checked == false && checkBox5.Checked == true)
                {
                    dataGridView1.Rows.Clear();
                    dates("serchskysc1");

                    datagridvColor();
                }
            }
            else
            {
                if (cabin == "Business")
                {
                    datecabin = "searchDatePriceBusiness";
                }
                else if (cabin == "Premium")
                {
                    datecabin = "searchDatePricePremium";
                }
                else if (cabin == "Economy")
                {
                   
                    datecabin = "searchDatePrice";
                }
                if (checkBox6.Checked == true && checkBox5.Checked == false)
                {
                    dataGridView1.Rows.Clear();
                   
                    myfunction();
                    datePrice(datecabin);


                    datagridvColor();
                }
                else if (checkBox6.Checked == false && checkBox5.Checked == true)
                {
                    dataGridView1.Rows.Clear();
                    myfunction();
                    datePrice("searchDatePricesky");

                    datagridvColor();
                }
            }
            min.Text = "";
            max.Text = "";
        }

       private void myfunction()
        {
            if(min.Text != "" && max.Text != "")
            {
                minP = double.Parse(min.Text);
                maxP=  double.Parse(max.Text);
            }
            else if(min.Text != "" && max.Text == "")
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns[10].DefaultCellStyle.SelectionForeColor = Color.Blue;
            dataGridView1.Columns[10].DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (e.RowIndex > -1)
            {
                String[] spearator = { "https://" };

                var val = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
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


      
       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            checkBox9.Visible = false;
            checkBox10.Visible = true;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox9.Visible = true;
            checkBox10.Visible = false;
        }

       
        

        private  void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cabingoogle(string str)
        {
           
        }
       
        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void chekGoogle_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void chekSkys_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void pricewithfrom_to(string frm,string to,float price1,float price2,string nameproce)
        {
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
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
                d.cmdd.Parameters.Add("@price2", SqlDbType.Float).Value = float.Parse(maxprice.Text);
            }
            else if((frm != "" || to != "") && price1 != 99999 && price2 == 99999)
            {
                d.cmdd.Parameters.Add("@From", SqlDbType.VarChar, 50).Value = frm;
                d.cmdd.Parameters.Add("@To", SqlDbType.VarChar, 50).Value = to;
                d.cmdd.Parameters.Add("@price1", SqlDbType.Float).Value = float.Parse(minPrice.Text);
            }
            

            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

             cnt = d.dt.Rows.Count;

            b = b + 1;
            if (checkBox2.Checked == true)
            {
                if (b == 1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }

                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if (checkBox1.Checked == true)
            {
                if (c == 1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }



                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                    double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }
        }
        string pricecabin1, pricecabin2, pricecabin3;
        private void button7_Click(object sender, EventArgs e)
        {
           
           
            
            if (checkBox1.Checked == true && checkBox2.Checked == false && comboBox1.Text.Equals("google"))
            {
                if (cabin == "Business")
                {
                    pricecabin1 = "serchFromTopriceGOOGlebigBusiness";
                    pricecabin2 = "serchFromTopriceGOOGleBusiness";
                    pricecabin3 = "serchFromTopriceGOOGlebetweenBusiness";
                }
                else if (cabin == "Premium")
                {
                    pricecabin1 = "serchFromTopriceGOOGlebigPremium";
                    pricecabin2 = "serchFromTopriceGOOGlePremium";
                    pricecabin3 = "serchFromTopriceGOOGlebetweenPremium";
                }
                else if (cabin == "Economy")
                {
                    pricecabin1 = "serchFromTopriceGOOGlebig";
                    pricecabin2 = "serchFromTopriceGOOGle";
                    pricecabin3 = "serchFromTopriceGOOGlebetween";
                }
                if (textBox1.Text != "" || textBox2.Text != "")
                {
                    if (checkBox7.Checked==true && checkBox8.Checked==false)
                    {
                        if(radioButton1.Checked==true && minPrice.Text!="")
                        {
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999,pricecabin1);
                            

                            datagridvColor();
                        }
                        else if(radioButton2.Checked == true && minPrice.Text != "")
                        {
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, pricecabin2);
                           
                            datagridvColor();
                        }
                        else if(radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text!="")
                        {
                           
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), pricecabin3);
                            

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
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopriceskysbig");

                           

                            datagridvColor();
                        }
                        else if (radioButton2.Checked == true && minPrice.Text != "")
                        {
                          
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), 99999, "serchFromTopricesky");
                            

                            datagridvColor();
                        }
                       
                        else if (radioButton3.Checked == true && minPrice.Text != "" && maxprice.Text != "")
                        {
                            
                            pricewithfrom_to(textBox1.Text, textBox2.Text, float.Parse(minPrice.Text), float.Parse(maxprice.Text), "serchFromTopriceskysbetween");

                           

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
            textBox1.Text = "";
            textBox2.Text = "";
            minPrice.Text = "";
            maxprice.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            interim it = new interim();
            it.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            googleF ggl = new googleF(cabin);
            ggl.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sky SK = new sky();
            SK.Show();
        }
        int pagenumber = 1;
        IPagedList<comprGOOGL> list;
        public async Task<IPagedList<comprGOOGL>> GetPagedListAsync(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (GFEntities12 db = new GFEntities12())
                {
                    return db.comprGOOGLs.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        IPagedList<comprGOOGLBusiness> listb;
        public async Task<IPagedList<comprGOOGLBusiness>> GetPagedListAsyncb(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (GFEntities18 db = new GFEntities18())
                {
                    return db.comprGOOGLBusinesses.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        IPagedList<comprGOOGLPremium> listp;
        public async Task<IPagedList<comprGOOGLPremium>> GetPagedListAsyncp(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (GFEntities18 db = new GFEntities18())
                {
                    return db.comprGOOGLPremiums.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
                }
            }
            );
        }
        IPagedList<comprsky> list1;
        public async Task<IPagedList<comprsky>> GetPagedListAsync1(int pageNumber = 1, int pageSize = 5000)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (SKYEntities db = new SKYEntities())
                {
                    return db.comprskies.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
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
        private async void button10_Click(object sender, EventArgs e)
        {
            button8.Visible = true;
            button9.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
            if (GF.Checked == true && SKYS.Checked == false) { 
                if(cabin== "Economy")
                {
                    list = await GetPagedListAsync();
                    button8.Enabled = list.HasPreviousPage;
                    button9.Enabled = list.HasNextPage;
                    dataGridView2.DataSource = list.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                    color();
                    deleteclmn();
                }
                else if(cabin== "Business")
                {
                    listb = await GetPagedListAsyncb();
                    button8.Enabled = listb.HasPreviousPage;
                    button9.Enabled = listb.HasNextPage;
                    dataGridView2.DataSource = listb.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, listb.PageCount);
                    color();
                    deleteclmn();
                }
                else if(cabin== "Premium")
                {
                    listp = await GetPagedListAsyncp();
                    button8.Enabled = listp.HasPreviousPage;
                    button9.Enabled = listp.HasNextPage;
                    dataGridView2.DataSource = listp.ToList();
                    label6.Text = string.Format("page {0}/{1}", pagenumber, listp.PageCount);
                    color();
                    deleteclmn();
                }
          
               
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
                    if(cabin== "Economy")
                    {
                        list = await GetPagedListAsync(--pagenumber);
                        button8.Enabled = list.HasPreviousPage;
                        button9.Enabled = list.HasNextPage;
                        dataGridView2.DataSource = list.ToList();
                        label6.Text = string.Format("page {0}/{1}", pagenumber, list.PageCount);
                        color();
                        deleteclmn();
                    }
                    else if(cabin== "Business")
                    {
                        listb = await GetPagedListAsyncb(--pagenumber);
                        button8.Enabled = listb.HasPreviousPage;
                        button9.Enabled = listb.HasNextPage;
                        dataGridView2.DataSource = listb.ToList();
                        label6.Text = string.Format("page {0}/{1}", pagenumber, listb.PageCount);
                        color();
                        deleteclmn();
                    }
                    else if(cabin== "Premium")
                    {
                        listp = await GetPagedListAsyncp(--pagenumber);
                        button8.Enabled = listp.HasPreviousPage;
                        button9.Enabled = listp.HasNextPage;
                        dataGridView2.DataSource = listp.ToList();
                        label6.Text = string.Format("page {0}/{1}", pagenumber, listp.PageCount);
                        color();
                        deleteclmn();
                    }
                   
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
        private void FromToDates(string adrss,string from,string to,string fromdate,string todate)
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
            d.cmdd.Connection = d.cn;

            d.dt.Load(d.cmdd.ExecuteReader());

            cnt = d.dt.Rows.Count;

            b = b + 1;
            if (checkBox2.Checked == true)
            {
                if (b == 1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }

                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if (checkBox1.Checked == true)
            {
                if (c == 1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }



                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            if(checkBox1.Checked && checkBox6.Checked)
            {
                if(cabin== "Economy")
                {
                    if (date1.Value<date2.Value)
                    {
                        if(textBox1.Text!="" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGle", textBox1.Text,textBox2.Text,date1.Value.ToString("yyyy/MM/dd"),date2.Value.ToString("yyyy/MM/dd"));}
                        else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGle","", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                        else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGle", textBox1.Text,"", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                    }
                    else
                    {
                        if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGle", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"),""); }
                        else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGle","", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                        else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGle", textBox1.Text,"", date1.Value.ToString("yyyy/MM/dd"), ""); }
                    }
                }
                else if(cabin== "Business")
                {
                    if (date1.Value < date2.Value)
                    {
                        if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleBusiness", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                        else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleBusiness", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                        else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGleBusiness", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                    }
                    else
                    {
                        if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleBusiness", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                        else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGleBusiness", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                        else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGleBusiness", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), ""); }
                    }
                }
                else if(cabin== "Premium")
                {
                    if (date1.Value < date2.Value)
                    {
                        if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGlePremium", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                        else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGlePremium", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                        else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGlePremium", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                    }
                    else
                    {
                        if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGlePremium", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                        else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesGOOGlePremium", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                        else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesGOOGlePremium", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), ""); }
                    }
                }
                datagridvColor();

            }
            else if(checkBox2.Checked && checkBox5.Checked)
            {
                if (date1.Value < date2.Value)
                {
                    if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesSky", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                    else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesSky", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                    else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesSky", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), date2.Value.ToString("yyyy/MM/dd")); }
                }
                else
                {
                    if (textBox1.Text != "" && textBox2.Text != "") { FromToDates("serchFromToDatesSky", textBox1.Text, textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                    else if (textBox1.Text == "" && textBox2.Text != "") { FromToDates("serchFromToDatesSky", "", textBox2.Text, date1.Value.ToString("yyyy/MM/dd"), ""); }
                    else if (textBox1.Text != "" && textBox2.Text == "") { FromToDates("serchFromToDatesSky", textBox1.Text, "", date1.Value.ToString("yyyy/MM/dd"), ""); }
                }
                datagridvColor();
            }
        }

        string adrssStops;
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Visible = false;
            checkBox10.Visible = true;

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Visible = true;
            checkBox10.Visible = false;
        }
        string stp;
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            
                checkBox9.Checked = false;
              
                if (cabin == "Economy" && checkBox10.Checked == true)
                {
                    adrssStops = "comprGOOGL";
                    stp = "Stops";
                    comb(adrssStops);
                }
            
            
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
           
               
                checkBox10.Checked = false;
                if (cabin == "Economy" && checkBox9.Checked == true)
                {
                    stp = "STOP";
                    adrssStops = "comprsky";
                    comb(adrssStops);
                }
            
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            

            
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
          

            DataRow[] ligne;
            dataGridView1.Rows.Clear();

           
            if (adrssStops == "comprsky")
            {
              
                ligne = d.dt.Select("STOP = '" + comboBox2.SelectedValue.ToString() + "'", "New_price desc");
                foreach (DataRow dr in ligne)
                {
                    dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                    double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(), dr[10].ToString());
                }
            }
            else
            {
               
                ligne = d.dt.Select("Stops = '" + comboBox2.SelectedValue.ToString() + "'", "New_price desc");
                foreach (DataRow dr in ligne)
                {
                    dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), DateTime.Parse(dr[3].ToString()),
                    double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(), dr[10].ToString(), dr[11].ToString());
                }
            }
            datagridvColor();
            ligne = null;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
           
            int cntDt = d.dt.Rows.Count;
            if (checkBox11.Checked)
            {
                dataGridView1.Rows.Clear();
                if (adrssStops == "comprsky")
                {
                    
                    for (int i = 0; i < cntDt; i++)
                    {
                        dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                        double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                    }
                }
                else
                {
                    
                    for (int i = 0; i < cntDt; i++)
                    {
                        dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                             double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                    }

                }
            }
            datagridvColor();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Visible = true;
            checkBox10.Visible = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Visible = false;
            checkBox10.Visible = true;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(comboBox1.Text=="google")
            {
                checkBox9.Visible = false;
                checkBox10.Visible = true;
            }
            else
            {
                checkBox9.Visible = true;
                checkBox10.Visible = false;
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            string str = "1";
            Information_about_files inf = new Information_about_files(str);
            inf.ShowDialog();
        }

        private void comboBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Columns[10].DefaultCellStyle.SelectionForeColor = Color.Blue;
            dataGridView2.Columns[10].DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView2.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
        private void citygoogle(string str)
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = str;
            d.cmdd.Parameters.Add("@city", SqlDbType.VarChar, 20).Value = textBox4.Text;
            d.cmdd.Connection = d.cn;
            
            d.dt.Load(d.cmdd.ExecuteReader());
           
             cnt =d.dt.Rows.Count;

            b = b + 1;
            if (checkBox2.Checked == true)
            {
                if (b == 1)
                {
                    //dataGridView1.Columns.RemoveAt(9);
                    c = 1;
                }

                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
                }
            }
            else if (checkBox1.Checked == true)
            {
                if (c == 1)
                {
                    DataGridViewTextBoxColumn cll = new DataGridViewTextBoxColumn();
                    cll.HeaderText = "Days";
                    dataGridView1.Columns.Insert(9, cll);
                    c = 0;
                }



                if (cnt == 0)
                {
                    MessageBox.Show("The information entered is not on the database!");
                }
                for (int i = 0; i < cnt; i++)
                {

                    dataGridView1.Rows.Add(d.dt.Rows[i][0].ToString(), d.dt.Rows[i][1].ToString(), d.dt.Rows[i][2].ToString(), DateTime.Parse(d.dt.Rows[i][3].ToString()),
                         double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString(), d.dt.Rows[i][11].ToString());
                }
                b = 0;

            }
        }

        string citysC;
        private void button13_Click(object sender, EventArgs e)
        {
            if(cabin== "Business")
            {
                citysC = "citysGFBusiness";
            }
            else if(cabin== "Premium")
            {
                citysC = "citysGFPremium";
            }
            else if(cabin== "Economy")
            {
                citysC = "citysGF";
            }
            label6.Text = "";
            dataGridView1.Visible = true;

            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            if (checkBox4.Checked == true && checkBox3.Checked == false)
            {
                citygoogle(citysC);
                datagridvColor();
            }
            else if (checkBox3.Checked == true && checkBox4.Checked == false)
            {
                citygoogle("cityssky");
                datagridvColor();
            }
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
                double.Parse(dr[4].ToString()), double.Parse(dr[5].ToString()), double.Parse(dr[6].ToString()), double.Parse(dr[7].ToString()), dr[8].ToString(), dr[9].ToString(),dr[10].ToString());
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
                     double.Parse(d.dt.Rows[i][4].ToString()), double.Parse(d.dt.Rows[i][5].ToString()), double.Parse(d.dt.Rows[i][6].ToString()), double.Parse(d.dt.Rows[i][7].ToString()), d.dt.Rows[i][8].ToString(), d.dt.Rows[i][9].ToString(), d.dt.Rows[i][10].ToString());
            }

            datagridvColor();
            radioButton5.Checked = false;
        }
    }
}
