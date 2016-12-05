using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFinalReportClosingOption")]
    public partial class tblFinalReportClosingOption
    {
        public tblFinalReportClosingOption()
        {
            //tblDeals = new HashSet<tblDeal>();
            //tblDeals1 = new HashSet<tblDeal>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FinalReportClosingOptionID { get; set; }

        [StringLength(20)]
        public string FinalReportClosingOptionCode { get; set; }

        //public virtual ICollection<tblDeal> tblDeals { get; set; }

        //public virtual ICollection<tblDeal> tblDeals1 { get; set; }
    }
}
