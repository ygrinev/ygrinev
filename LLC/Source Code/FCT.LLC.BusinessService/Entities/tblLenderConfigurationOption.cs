using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderConfigurationOption")]
    public partial class tblLenderConfigurationOption
    {
        [Key]
        public int ConfigurationValueID { get; set; }

        public int OptionID { get; set; }

        public int? LenderID { get; set; }

        [Required]
        [StringLength(256)]
        public string Key { get; set; }

        [StringLength(4000)]
        public string Value { get; set; }

        public DateTime LastModified { get; set; }

        public virtual tblConfigurationOption tblConfigurationOption { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
