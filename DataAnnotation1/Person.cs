using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnnotation1
{
    class Person : IValidateable
    {
        public void Init(TabPage p)
        {
            Validator.Init(this, p);
        }
        [Required, StringLength(8, ErrorMessage = "Marital status string cannot exceed 8 symbols\n"), RegularExpression(@"(MARRIED|DIVORCED|SINGLE)", ErrorMessage = "Marital status must be 'MARRIED', 'DIVORCED' or 'SINGLE'\n")]
        public string txtMaritalStatus { get; set; }
        //[StringLength(12), RegularExpression(@"(\d[-])*", ErrorMessage = @"Person's Phone number may should follow the pattern: '416\-\d{3}\-\d{4}'\n")]
        public string txtPersonPhone { get; set; }
        public string txtPersonProvince { get; set; }
        public string txtPersonCity { get; set; }
        public string txtPersonStreet { get; set; }
        public string txtPersonUnit { get; set; }
        public string txtMiddleName { get; set; }
        public string txtLastName { get; set; }
        public string txtFirstName { get; set; }
    }
}
