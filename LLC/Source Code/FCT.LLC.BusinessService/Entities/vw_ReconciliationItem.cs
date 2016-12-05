using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    public class vw_ReconciliationItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public int ItemID { get; set; }

        [StringLength(16)]
        public string BatchNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(11)]
        public string FCTURN { get; set; }

        [StringLength(30)]
        public string TransactionType { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal? AmountIn { get; set; }

        [Column(TypeName = "money")]
        public decimal? AmountOut { get; set; }

        [StringLength(15)]
        public string ItemType { get; set; }
    }
}
