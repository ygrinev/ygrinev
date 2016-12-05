using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("tblDeal")]
    public class tblDeal : AuditEntityBase
    {
        [Key]
        public int DealID { get; set; }
        [Required]
        public string FCTRefNum { get; set; }
       
        public DateTime? ClosedDate { get; set; }

        public ICollection<LLCProperty> Properties { get; set; }
        public ICollection<LLCMortgage> Mortgages { get; set; }
        public ICollection<LLCNote> Notes { get; set; }


    }
}
