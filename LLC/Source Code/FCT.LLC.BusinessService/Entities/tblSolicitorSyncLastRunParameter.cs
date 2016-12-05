using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblSolicitorSyncLastRunParameter
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LenderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string LoadType { get; set; }

        public DateTime LastRunDateTime { get; set; }

        public int LastRunSerialNumber { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
