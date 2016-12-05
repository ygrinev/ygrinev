using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCreditCard")]
    public partial class tblCreditCard
    {
        [Key]
        public int CreditCardID { get; set; }

        [Required]
        [StringLength(20)]
        public string CardType { get; set; }

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; }

        public int? ExpiryYear { get; set; }

        public int? ExpirtyMonth { get; set; }

        public int? LawyerID { get; set; }

        [StringLength(10)]
        public string SecureNumber { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public int? AddressID { get; set; }
    }
}
