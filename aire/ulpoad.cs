using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace aire
{
    public partial class ulpoad : Form
    {
        public ulpoad()
        {
            InitializeComponent();
        }
        ado d = new ado();
        private void ulpoad_Load(object sender, EventArgs e)
        {
            d.connecter();
            d.da = new SqlDataAdapter("select * from namefilesGF", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "GF");
            d.dt = d.ds.Tables["GF"];

                radioButton1.Text = d.dt.Rows[0][1].ToString();
                radioButton2.Text = d.dt.Rows[1][1].ToString();
            label2.Visible = false;
        }
        DataTable dt = new DataTable();
        OleDbConnection con;
        private async void button1_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;
            button2.Enabled = false;
            OpenFileDialog ope = new OpenFileDialog();

            ope.Filter = "ALL Files |*.*| Excel Files |*.xlsx";

            if (ope.ShowDialog() == DialogResult.OK)
            {
                string constr = "PROVIDER= Microsoft.ACE.OLEDB.12.0; Data Source =" + ope.FileName + ";Extended Properties='Excel 12.0;'";
                textBox1.Text = "";
                textBox1.Text = ope.FileName.ToString();

                con = new OleDbConnection(constr);
                int cnt;

                OleDbCommand cmd = new OleDbCommand("select * from [Flight$]", con);
                con.Open();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                cnt = dt.Rows.Count;


                cnt = dt.Rows.Count;

                
              
                 
              
                d.cmdd = new SqlCommand("delete from interim", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd.CommandType = CommandType.Text;
                int j = 1;
                int b = 0;

                string sql = "";
                string sql1 = "", sql2 = "", sql3 = "", sql4 = "", sql5 = "", sql6 = "", sql7 = "", sql8 = "", sql9 = "", sql10 = "", sql11 = "", sql12 = "";
                label2.Visible = true;
                await Task.Run(() =>
                {

                    for (int i = 0; i < cnt; i++)
                    {
                        j++;
                        sql += " insert into interim values ('" + dt.Rows[i]["FROM"].ToString() + "','"
                    + dt.Rows[i]["TO"].ToString() + "','"
                    + Convert.ToDateTime(dt.Rows[i]["DATES"]).ToString("yyyy/MM/dd") + "','"
                    + dt.Rows[i]["PRICE"].ToString() + "','" + dt.Rows[i]["CABIN"].ToString() + "')";
                        if (j == 10001)
                        {
                            b++;
                            if (b == 1) { sql1 = sql; }
                            else if (b == 2) { sql2 = sql; }
                            else if (b == 3) { sql3 = sql; }
                            else if (b == 4) { sql4 = sql; }
                            else if (b == 5) { sql5 = sql; }
                            else if (b == 6) { sql6 = sql; }
                            else if (b == 7) { sql7 = sql; }
                            else if (b == 8) { sql8 = sql; }
                            else if (b == 9) { sql9 = sql; }
                            else if (b == 10) { sql10 = sql; }
                            else if (b == 11) { sql11 = sql; }
                            j = 1;
                            sql = "";
                        }
                        else if (i == cnt - 1 && j < 10001)
                        {
                            sql12 = sql;
                            j = 1;
                            sql = "";
                        }
                    }

                });
                await Task.Run(() =>
                {

                    sql1 += " " + sql2;

                    if (sql1 != "")
                    {
                        d.cmdd = new SqlCommand(sql1, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }

                    sql3 += " " + sql4;
                    if (sql3 != "")
                    {
                        d.cmdd = new SqlCommand(sql3, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }

                    sql5 += " " + sql6;
                    if (sql5 != "")
                    {
                        d.cmdd = new SqlCommand(sql5, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    sql7 += " " + sql8;
                    if (sql7 != "")
                    {
                        d.cmdd = new SqlCommand(sql7, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }


                    if (sql9 != "")
                    {
                        d.cmdd = new SqlCommand(sql9, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }


                    if (sql10 != "")
                    {
                        d.cmdd = new SqlCommand(sql10, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    if (sql11 != "")
                    {
                        d.cmdd = new SqlCommand(sql11, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                    if (sql12 != "")
                    {
                        d.cmdd = new SqlCommand(sql12, d.cn);
                        d.cmdd.ExecuteNonQuery();
                    }
                });

           
            }

            d.cmdd = new SqlCommand("update interim set Montant='0' where (Montant='')", d.cn);
            d.cmdd.ExecuteNonQuery();
            label2.Visible = false;
            button2.Enabled= true;
            MessageBox.Show("complete");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked || radioButton2.Checked)
            {

                if(radioButton1.Checked)
                {

                    d.cmdd = new SqlCommand("delete interimcmpr", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("exec cmprinterim", d.cn);
                    d.cmdd.ExecuteNonQuery();

                    d.cmdd = new SqlCommand("update interimcmpr set Difference=(New_Price-Old_Price) where (Difference=0 AND Old_Price>0)", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("exec doblerowsinterim", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }
                else if(radioButton2.Checked)
                {
                    d.cmdd = new SqlCommand("delete interimcmpr", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("exec cmprinterimold", d.cn);
                    d.cmdd.ExecuteNonQuery();

                    d.cmdd = new SqlCommand("update interimcmpr set Difference=(New_Price-Old_Price) where (Difference=0 AND Old_Price>0)", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("exec doblerowsinterim", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }

            }
            else
            {
                MessageBox.Show("Please choose one of the two files above");
            }
        }
    }
}
