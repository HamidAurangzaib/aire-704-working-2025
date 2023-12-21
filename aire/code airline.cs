using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Charts;
using DataTable = System.Data.DataTable;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;

namespace aire
{
    public partial class code_airline : Form
    {
        ado d = new ado();
        public code_airline()
        {
            InitializeComponent();
        }
        public void Remplissage_DtGdV()
        {

            DataTable dtN = new DataTable();

            d.cmdd.CommandType = CommandType.Text;
            d.cmdd.CommandText = "select * from tx group by code ";
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dtN.Load(d.dr);
            d.dr.Close();


        }
        private void Remplissage_DtGdV1()
        {

            DataTable dt1 = new DataTable();

            d.cmdd.CommandType = CommandType.Text;
            d.cmdd.CommandText = "select * from cpy group by airline";
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dt1.Load(d.dr);
            d.dr.Close();


        }
        private void Remplissage_DtGdV2()
        {

            DataTable dt1 = new DataTable();

            d.cmdd.CommandType = CommandType.Text;
            d.cmdd.CommandText = "select * from airlinex";
            d.cmdd.Connection = d.cn;
            d.dr = d.cmdd.ExecuteReader();
            dt1.Load(d.dr);
            dataGridView3.DataSource = dt1;
            DataGridViewColumn column = dataGridView3.Columns[2];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ((DataGridViewImageColumn)dataGridView3.Columns[2]).ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView3.RowTemplate.Height = 50;
            d.dr.Close();


        }
        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView3.Columns[e.ColumnIndex].Name == "image" && e.Value != null)
            {
                Image image = Image.FromFile(e.Value.ToString());
                e.Value = null;
                e.FormattingApplied = true;
                dataGridView3.Rows[e.RowIndex].Height = image.Height;
                dataGridView3.Columns[e.ColumnIndex].Width = image.Width;
                DataGridViewImageCell cell = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewImageCell;
                cell.Value = image;
            }
        }

        private void code_airline_Load(object sender, EventArgs e)
        {
            d.connecter();
            button6.Hide();
            Remplissage_DtGdV();
            Remplissage_DtGdV1();
            Remplissage_DtGdV2();

        }
        public void supprm()
        {

            d.cmdd = new SqlCommand("exec deletecpy", d.cn);
            d.cmdd.ExecuteNonQuery();
        }
        public void supprm1()
        {
           
            d.cmdd = new SqlCommand("exec deletetx", d.cn);
            d.cmdd.ExecuteNonQuery();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            d.cmdd.Parameters.Clear();
            d.cmdd.CommandType = CommandType.StoredProcedure;
            d.cmdd.CommandText = "airlinxx";
            d.cmdd.Parameters.Add("@name", SqlDbType.VarChar, 20).Value = textBox1.Text;
            d.cmdd.Parameters.Add("@codeiata", SqlDbType.VarChar, 20).Value = textBox2.Text;
            d.cmdd.Parameters.AddWithValue("image", savePhoto(pictureBox1));

            d.cmdd.Connection = d.cn;

            d.cmdd.ExecuteNonQuery();

            supprm();
            supprm1();
            Remplissage_DtGdV();
            Remplissage_DtGdV1();
            Remplissage_DtGdV2();
            updatee();
        }
        //save image    
        public byte[] savePhoto(PictureBox pb)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pb.Image.RawFormat);
            return ms.GetBuffer();
        }
        private void updatee()
        {
           
            d.cmdd = new SqlCommand("exec updatcopy", d.cn);
            d.cmdd.ExecuteNonQuery();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Remplissage_DtGdV();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Remplissage_DtGdV1();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            updatee();
            compar();
        }
        private void compar()
        {
            d.cmdd = new SqlCommand("exec compar", d.cn);
            d.cmdd.ExecuteNonQuery();
        }
        OleDbConnection con;
        DataTable dt = new DataTable();
        private async void button7_Click(object sender, EventArgs e)
        {
            dt = null;
            d.dt = null;

            OpenFileDialog ope = new OpenFileDialog();

            ope.Filter = "ALL Files |*.*| Excel Files |*.xlsx";

            if (ope.ShowDialog() == DialogResult.OK)
            {
                string constr = "PROVIDER= Microsoft.ACE.OLEDB.12.0; Data Source =" + ope.FileName + ";Extended Properties='Excel 12.0;'";
               

                con = new OleDbConnection(constr);
                int cnt;

                OleDbCommand cmd = new OleDbCommand("select * from [data$]", con);
                con.Open();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                cnt = dt.Rows.Count;


                cnt = dt.Rows.Count;
                d.cmdd.CommandType = CommandType.Text;

                string sql = "";
                await Task.Run(() =>
                {

                    for (int i = 0; i < cnt; i++)
                    {
                        
                        sql += " insert into airlinex values ('" + dt.Rows[i]["NAME"].ToString() + "','" + dt.Rows[i]["IATA"].ToString() + "')";
                      
                    }
                    d.cmdd = new SqlCommand(sql, d.cn);
                    d.cmdd.ExecuteNonQuery();

                });
                

            }
            supprm();
            supprm1();
            Remplissage_DtGdV();
            Remplissage_DtGdV1();
            Remplissage_DtGdV2();
            updatee();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void UploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;*.png;)|*.jpg;*.jpeg;.*.gif;*.png;";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(opnfd.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void dataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            textBox1.Text = dataGridView3.Rows[index].Cells[0].Value.ToString();
            textBox2.Text = dataGridView3.Rows[index].Cells[1].Value.ToString();

            DataGridViewCell cell = dataGridView3.Rows[index].Cells[2];

            // Check if the cell value is not null
            if (cell.Value != null && cell.Value is byte[])
            {
                // Convert byte array to Image
                byte[] byteArray = (byte[])cell.Value;
                Image image = ByteArrayToImage(byteArray);

                // Display the image in PictureBox
                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else {
                pictureBox1.Image = null;
            }
        }
        // Method to convert byte array to Image
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                return Image.FromStream(stream);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please select the record from table below");
            }
            else
            {
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "update_airlinxx";
                d.cmdd.Parameters.Add("@name", SqlDbType.VarChar, 20).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@codeiata", SqlDbType.VarChar, 20).Value = textBox2.Text;
                d.cmdd.Parameters.AddWithValue("image", savePhoto(pictureBox1));

                d.cmdd.Connection = d.cn;

                d.cmdd.ExecuteNonQuery();

                MessageBox.Show("Record Updated Succesfully");

                pictureBox1.Image = null;

                textBox1.Text = "";
                textBox2.Text = "";

                Remplissage_DtGdV2();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please select the record from table below");
            }
            else
            {
                d.cmdd.Parameters.Clear();
                d.cmdd.CommandType = CommandType.StoredProcedure;
                d.cmdd.CommandText = "delete_airlinxx";
                d.cmdd.Parameters.Add("@name", SqlDbType.VarChar, 20).Value = textBox1.Text;
                d.cmdd.Parameters.Add("@codeiata", SqlDbType.VarChar, 20).Value = textBox2.Text;

                d.cmdd.Connection = d.cn;

                d.cmdd.ExecuteNonQuery();

                MessageBox.Show("Record Deleted Succesfully");

                pictureBox1.Image = null;

                textBox1.Text = "";
                textBox2.Text = "";

                Remplissage_DtGdV2();

            }
        }
    }
}
