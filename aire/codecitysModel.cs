using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aire
{
    [Table("codecitys")]
    public class codecitysModel
    {
        public string code { get; set; }
        public string city { get; set; }
    }
}
