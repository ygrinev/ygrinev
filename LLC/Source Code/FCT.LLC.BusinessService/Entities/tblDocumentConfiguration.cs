using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentConfiguration")]
    public partial class tblDocumentConfiguration
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(250)]
        public string Key { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8000)]
        public string Value { get; set; }

        public int? LenderID { get; set; }

        [Key]
        [Column(Order = 2)]
        public int DocumentConfigurationID { get; set; }
    }
}
