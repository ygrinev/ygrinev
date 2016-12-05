using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPaymentAuthorization")]
    public partial class tblPaymentAuthorization
    {
        [Key]
        public int PaymentAuthorizationID { get; set; }

        public int LawyerID { get; set; }

        public int DealID { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthorizationCode { get; set; }

        public DateTime DateTime { get; set; }
    }
}
