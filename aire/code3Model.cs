using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aire
{
    [Table("code3")]
    public class code3Model
    {
        public string code { get; set; }
        public string city { get; set; }
        public string photos { get; set; }
    }
}
