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
    public partial class Serch : Form
    {
        public Serch()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            home h = new home();
            h.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            airline a = new airline();
            a.Show();
        }
        string cabin="null";
        private void button2_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                cabin = "Economy";
                search_skys_flight sg = new search_skys_flight(cabin);
                sg.Show();
            }
            else if(radioButton2.Checked==true)
            {
                cabin = "Business";
                search_skys_flight sg = new search_skys_flight(cabin);
                sg.Show();
            }
            else if(radioButton3.Checked==true)
            {
                cabin = "Premium";
                search_skys_flight sg = new search_skys_flight(cabin);
                sg.Show();
            }
          else if(radioButton4.Checked==true)
            {
                Search_GF_all_cabin alGF = new Search_GF_all_cabin();
                alGF.Show();
            }
              
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GOOGLE_SKYS G = new GOOGLE_SKYS();
            G.Show();
        }
        string itxcabin;
        private void button3_Click_1(object sender, EventArgs e)
        {
            //if (radioButton5.Checked == true)
            //{
                itxcabin = "Economy";
                rechirch_itx_output rio = new rechirch_itx_output(itxcabin);
                rio.Show();
            //}
            //else if (radioButton6.Checked == true)
            //{
            //    itxcabin = "Business";
            //    rechirch_itx_output rio = new rechirch_itx_output(itxcabin);
            //    rio.Show();
            //}
            //else if (radioButton7.Checked == true)
            //{
            //    itxcabin = "Premium";
            //    rechirch_itx_output rio = new rechirch_itx_output(itxcabin);
            //    rio.Show();
            //}
            //else if(radioButton8.Checked==true)
            //{
            //    each_output_all_cabin ou = new each_output_all_cabin();
            //    ou.Show();
            //}

           
        }
        string ITXcabin="null";
        private void button5_Click(object sender, EventArgs e)
        {
            if(radioButton9.Checked==true)
            {
                ITXcabin = "all";
                itxairline it = new itxairline(ITXcabin);
                it.Show();
            }
            else if(radioButton10.Checked==true)
            {
                ITXcabin = "normal";
                itxairline it = new itxairline(ITXcabin);
                it.Show();
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            aftereachitx ea = new aftereachitx();
            ea.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            compare_itx_with_fare c = new compare_itx_with_fare();
            c.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
           target trg = new target();
            trg.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            interim i = new interim();
            i.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            quickskys qu = new quickskys();
            qu.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new_flight_with_sky fs = new new_flight_with_sky();
            fs.Show();
        }

        private void Serch_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            GoogleAirline GfAirline = new GoogleAirline();
            GfAirline.Show();
        }
    }
}
