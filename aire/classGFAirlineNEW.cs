using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aire
{
    [Table("comprGOOGLAirline")]
    internal class classGFAirlineNEW
    {

        public int id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string citys { get; set; }
        public DateTime Dates { get; set; }
        public double Olde_price { get; set; }
        public double New_price { get; set; }
        public double Difference { get; set; }
        public double Cheapest { get; set; }
        public string Airline { get; set; }
        public string Aircode { get; set; }
        public string Cabin { get; set; }
        public string Days { get; set; }
        public string Stops { get; set; }
        public string web { get; set; }
        public bool IsTargetFound { get; set; }
        public string Name { get; set; }
        public DateTime NewUploadDate { get; set; }
        public double OtaDiscount { get; set; }
        public double OtaTotal { get; set; }
        public int oldid { get; set; }

        // Price history tracking
        public DateTime? DateNewPriceChanged { get; set; }

        // Target categorization properties
        public bool? IsOldTarget { get; set; }  // Yellow: Difference between -5 and 0
        public bool? IsMonthTarget { get; set; } // Purple: Blue records with different months
        public bool? IsTargetDeal { get; set; }    // Green: Cheapest among all categories
    }
}
