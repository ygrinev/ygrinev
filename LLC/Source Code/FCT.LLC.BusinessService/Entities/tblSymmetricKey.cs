using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSymmetricKey")]
    public partial class tblSymmetricKey
    {
        [Key]
        public Guid SymmetricKeyId { get; set; }

        [Required]
        [StringLength(256)]
        public string KeyValue { get; set; }

        [Required]
        [StringLength(256)]
        public string InitializationVector { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comments { get; set; }

        [Required]
        [StringLength(256)]
        public string CheckSum { get; set; }

        public bool IsProtectedByDPAPI { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
