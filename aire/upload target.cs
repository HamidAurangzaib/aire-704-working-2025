using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace aire
{
    public partial class upload_target : Form
    {
        OleDbConnection con;
        DataTable dt;
        DataTable dt1;
        public upload_target()
        {
            InitializeComponent();
        }
        ado d = new ado();
        private void datatarget()
        {
            if (d.dt != null)
                d.dt.Rows.Clear();
           
            d.ds = new DataSet();
            d.da = new SqlDataAdapter("select * from tblTarget", d.cn);
            d.ds = new DataSet();

            d.da.Fill(d.ds, "tblTarget");
            d.dt = d.ds.Tables["tblTarget"];
            dataGridView1.DataSource = d.dt;
            dataGridView1.Columns[0].Visible = false; // Hide the first column (ID column)
        }
        private void upload_target_Load(object sender, EventArgs e)
        {
            d.connecter();
            datatarget();

            // Hide radio buttons 1 and 2
            radioButton1.Visible = false;
            radioButton2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox6.Text == "" || textBoxCabin.Text == "" || textBoxOtaDisc.Text == "")
            {
                MessageBox.Show("Please fill all values");
            }
            else
            {
                d.cmd = new SqlCommandBuilder(d.da);

                DataRow dr = d.dt.NewRow();
                //dr[0] is id column
                dr[1] = textBox1.Text; //tblTaregt table column: 2 => dr[1]
                dr[2] = textBox2.Text;
                dr[3] = dateTimePicker1.Text;
                dr[4] = dateTimePicker2.Text;
                dr[5] = textBox6.Text;
                dr[6] = textBox3.Text;
                dr[7] = textBoxCabin.Text;
                dr[10] = textBoxOtaDisc.Text;

                d.dt.Rows.Add(dr);
                d.da.Update(d.dt);

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                dateTimePicker1.Text = DateTime.Now.ToString();
                dateTimePicker2.Text = DateTime.Now.ToString();
                textBox6.Text = "";
                textBoxCabin.Text = "";
                textBoxOtaDisc.Text = "";
                MessageBox.Show("Record inserted successfully");
            }
        }
        int index;
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Please fill all values");
            }
            else
            {
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "CheckTargetRecordExistence";
                d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
                d.cmdd.Parameters.Add("@fromDate", SqlDbType.Date).Value = dateTimePicker1.Text;
                d.cmdd.Parameters.Add("@toDate", SqlDbType.Date).Value = dateTimePicker2.Text;
                d.cmdd.Parameters.Add("@price", SqlDbType.Float, 20).Value = float.Parse(textBox6.Text);
                d.cmdd.Parameters.Add("@aircode", SqlDbType.VarChar).Value = textBox3.Text;

                d.cmdd.Connection = d.cn;

                int recordCount = (int)d.cmdd.ExecuteScalar(); // ExecuteScalar to check record existence

                if (recordCount > 0)
                {
                    // Record exists, proceed with deletion
                    d.cmdd.CommandText = "deleteSingleTarget";
                    d.dr = d.cmdd.ExecuteReader(); // Perform the deletion
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cnt = dv.Count;

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    dateTimePicker1.Text = DateTime.Now.ToString();
                    dateTimePicker2.Text = DateTime.Now.ToString();
                    textBox6.Text = "";
                    textBoxCabin.Text = "";
                    textBoxOtaDisc.Text = "";

                    MessageBox.Show("Record deleted successfully.");
                }
                else
                {
                    // Record doesn't exist
                    MessageBox.Show("Record does not exist in the database.");
                }
                datatarget();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if(textBox3.Text.Length>2)
            // {
            //     //new
            //     //flight and sky
            //     d.cmdd = new SqlCommand("exec test3", d.cn);
            //     d.cmdd.ExecuteNonQuery();

            //     //itx and gds
            //     d.cmdd = new SqlCommand("exec test4", d.cn);
            //     d.cmdd.ExecuteNonQuery();
            // }
            // else
            // {
            //     MessageBox.Show("Error in TextBox (Airline ITX) you need enter Name Airline like this (Vietnam Airlines) not (VA)");
            // }

            d.cmdd = new SqlCommand("exec UpdateIsFoundStatusForGFAirline", d.cn);
            d.cmdd.CommandTimeout = 0; //in seconds
            d.cmdd.ExecuteNonQuery();

            MessageBox.Show("Finished");


        }

        private async void button4_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "ALL Files |*.*| Excel Files |*.xlsx";
            if (op.ShowDialog() == DialogResult.OK)
            {
                string constr = "PROVIDER= Microsoft.ACE.OLEDB.12.0; Data Source =" + op.FileName + ";Extended Properties='Excel 12.0;'";
                textBox1.Text = "";
                //textBox1.Text = op.FileName.ToString();
                con = new OleDbConnection(constr);

                OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
                con.Open();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int cntr = 0;
                cntr = dt.Rows.Count;


               
                    //d.cmdd = new SqlCommand("delete tblTarget", d.cn);
                    //d.cmdd.ExecuteNonQuery();
                
                d.cmdd.CommandType = CommandType.Text;
                label3.Visible = true;
                int b = 0;
                string sql = "";
                string sql1 = "", sql2 = "", sql3 = "", sql4 = "", sql5 = "", sql6 = "", sql7 = "", sql8 = "", sql9 = "", sql10 = "", sql11 = "", sql12 = "";

                int j = 1;
                await Task.Run(() => {

                    for (int i = 0; i < cntr; i++)
                    {
                        j++;
                        sql += "insert into tblTarget  values ('" + dt.Rows[i]["FROM"].ToString() + "','" + dt.Rows[i]["TO"].ToString() + "','"  + dt.Rows[i]["FROM DATE"].ToString() + "','" + dt.Rows[i]["TO DATE"].ToString() + "','" + dt.Rows[i]["PRICE"].ToString() + "','" + dt.Rows[i]["AIRCODE"].ToString() + "','" + dt.Rows[i]["CABIN"].ToString() + "','" + dt.Rows[i]["STOPS"].ToString() + "','" + dt.Rows[i]["DAYS"].ToString() + "','" + dt.Rows[i]["OTADiscount"].ToString() + "')";
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
                       
                    }
                    catch(Exception ex)
                    {
                   
                        MessageBox.Show(ex.Message);
                    }
                });

                datatarget();
                MessageBox.Show("Records Uploaded Successfully");

                //label3.Visible = false;

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            index = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
            dateTimePicker1.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
            dateTimePicker2.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.Rows[index].Cells[5].Value.ToString();
            textBox3.Text = dataGridView1.Rows[index].Cells[6].Value.ToString();
            textBoxCabin.Text = dataGridView1.Rows[index].Cells[7].Value.ToString();
            textBoxOtaDisc.Text = dataGridView1.Rows[index].Cells[10].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show("Are you sure you want to delete all records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Check if the user clicked "Yes" in the confirmation dialog
            if (result == DialogResult.Yes)
            {
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd = new SqlCommand("delete tblTarget", d.cn);
                d.cmdd.ExecuteNonQuery();

                MessageBox.Show("Records Deleted Successfully");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Please fill all values");
            }
            else
            {
                int id = (int)dataGridView1.Rows[index].Cells[0].Value;
                d.dt.Rows.Clear();
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "CheckTargetRecordExistenceById";
                d.cmdd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                d.cmdd.Connection = d.cn;

                int recordCount = (int)d.cmdd.ExecuteScalar(); // ExecuteScalar to check record existence

                if (recordCount > 0)
                {
                    // Record exists, proceed with deletion
                    d.cmdd.CommandText = "UpdateTarget";

                    d.cmdd.Parameters.Add("@from", SqlDbType.VarChar, 20).Value = textBox1.Text;
                    d.cmdd.Parameters.Add("@to", SqlDbType.VarChar, 20).Value = textBox2.Text;
                    d.cmdd.Parameters.Add("@fromDate", SqlDbType.Date).Value = dateTimePicker1.Text;
                    d.cmdd.Parameters.Add("@toDate", SqlDbType.Date).Value = dateTimePicker2.Text;
                    d.cmdd.Parameters.Add("@price", SqlDbType.Float, 20).Value = float.Parse(textBox6.Text);
                    d.cmdd.Parameters.Add("@aircode", SqlDbType.VarChar).Value = textBox3.Text;
                    d.cmdd.Parameters.Add("@cabin", SqlDbType.VarChar).Value = textBoxCabin.Text;
                    d.cmdd.Parameters.Add("@otaDiscount", SqlDbType.Float, 20).Value = float.Parse(textBoxOtaDisc.Text);

                    d.dr = d.cmdd.ExecuteReader(); // Perform the deletion
                    d.dt.Load(d.dr);
                    DataView dv = new DataView(d.dt);
                    int cnt = dv.Count;

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    dateTimePicker1.Text = DateTime.Now.ToString();
                    dateTimePicker2.Text = DateTime.Now.ToString();
                    textBox6.Text = "";
                    textBoxCabin.Text = "";
                    textBoxOtaDisc.Text = "";

                    MessageBox.Show("Record updated successfully.");
                }
                else
                {
                    // Record doesn't exist
                    MessageBox.Show("Record does not exist in the database.");
                }
                datatarget();
            }
        }
    }
}
