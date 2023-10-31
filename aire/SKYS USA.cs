using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace aire
{
    public partial class SKYS_USA : Form
    {
        OleDbConnection con;

        DataTable dt;
        DataTable dt1;
        ado d = new ado();
        private readonly SynchronizationContext synchronizationcontext;
        public SKYS_USA()
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;

        }
        int bb = 0;
        private void SKYS_USA_Load(object sender, EventArgs e)
        {
            button3.Visible = true;
            label4.Visible = false;
            textBox2.Visible = false;
            label2.Visible = false;
            button2.Visible = false;

            d.connecter();
            d.dt.Rows.Clear();

            int count;
            d.da = new SqlDataAdapter("select * from namefilesSKYSCOPY1", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "SKY");
            d.dt = d.ds.Tables["SKY"];
            count = d.dt.Rows.Count;
            if (count > 0)
            {
                label5.Text = d.dt.Rows[count - 1][1].ToString();
                label6.Text = d.dt.Rows[count - 1][2].ToString();
            }
            label6.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label4.Visible = false;
            textBox2.Visible = false;
            label2.Visible = false;
            button2.Visible = false;
            label6.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label4.Visible = true;
            textBox2.Visible = true;
            label6.Visible = true;
            button2.Visible = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "ALL Files |*.*| Excel Files |*.xlsx";



            if (op.ShowDialog() == DialogResult.OK)
            {
                string constr = "PROVIDER= Microsoft.ACE.OLEDB.12.0; Data Source =" + op.FileName + ";Extended Properties='Excel 12.0;'";
                textBox1.Text = "";
                textBox1.Text = op.FileName.ToString();
                con = new OleDbConnection(constr);

                OleDbCommand cmd = new OleDbCommand("select * from [data$]", con);
                con.Open();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int cntr = 0;
                cntr = dt.Rows.Count;
                label3.Visible = true;
                int count;
                d.da = new SqlDataAdapter("select count(*) from skyspoinCOPY1", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "s");
                d.dt = d.ds.Tables["s"];
                count = d.dt.Rows.Count;

                if (count == 1)
                {

                    d.cmdd = new SqlCommand("exec insertskys2COPY1", d.cn);
                    d.cmdd.ExecuteNonQuery();

                }


                d.cmdd.CommandType = CommandType.Text;

                int b = 0;
                string sql = "";
                string sql1 = "", sql2 = "", sql3 = "", sql4 = "", sql5 = "", sql6 = "", sql7 = "", sql8 = "", sql9 = "", sql10 = "", sql11 = "", sql12 = "";

                int j = 1;
                await Task.Run(() => {

                    for (int i = 0; i < cntr; i++)
                    {
                        j++;
                        sql += "insert into skys1COPY1  values ('" + dt.Rows[i]["FROM"].ToString() + "','" + dt.Rows[i]["TO"].ToString() + "','" + Convert.ToDateTime(dt.Rows[i]["DATES"]).ToString("yyyy/MM/dd") + "','" + dt.Rows[i]["PRICE"].ToString() + "','" + dt.Rows[i]["CABIN"].ToString() + "','" + dt.Rows[i]["URL"].ToString() + "')";
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
                        else if (i == cntr - 1 && j < 10001)
                        {
                            sql12 = sql;
                            j = 1;
                            sql = "";
                        }
                    }

                });
                await Task.Run(() =>
                {

                    try
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
                        d.cmdd = new SqlCommand("delete  skyspoinCOPY1", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        if (radioButton1.Checked)
                        {
                            d.cmdd = new SqlCommand("update skys1COPY1 set Montant =(Montant*2)", d.cn);
                            d.cmdd.ExecuteNonQuery();

                        }
                        bb = 1;
                        if (radioButton2.Checked == true) { MessageBox.Show("Part one complete"); }

                    }
                    catch
                    {
                        d.cmdd = new SqlCommand("delete skys1COPY1", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd = new SqlCommand("insert into skyspoinCOPY1 values('4')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        MessageBox.Show("Error!! \n Try again with NEW 1");
                    }
                });


                label3.Visible = false;

            }

            if (radioButton1.Checked && bb == 1)
            {

                d.cmdd = new SqlCommand("exec CheapestSkyCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec cheapskyscannerCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                memoir();
                bb = 0;
                MessageBox.Show("Part one complete");
            }
        }
        private void memoir()
        {
            if (radioButton2.Checked)
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    string[] a, b;
                    int c, z;
                    a = Convert.ToString(textBox1.Text).Split('\\');
                    c = a.Length - 1;
                    string sqlA = a[c];
                    b = Convert.ToString(textBox2.Text).Split('\\');
                    z = b.Length - 1;
                    string sqlB = b[z];
                    d.cmdd.CommandType = CommandType.Text;
                    d.cmdd = new SqlCommand("insert into namefilesSKYSCOPY1 values('" + sqlA + "','" + sqlB + "')", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC DELETnamefilesSKYSCOPY1", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }
            }
            else if (radioButton1.Checked)
            {
                if (textBox1.Text != "")
                {
                    string[] a;
                    int c;
                    a = Convert.ToString(textBox1.Text).Split('\\');
                    c = a.Length - 1;
                    string sqlA = a[c];

                    d.cmdd.CommandType = CommandType.Text;
                    d.cmdd = new SqlCommand("insert into namefilesSKYSCOPY1 values('" + sqlA + "','" + sqlA + "')", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC DELETnamefilesSKYSCOPY1", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;
            OpenFileDialog op = new OpenFileDialog();

            op.Filter = "ALL Files |*.*| Excel Files |*.xlsx";

            if (op.ShowDialog() == DialogResult.OK)
            {
                string constr = "PROVIDER= Microsoft.ACE.OLEDB.12.0; Data Source =" + op.FileName + ";Extended Properties='Excel 12.0;'";
                textBox2.Text = "";
                textBox2.Text = op.FileName.ToString();
                con = new OleDbConnection(constr);

                OleDbCommand cmd = new OleDbCommand("select * from [data$]", con);
                con.Open();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int cntr = 0;
                cntr = dt.Rows.Count;
                d.cmdd.CommandType = CommandType.Text;
                label2.Visible = true;
                int b = 0;
                string sql = "";
                string sql1 = "", sql2 = "", sql3 = "", sql4 = "", sql5 = "", sql6 = "", sql7 = "", sql8 = "", sql9 = "", sql10 = "", sql11 = "", sql12 = "";

                int j = 1;
                await Task.Run(() => {


                    for (int i = 0; i < cntr; i++)
                    {

                        j++;
                        sql += "insert into skys1COPY1  values ('" + dt.Rows[i]["FROM"].ToString() + "','" + dt.Rows[i]["TO"].ToString() + "','" + Convert.ToDateTime(dt.Rows[i]["DATES"]).ToString("yyyy/MM/dd") + "','" + dt.Rows[i]["PRICE"].ToString() + "','" + dt.Rows[i]["CABIN"].ToString() + "','" + dt.Rows[i]["URL"].ToString() + "')";
                        if (j == 10000)
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
                        else if (i == cntr - 1 && j < 10000)
                        {
                            sql12 = sql;
                            j = 1;
                            sql = "";
                        }
                    }
                });

                await Task.Run(() =>
                {
                    try
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
                        d.cmdd = new SqlCommand("update skys1COPY1 set Montant =(Montant*2)", d.cn);
                        d.cmdd.ExecuteNonQuery();

                        bb = 1;


                    }
                    catch
                    {
                        d.cmdd = new SqlCommand("delete skys1COPY1", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd = new SqlCommand("insert into skyspoinCOPY1 values('4')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        MessageBox.Show("Error!! \n Try again with NEW 1");
                    }
                });


                label2.Visible = false;
            }
            if (bb == 1 && radioButton2.Checked == true)
            {
                d.cmdd = new SqlCommand("exec CheapestSkyCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec cheapskyscannerCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                memoir();
                MessageBox.Show("The second part is complete");

                bb = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            d.cmdd.CommandType = CommandType.Text;
            if (textBox1.Text != "")
            {
                d.cmdd = new SqlCommand("exec deltTablXCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec any5COPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec cheapskyscannerCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec addnewcheskyCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec doblerowschskyCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete comprskyCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec cmprSCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec doblerowsskyCOPY1", d.cn);
                d.cmdd.ExecuteNonQuery();
            }

            d.cmdd = new SqlCommand("exec add_G_SCOPY1", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec upd_g_sCOPY1", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec upd_cmprgoogleCOPY1", d.cn);
            d.cmdd.ExecuteNonQuery();
            dt = null;
            d.dt = null;
        }
    }
}
