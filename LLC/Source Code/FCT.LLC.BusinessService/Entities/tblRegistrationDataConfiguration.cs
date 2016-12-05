using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblRegistrationDataConfiguration")]
    public partial class tblRegistrationDataConfiguration
    {
        [Key]
        public int RegistrationDataConfigurationID { get; set; }

        public int LenderID { get; set; }

        public int? LanguageID { get; set; }

        [StringLength(50)]
        public string PackageLenderName { get; set; }

        [StringLength(50)]
        public string DocumentTypeLenderName { get; set; }

        [Required]
        [StringLength(256)]
        public string Key { get; set; }

        [StringLength(4000)]
        public string Value { get; set; }

        [Required]
        [StringLength(50)]
        public string ValueType { get; set; }
    }
}
