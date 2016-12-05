using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblConfigurationOption")]
    public partial class tblConfigurationOption
    {
        public tblConfigurationOption()
        {
            tblApplicationConfigurationOptions = new HashSet<tblApplicationConfigurationOption>();
            tblLenderConfigurationOptions = new HashSet<tblLenderConfigurationOption>();
        }

        [Key]
        public int OptionID { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string OptionName { get; set; }

        [Required]
        [StringLength(255)]
        public string OptionType { get; set; }

        [StringLength(255)]
        public string OptionDescription { get; set; }

        public DateTime LastModified { get; set; }

        public virtual ICollection<tblApplicationConfigurationOption> tblApplicationConfigurationOptions { get; set; }

        public virtual tblConfigurationCategory tblConfigurationCategory { get; set; }

        public virtual ICollection<tblLenderConfigurationOption> tblLenderConfigurationOptions { get; set; }
    }
}
