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
    public partial class GF_Hotel : Form
    {
        public GF_Hotel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            holidays_google h = new holidays_google();
            h.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Search_Easyjet se = new Search_Easyjet();
            se.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Search_Jet2holidays sj = new Search_Jet2holidays();
            sj.Show();
        }
    }
}
