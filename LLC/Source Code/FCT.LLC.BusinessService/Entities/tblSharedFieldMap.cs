using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSharedFieldMap")]
    public partial class tblSharedFieldMap
    {
        [Key]
        public int SharedFieldMapID { get; set; }

        [StringLength(255)]
        public string FriendlyFieldName { get; set; }

        [Required]
        [StringLength(50)]
        public string OptionName { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldKey { get; set; }

        [Required]
        [StringLength(255)]
        public string LawyerFieldReference { get; set; }

        [Required]
        [StringLength(255)]
        public string LenderFieldReference { get; set; }

        public bool UseForeignKeyLawyer { get; set; }

        public bool UseForeignKeyLender { get; set; }

        public int? SortOrder { get; set; }
    }
}
