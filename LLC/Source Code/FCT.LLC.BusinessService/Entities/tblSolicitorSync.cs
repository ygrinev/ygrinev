using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSolicitorSync")]
    public partial class tblSolicitorSync
    {
        public tblSolicitorSync()
        {
            tblSolicitorSyncReportDatas = new HashSet<tblSolicitorSyncReportData>();
        }

        [Key]
        public int SolicitorSyncID { get; set; }

        public int LenderID { get; set; }

        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        [Required]
        [StringLength(1)]
        public string LoadType { get; set; }

        [Column(TypeName = "image")]
        public byte[] FileData { get; set; }

        public DateTime CreateDateTime { get; set; }

        [Column(TypeName = "text")]
        public string TempFileData { get; set; }

        public virtual tblLender tblLender { get; set; }

        public virtual ICollection<tblSolicitorSyncReportData> tblSolicitorSyncReportDatas { get; set; }
    }
}
