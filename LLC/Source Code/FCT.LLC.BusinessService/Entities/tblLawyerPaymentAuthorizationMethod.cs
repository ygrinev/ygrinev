using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerPaymentAuthorizationMethod")]
    public partial class tblLawyerPaymentAuthorizationMethod
    {
        [Key]
        public int PaymentMethodID { get; set; }

        public int LawyerID { get; set; }

        public int AuthorizedTypeID { get; set; }

        public virtual tblLawyer tblLawyer { get; set; }

        public virtual tblLawyerAuthorizedPaymentMethodType tblLawyerAuthorizedPaymentMethodType { get; set; }
    }
}
