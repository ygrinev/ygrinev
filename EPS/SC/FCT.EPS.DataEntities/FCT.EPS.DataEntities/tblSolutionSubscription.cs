namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
     

    [Table("tblSolutionSubscription")]
    public partial class tblSolutionSubscription
    {
        public tblSolutionSubscription()
        {
          
        }

        [Key]
        [StringLength(10)]
        public string SubscriptionID { get; set; }

        public int FCTAccountID { get; set; }

        public int ServiceProviderID { get; set; }

        [StringLength(100)]
        public string ServceEndPoint { get; set; }

        public virtual tblFCTAccount tblFCTAccount { get; set; }

        public virtual tblPaymentServiceProvider tblPaymentServiceProvider { get; set; }
    }
}
