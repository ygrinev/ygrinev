namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentServiceProvider")]
    public partial class tblPaymentServiceProvider
    {
        public tblPaymentServiceProvider()
        {
            tblPaymentTransaction = new HashSet<tblPaymentTransaction>();
            tblSolutionSubscription = new HashSet<tblSolutionSubscription>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ServiceProviderID { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceProviderName { get; set; }

        public virtual ICollection<tblPaymentTransaction> tblPaymentTransaction { get; set; }

        public virtual ICollection<tblSolutionSubscription> tblSolutionSubscription { get; set; }
    }
}
