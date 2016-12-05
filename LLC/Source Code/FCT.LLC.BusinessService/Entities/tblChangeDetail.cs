using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblChangeDetail")]
    public partial class tblChangeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ChangeDetailID { get; set; }

        public int ChangeID { get; set; }

        [Required]
        [StringLength(255)]
        public string TagPath { get; set; }

        public string PreviousValue { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
