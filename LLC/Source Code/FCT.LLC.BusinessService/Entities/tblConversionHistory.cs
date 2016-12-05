using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblConversionHistory")]
    public partial class tblConversionHistory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string LenderDealRefNumber { get; set; }

        public DateTime ConversionTimeStamp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ConversionSchemaVersion { get; set; }

        public bool IsViewedByLawyer { get; set; }
    }
}
