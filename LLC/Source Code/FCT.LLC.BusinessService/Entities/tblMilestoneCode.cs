namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMilestoneCode")]
    public partial class tblMilestoneCode
    {
        public tblMilestoneCode()
        {
            tblMilestones = new HashSet<tblMilestone>();
            tblMilestoneLabels = new HashSet<tblMilestoneLabel>();
        }

        [Key]
        public int MilestoneCodeID { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(10)]
        public string BusinessModel { get; set; }

        public bool IsMajor { get; set; }

        public bool IsOptional { get; set; }

        public bool IsPreClosing { get; set; }

        public bool IsPostClosing { get; set; }

        public bool IsExternalActivated { get; set; }

        public virtual ICollection<tblMilestone> tblMilestones { get; set; }

        public virtual ICollection<tblMilestoneLabel> tblMilestoneLabels { get; set; }
    }
}
