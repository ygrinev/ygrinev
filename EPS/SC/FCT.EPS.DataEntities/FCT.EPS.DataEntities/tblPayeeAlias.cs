namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tblPayeeAlias
    {
        public int PayeeInfoID { get; set; }

        [Key]
        public int AliasID { get; set; }

        [Required]
        [StringLength(100)]
        public string Alias { get; set; }

        public virtual tblPayeeInfo tblPayeeInfo { get; set; }
    }
}
