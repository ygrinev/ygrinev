using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFee")]
    public partial class tblFee
    {
        [Key]
        public int FeeID { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? HST { get; set; }

        [Column(TypeName = "money")]
        public decimal? GST { get; set; }

        [Column(TypeName = "money")]
        public decimal? QST { get; set; }

    }
}
