using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aire
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string user = "admin";
        string pasword = "12345";
        private void button1_Click(object sender, EventArgs e)
        {
            if (user.Equals(textBox1.Text) && pasword.Equals(textBox2.Text))
            {
                
                home h = new home();
                h.Show();
                
            }
            else { MessageBox.Show("The username and password are incorrect!!!"); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
