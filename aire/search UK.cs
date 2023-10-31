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
    public partial class search_UK : Form
    {
        public search_UK()
        {
            InitializeComponent();
        }
        int domestic =1;
        private void button1_Click(object sender, EventArgs e)
        {
           
                google_skys_copy g = new google_skys_copy(domestic);
                g.Show();
          
        }

        

       

     
        private void search_UK_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            search_skys_flight_copy sr = new search_skys_flight_copy(domestic);
            sr.Show();
        }
    }
}
