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
    class Company : IValidateable
    {
        public void Init(TabPage p)
        {
            Validator.Init(this, p);
        }
        public int TabIndex { get { return 0; } }
        [Required, StringLength(50), RegularExpression("\\w{2,50}",ErrorMessage="Company Name must have only alpha-nummeric symbols and be 2 to 50 digits!\n")]
        public string txtName { get; set; }
       // [Column(Order = 2), StringLength(2)]
        public string txtMinSize{ get; set; }
        // [Column(Order = 3), StringLength(2)]
        public string txtMaxSize{ get; set; }
        //[Column(Order = 4), StringLength(6)]
        public string txtUnitNumber{ get; set; }
        public string txtStreetAddress{ get; set; }
        public string txtCity{ get; set; }
        public string txtProvince{ get; set; }
        public string txtPhone{ get; set; }
    }
}
