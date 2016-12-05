using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblTrustAccountLender")]
    public partial class tblTrustAccountLender
    {
        [Key]
        public int LenderTrustAccountID { get; set; }

        public int TrustAccountID { get; set; }

        public int LenderID { get; set; }

        public virtual tblLender tblLender { get; set; }

        public virtual tblTrustAccount tblTrustAccount { get; set; }
    }
}
