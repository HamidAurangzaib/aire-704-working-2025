using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace aire
{
    public partial class SkysCopy : Form
    {
        OleDbConnection con;

        DataTable dt;
        DataTable dt1;
        ado d = new ado();
        private readonly SynchronizationContext synchronizationcontext;
        int domest;
        public SkysCopy(int domestic)
        {
            InitializeComponent();
            synchronizationcontext = SynchronizationContext.Current;
            domest = domestic;
        }
        int bb = 0;
        private void SkysCopy_Load(object sender, EventArgs e)
        {
            button3.Visible = true;
            label4.Visible = false;
            textBox2.Visible = false;
            label2.Visible = false;
            button2.Visible = false;

            d.connecter();
            d.dt.Rows.Clear();

            int count;

            if (domest == 7)
            {
                d.da = new SqlDataAdapter("select * from namefilesSKYSCOPY", d.cn);
                d.ds = new DataSet();
            }
            else if(domest==2)
            {
                d.da = new SqlDataAdapter("select * from namefilesSKYS2Days", d.cn);
                d.ds = new DataSet();
            }
            else if(domest==3)
            {
                d.da = new SqlDataAdapter("select * from namefilesSKYS3Days", d.cn);
                d.ds = new DataSet();
            }
            else if(domest==4)
            {
                d.da = new SqlDataAdapter("select * from namefilesSKYS4Days", d.cn);
                d.ds = new DataSet();
            }
            else if(domest==14)
            {
                d.da = new SqlDataAdapter("select * from namefilesSKYS14Days", d.cn);
                d.ds = new DataSet();
            }

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
        string proc1, proc2, proc3, proc4,proc5;
        private async void button1_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "ALL Files |*.*| Excel Files |*.xlsx";

            if (domest == 7)
            {
                proc1 = "skyspoinCOPY";
                proc2 = "insertskys2COPY";
                proc3 = "skys1COPY";
                proc4 = "CheapestSkyCOPY";
                proc5 = "cheapskyscannerCOPY";
            }
            else if (domest == 2)
            {
                proc1 = "skyspoin2Days";
                proc2 = "insertskys22Days";
                proc3 = "skys12Days";
                proc4 = "CheapestSky2Days";
                proc5 = "cheapskyscanner2Days";
            }
            else if (domest == 3)
            {
                proc1 = "skyspoin3Days";
                proc2 = "insertskys23Days";
                proc3 = "skys13Days";
                proc4 = "CheapestSky3Days";
                proc5 = "cheapskyscanner3Days";
            }
            else if (domest == 4)
            {
                proc1 = "skyspoin4Days";
                proc2 = "insertskys24Days";
                proc3 = "skys14Days";
                proc4 = "CheapestSky4Days";
                proc5 = "cheapskyscanner4Days";
            }
            else if (domest == 14)
            {
                proc1 = "skyspoin14Days";
                proc2 = "insertskys214Days";
                proc3 = "skys114Days";
                proc4 = "CheapestSky14Days";
                proc5 = "cheapskyscanner14Days";
            }

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
                d.da = new SqlDataAdapter("select count(*) from "+proc1+"", d.cn);
                d.ds = new DataSet();

                d.da.Fill(d.ds, "s");
                d.dt = d.ds.Tables["s"];
                count = d.dt.Rows.Count;

                if (count == 1)
                {

                    d.cmdd = new SqlCommand("exec "+proc2+"", d.cn);
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
                        sql += "insert into "+proc3+"  values ('" + dt.Rows[i]["FROM"].ToString() + "','" + dt.Rows[i]["TO"].ToString() + "','" + Convert.ToDateTime(dt.Rows[i]["DATES"]).ToString("yyyy/MM/dd") + "','" + dt.Rows[i]["PRICE"].ToString() + "','" + dt.Rows[i]["CABIN"].ToString() + "','" + dt.Rows[i]["URL"].ToString() + "')";
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
                        d.cmdd = new SqlCommand("delete  "+proc1+"", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        if (radioButton1.Checked)
                        {
                            d.cmdd = new SqlCommand("update "+proc3+" set Montant =(Montant*2)", d.cn);
                            d.cmdd.ExecuteNonQuery();

                        }
                        bb = 1;
                        if (radioButton2.Checked == true) { MessageBox.Show("Part one complete"); }

                    }
                    catch
                    {
                        d.cmdd = new SqlCommand("delete "+proc3+"", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd = new SqlCommand("insert into "+proc1+" values('4')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        MessageBox.Show("Error!! \n Try again with NEW 1");
                    }
                });


                label3.Visible = false;

            }

            if (radioButton1.Checked && bb == 1)
            {

                d.cmdd = new SqlCommand("exec "+proc4+"", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec "+proc5+"", d.cn);
                d.cmdd.ExecuteNonQuery();
                memoir();
                bb = 0;
                MessageBox.Show("Part one complete");
            }

        }
        string name1, name2;

        private void memoir()
        {
            if(domest==2)
            {
                name1 = "namefilesSKYS2Days";
                name2 = "DELETnamefilesSKYS2Days";
            }
            else if(domest==3)
            {
                name1 = "namefilesSKYS3Days";
                name2 = "DELETnamefilesSKYS3Days";
            }
            else if(domest==4)
            {
                name1 = "namefilesSKYS4Days";
                name2 = "DELETnamefilesSKYS4Days";
            }
            else if(domest==7)
            {
                name1 = "namefilesSKYSCOPY";
                name2 = "DELETnamefilesSKYSCOPY";
            }
            else if(domest==14)
            {
                name1 = "namefilesSKYS14Days";
                name2 = "DELETnamefilesSKYS14Days";
            }

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
                    d.cmdd = new SqlCommand("insert into "+name1+" values('" + sqlA + "','" + sqlB + "')", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC "+name2+"", d.cn);
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
                    d.cmdd = new SqlCommand("insert into  " + name1 + " values('" + sqlA + "','" + sqlA + "')", d.cn);
                    d.cmdd.ExecuteNonQuery();
                    d.cmdd = new SqlCommand("EXEC " + name2 + "", d.cn);
                    d.cmdd.ExecuteNonQuery();
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;
            if (domest == 7)
            {
                proc1 = "skyspoinCOPY";
                proc2 = "insertskys2COPY";
                proc3 = "skys1COPY";
                proc4 = "CheapestSkyCOPY";
                proc5 = "cheapskyscannerCOPY";
            }
            else if (domest == 2)
            {
                proc1 = "skyspoin2Days";
                proc2 = "insertskys22Days";
                proc3 = "skys12Days";
                proc4 = "CheapestSky2Days";
                proc5 = "cheapskyscanner2Days";
            }
            else if (domest == 3)
            {
                proc1 = "skyspoin3Days";
                proc2 = "insertskys23Days";
                proc3 = "skys13Days";
                proc4 = "CheapestSky3Days";
                proc5 = "cheapskyscanner3Days";
            }
            else if (domest == 4)
            {
                proc1 = "skyspoin4Days";
                proc2 = "insertskys24Days";
                proc3 = "skys14Days";
                proc4 = "CheapestSky4Days";
                proc5 = "cheapskyscanner4Days";
            }
            else if (domest == 14)
            {
                proc1 = "skyspoin14Days";
                proc2 = "insertskys214Days";
                proc3 = "skys114Days";
                proc4 = "CheapestSky14Days";
                proc5 = "cheapskyscanner14Days";
            }
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
                        sql += "insert into "+proc3+"  values ('" + dt.Rows[i]["FROM"].ToString() + "','" + dt.Rows[i]["TO"].ToString() + "','" + Convert.ToDateTime(dt.Rows[i]["DATES"]).ToString("yyyy/MM/dd") + "','" + dt.Rows[i]["PRICE"].ToString() + "','" + dt.Rows[i]["CABIN"].ToString() + "','" + dt.Rows[i]["URL"].ToString() + "')";
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
                        d.cmdd = new SqlCommand("update "+proc3+" set Montant =(Montant*2)", d.cn);
                        d.cmdd.ExecuteNonQuery();

                        bb = 1;


                    }
                    catch
                    {
                        d.cmdd = new SqlCommand("delete "+proc3+"", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        d.cmdd = new SqlCommand("insert into "+proc1+" values('4')", d.cn);
                        d.cmdd.ExecuteNonQuery();
                        MessageBox.Show("Error!! \n Try again with NEW 1");
                    }
                });


                label2.Visible = false;
            }
            if (bb == 1 && radioButton2.Checked == true)
            {
                d.cmdd = new SqlCommand("exec "+proc4+"", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec "+proc5+"", d.cn);
                d.cmdd.ExecuteNonQuery();
                memoir();
                MessageBox.Show("The second part is complete");

                bb = 0;
            }

        }


        string f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11;
        private void button3_Click(object sender, EventArgs e)
        {

            if(domest==2)
            {
                f1 = "deltTablX2Days";
                f2 = "any52Days";
                f3 = "cheapskyscanner2Days";
                f4 = "addnewchesky2Days";
                f5 = "doblerowschsky2Days";
                f6 = "comprsky2Days";
                f7 = "cmprS2Days";
                f8 = "doblerowssky2Days";
                f9 = "add_G_S2Days";
                f10 = "upd_g_s2Days";
                f11 = "upd_cmprgoogle2Days";
            }
            else if(domest==3)
            {
                f1 = "deltTablX3Days";
                f2 = "any53Days";
                f3 = "cheapskyscanner3Days";
                f4 = "addnewchesky3Days";
                f5 = "doblerowschsky3Days";
                f6 = "comprsky3Days";
                f7 = "cmprS3Days";
                f8 = "doblerowssky3Days";
                f9 = "add_G_S3Days";
                f10 = "upd_g_s3Days";
                f11 = "upd_cmprgoogle3Days";
            }
            else if(domest==4)
            {
                f1 = "deltTablX4Days";
                f2 = "any54Days";
                f3 = "cheapskyscanner4Days";
                f4 = "addnewchesky4Days";
                f5 = "doblerowschsky4Days";
                f6 = "comprsky4Days";
                f7 = "cmprS4Days";
                f8 = "doblerowssky4Days";
                f9 = "add_G_S4Days";
                f10 = "upd_g_s4Days";
                f11 = "upd_cmprgoogle4Days";
            }
            else if(domest==7)
            {
                f1 = "deltTablXCOPY";
                f2 = "any5COPY";
                f3 = "cheapskyscannerCOPY";
                f4 = "addnewcheskyCOPY";
                f5 = "doblerowschskyCOPY";
                f6 = "comprskyCOPY";
                f7 = "cmprSCOPY";
                f8 = "doblerowsskyCOPY";
                f9 = "add_G_SCOPY";
                f10 = "upd_g_sCOPY";
                f11 = "upd_cmprgoogleCOPY";
            }
            else if(domest==14)
            {
                f1 = "deltTablX14Days";
                f2 = "any514Days";
                f3 = "cheapskyscanner14Days";
                f4 = "addnewchesky14Days";
                f5 = "doblerowschsky14Days";
                f6 = "comprsky14Days";
                f7 = "cmprS14Days";
                f8 = "doblerowssky14Days";
                f9 = "add_G_S14Days";
                f10 = "upd_g_s14Days";
                f11 = "upd_cmprgoogle14Days";
            }
            d.cmdd.CommandType = CommandType.Text;
            if (textBox1.Text != "")
            {
                d.cmdd = new SqlCommand("exec "+f1+"", d.cn);
                d.cmdd.ExecuteNonQuery();

                d.cmdd = new SqlCommand("exec " + f2 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec " + f3 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec " + f4 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec " + f5 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("delete " + f6 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec " + f7 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
                d.cmdd = new SqlCommand("exec " + f8 + "", d.cn);
                d.cmdd.ExecuteNonQuery();
            }

            d.cmdd = new SqlCommand("exec " + f9 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec " + f10 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            d.cmdd = new SqlCommand("exec " + f11 + "", d.cn);
            d.cmdd.ExecuteNonQuery();
            dt = null;
            d.dt = null;
        }
    }
}
