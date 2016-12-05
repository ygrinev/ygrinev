using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerAuthorizedPaymentMethodType")]
    public partial class tblLawyerAuthorizedPaymentMethodType
    {
        public tblLawyerAuthorizedPaymentMethodType()
        {
            tblLawyerPaymentAuthorizationMethods = new HashSet<tblLawyerPaymentAuthorizationMethod>();
        }

        [Key]
        public int AuthorizedTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        public virtual ICollection<tblLawyerPaymentAuthorizationMethod> tblLawyerPaymentAuthorizationMethods { get; set; }
    }
}
