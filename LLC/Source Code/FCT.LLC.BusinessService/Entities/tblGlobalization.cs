namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGlobalization")]
    public partial class tblGlobalization
    {
        [Key]
        public int ResourceID { get; set; }

        [Required]
        [StringLength(5)]
        public string LocaleID { get; set; }

        [Required]
        [StringLength(128)]
        public string ResourceSet { get; set; }

        [Required]
        [StringLength(128)]
        public string ResourceKey { get; set; }

        [Required]
        [StringLength(4000)]
        public string Value { get; set; }

        [Required]
        [StringLength(255)]
        public string Type { get; set; }

        [Column(TypeName = "ntext")]
        public string TextFile { get; set; }

        [Required]
        [StringLength(128)]
        public string Filename { get; set; }

        [StringLength(50)]
        public string VersionNo { get; set; }

        public DateTime? LastModified { get; set; }

        [Column(TypeName = "image")]
        public byte[] BinFile { get; set; }
    }
}
