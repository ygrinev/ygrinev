using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderBusinessField")]
    public partial class tblLenderBusinessField
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LenderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessFieldID { get; set; }

        [Required]
        [StringLength(10)]
        public string SubmitType { get; set; }

        public virtual tblBusinessField tblBusinessField { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
