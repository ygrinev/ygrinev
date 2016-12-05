using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerClerk")]
    public class tblLawyerClerk
    {
        [Key]
        public int LawyerClerkID { get; set; }

        public int LawyerID { get; set; }

        public int ClerkID { get; set; }

        public bool Enabled { get; set; }

    }
}
