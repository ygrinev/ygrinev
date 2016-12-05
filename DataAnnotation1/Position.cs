using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnnotation1
{
    class Position : IValidateable
    {
        public void Init(TabPage p)
        {
            Validator.Init(this, p);
        }
            public string txtStartDate{ get; set; }
            public string txtExperience{ get; set; }
            public string txtIDE{ get; set; }
            public string txtDatabases{ get; set; }
            public string txtLanguages{ get; set; }
            [StringLength(3, ErrorMessage = "Maximum salary cannot exceed 3 symbols\n"), RegularExpression(@"\d{2,3}", ErrorMessage = "Maximum salary must be 2 or 3 digit number\n")]
            public string txtSalaryMax { get; set; }
            [StringLength(3, ErrorMessage = "Minimum salary cannot exceed 3 symbols\n"), RegularExpression(@"\d{2,3}", ErrorMessage = "Minimum salary must be 2 or 3 digit number\n")]
            public string txtSalaryMin{ get; set; }
            public string txtPosTitle{ get; set; }
    }
}
