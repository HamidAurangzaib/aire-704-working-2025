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
    public partial class Search_USA : Form
    {
        public Search_USA()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Search_SKYS_GF_USA usa = new Search_SKYS_GF_USA();
            usa.Show();
        }
    }
}
