using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderInstruction")]
    public partial class tblLenderInstruction
    {
        public int ProgramID { get; set; }

        public int InstructionID { get; set; }

        public int? InstructionForFunding { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [Key]
        public int LenderInstructionID { get; set; }
    }
}
