namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblSolution")]
    public partial class tblSolution
    {
        public tblSolution()
        {
            tblFCTAccount = new HashSet<tblFCTAccount>();
            tblPayeeReference = new HashSet<tblPayeeReference>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int SolutionID { get; set; }

        [Required]
        [StringLength(50)]
        public string SolutionName { get; set; }

        public virtual ICollection<tblFCTAccount> tblFCTAccount { get; set; }

        public virtual ICollection<tblPayeeReference> tblPayeeReference { get; set; }
    }
}
