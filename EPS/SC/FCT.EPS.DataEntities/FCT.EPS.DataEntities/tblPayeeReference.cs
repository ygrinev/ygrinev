namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPayeeReference")]
    public partial class tblPayeeReference
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PayeeInfoID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PayeeTypeID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolutionID { get; set; }

        public virtual tblPayeeInfo tblPayeeInfo { get; set; }

        public virtual tblPayeeType tblPayeeType { get; set; }

        public virtual tblSolution tblSolution { get; set; }
    }
}
