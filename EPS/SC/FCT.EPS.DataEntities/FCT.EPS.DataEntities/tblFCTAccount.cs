namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblFCTAccount")]
    public partial class tblFCTAccount
    {
        public tblFCTAccount()
        {
            tblSolutionSubscription = new HashSet<tblSolutionSubscription>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int FCTAccountID { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public int? AddressID { get; set; }

        [StringLength(10)]
        public string BankName { get; set; }

        [StringLength(10)]
        public string TransitNumber { get; set; }

        [StringLength(10)]
        public string BranchNumber { get; set; }

        public int? SolutionID { get; set; }

        public virtual tblAddress tblAddress { get; set; }

        public virtual ICollection<tblSolutionSubscription> tblSolutionSubscription { get; set; }

        public virtual tblBPSReference tblBPSReference { get; set; }

        public virtual tblSolution tblSolution { get; set; }
    }
}
