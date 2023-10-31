using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aire
{
    class ClassTAX
    {
        public string Code { get; set; }
        public string From { get; set; }
        public string Via { get; set; }
        public string To { get; set; }
        public string Airline { get; set; }
        public string Cabin { get; set; }
        public double Tax1 { get; set; }
        public string Tcode1 { get; set; }
        public double Tax2 { get; set; }
        public string Tcode2 { get; set; }
        public double Tax3 { get; set; }
        public string Tcode3 { get; set; }
        public double Total_tax { get; set; }
    }
}
