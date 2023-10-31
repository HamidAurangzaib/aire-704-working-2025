using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace aire
{
    public partial class groupcode : Form
    {
        public groupcode()
        {
            InitializeComponent();
        }
        ado d = new ado();
        public void Remplissage_DtGdV()
        {

            if (d.dt.Rows != null)
            {
                d.dt.Clear();
            }

            d.cmdd.CommandType = CommandType.Text;
            d.cmdd.CommandText = "select * from codecitys";
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            dataGridView1.DataSource = d.dt;
            d.dr.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddCode();
            Remplissage_DtGdV();
        }

        private void groupcode_Load(object sender, EventArgs e)
        {
            d.connecter();
            Remplissage_DtGdV();
        }
        public void AddCode()
        {
            int count = 0;
            string vr = textBox1.Text;
            string vr1 = textBox2.Text;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == vr1 && Convert.ToString(row.Cells[1].Value) == vr)
                {
                    count = 1;
                }

            }
            if (count == 0)
            {

                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "addgroup";
                d.cmdd.Parameters.Add("@goup_name", SqlDbType.VarChar, 20).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@code", SqlDbType.VarChar, 20).Value = textBox2.Text;
                d.cmdd.Connection = d.cn;
                d.cmdd.ExecuteNonQuery();
                count = 0;
            }
            else if (count == 1)
            {
                MessageBox.Show("Already exist");
                count = 0;
            }
        }
        public void Search1code()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "Searsh1group";
            d.cmdd.Parameters.Add("@group", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Parameters.Add("@code", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            dataGridView1.DataSource = d.dt;

        }
        public void Search2code()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "Searsh2group";
            d.cmdd.Parameters.Add("@group", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            dataGridView1.DataSource = d.dt;

        }
        public void Search3code()
        {
            d.dt.Rows.Clear();
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "Searsh3group";
            d.cmdd.Parameters.Add("@code", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            d.dt.Load(d.dr);
            dataGridView1.DataSource = d.dt;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Search1code();
                button1.Enabled = false;

            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                Search2code();
                button1.Enabled = false;
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                Search3code();
                button1.Enabled = false;
            }
        }
        OleDbConnection con;

        DataTable dt;
        private async void button2_Click(object sender, EventArgs e)
        {
            d.dt = null;
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "ALL Files |*.*| Excel Files |*.xlsx";
            if (op.ShowDialog() == DialogResult.OK)
            {
                string constr = "PROVIDER= Microsoft.ACE.OLEDB.12.0; Data Source =" + op.FileName + ";Extended Properties='Excel 12.0;'";
                textBox1.Text = "";
                textBox1.Text = op.FileName.ToString();
                con = new OleDbConnection(constr);

                OleDbCommand cmd = new OleDbCommand("select * from [group$]", con);
                con.Open();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int cntr = 0;
                cntr = dt.Rows.Count;


                d.cmdd.CommandType = CommandType.Text;
                label3.Visible = true;
                int b = 0;
                string sql = "";
                string sql1 = "", sql2 = "", sql3 = "", sql4 = "", sql5 = "", sql6 = "", sql7 = "", sql8 = "", sql9 = "", sql10 = "", sql11 = "", sql12 = "";

                int j = 1;
                await Task.Run(() =>
                {

                    for (int i = 0; i < cntr; i++)
                    {
                        j++;
                        sql += "insert into codecitys  values ('" + dt.Rows[i]["GROUPS"].ToString() + "','" + dt.Rows[i]["CODE"].ToString() + "')";
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
                    //try
                    //{

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

                    //  }
                    //catch
                    //{

                    //    MessageBox.Show("Error!! \n Try again ");
                    //}
                });





                MessageBox.Show("Part complete");

            }
        }
        public void delete1code()
        {

            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "delete1group";
            d.cmdd.Parameters.Add("@group", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Parameters.Add("@code", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Connection = d.cn;
            d.cmdd.ExecuteNonQuery();

        }
        public void delete2code()
        {

            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "delete2group";
            d.cmdd.Parameters.Add("@group", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Connection = d.cn;
            d.cmdd.ExecuteNonQuery();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                delete1code();
                Remplissage_DtGdV();
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                delete2code();
                Remplissage_DtGdV();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
          
            Remplissage_DtGdV();
        }
    }

}
