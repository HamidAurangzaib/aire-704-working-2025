using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aire
{
    public partial class ImageUpload : Form
    {
        ado d = new ado();
        public ImageUpload()
        {
            InitializeComponent();
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

        private void Submit_Click(object sender, EventArgs e)
        {
            d.cmdd = new SqlCommand("insert into Images values(@image, @airline, @code)", d.cn);
            d.cmdd.Parameters.AddWithValue("image", savePhoto(pictureBox1));
            d.cmdd.Parameters.AddWithValue("airline", textBox2.Text.ToString());
            d.cmdd.Parameters.AddWithValue("code", textBox1.Text.ToString());
            d.cmdd.ExecuteNonQuery();

            MessageBox.Show("Image Saved in Database ", "Image Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //save image    
        public byte[] savePhoto(PictureBox pb)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pb.Image.RawFormat);
            return ms.GetBuffer();
        }

        private void ImageUpload_Load(object sender, EventArgs e)
        {
            d.connecter();

            //comb();
        }
        //private void comb()
        //{
        //    d.ds.Clear();

        //    d.da = new SqlDataAdapter("select distinct Airline from airlinePhoto", d.cn);
        //    d.da.Fill(d.ds, "Airline");

        //    //Adding 'Please Select'
        //    DataRow HdRow = d.ds.Tables["Airline"].NewRow();
        //    HdRow[0] = "Please Select";
        //    d.ds.Tables["Airline"].Rows.InsertAt(HdRow, 0);

        //    comboBox2.DataSource = d.ds.Tables["Airline"];
        //    comboBox2.DisplayMember = "Airline";
        //    comboBox2.ValueMember = "Airline";

        //    d.da = new SqlDataAdapter("select distinct Aircode from airlinePhoto", d.cn);
        //    d.da.Fill(d.ds, "Aircode");

        //    //Adding 'Please Select'
        //    DataRow Hd2Row = d.ds.Tables["Aircode"].NewRow();
        //    Hd2Row[0] = "Please Select";
        //    d.ds.Tables["Aircode"].Rows.InsertAt(Hd2Row, 0);

        //    comboBox4.DataSource = d.ds.Tables["Aircode"];
        //    comboBox4.DisplayMember = "Aircode";
        //    comboBox4.ValueMember = "Aircode";
        //}

        //private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    d.da = new SqlDataAdapter("select distinct Aircode from airlinePhoto where Airline = ", d.cn);
        //    d.da.Fill(d.ds, "Aircode");

        //    //Adding 'Please Select'
        //    DataRow Hd2Row = d.ds.Tables["Aircode"].NewRow();
        //    Hd2Row[0] = "Please Select";
        //    d.ds.Tables["Aircode"].Rows.InsertAt(Hd2Row, 0);

        //    comboBox4.DataSource = d.ds.Tables["Aircode"];
        //    comboBox4.DisplayMember = "Aircode";
        //    comboBox4.ValueMember = "Aircode";
        //}
    }
}
