using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class ProvinceTax
    {
        public string Province { get; set; }
        public decimal GSTRate { get; set; }
        public decimal HSTRate { get; set; }
        public decimal QSTRate { get; set; }
    }
}
