using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aire
{
    [Table("airlinex")]
    public class airlinexModel
    {
        [StringLength(80)] // Airline name with max length 80
        public string Airline { get; set; }

        [StringLength(40)] // Airline code with max length 40
        public string CodeAirline { get; set; }

        public byte[] Photo { get; set; } // Photo as varbinary (byte array)
    }
}
