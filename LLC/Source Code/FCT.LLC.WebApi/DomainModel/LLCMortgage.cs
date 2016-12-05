using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("tblMortgage")]
    public class LLCMortgage: AuditEntityBase
    {
        [Key]
        public int MortgageID { get; set; }
        public string MortgageNumber { get; set; }
        public string MortgageType { get; set; }

        [Column("DealID")]
        public int DealID { get; set; }
    }
}
