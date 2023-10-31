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
    public partial class Srch_Hotel : Form
    {
        public Srch_Hotel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hotel ht = new Hotel("null", "null");
            ht.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Disney ht = new Disney("null", "null");
            ht.Show();
        }
    }
}
