namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblCreditorListExcluded")]
    public partial class tblCreditorListExcluded
    {
        [Key, StringLength(6)]
        //[ForeignKey("tblRBCCreditorListStaging"), Column(Order = 1)]
        public string CompanyID { get; set; }
        //[Key]
        //[ForeignKey("tblRBCCreditorListStaging"), Column(Order = 2)]
	    public int? CCIN { get; set; }
        [Required]
	    public DateTime LastModifiedDate { get; set; }
        //[Required]
        //public virtual tblRBCCreditorListStaging tblRBCCreditorListStaging { get; set; }
    }
}
