using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("tblNotes")]
    public class LLCNote: AuditEntityBase
    {
        [Key]
        public int NotesID { get; set; }
        [Required]
        public int ActionableNoteStatus { get; set; }
        [Required]
        public string NoteType { get; set; }

        public string Notes { get; set; }

        public DateTime NotesDate { get; set; }

        [Column("DealID")]
        public int DealID { get; set; }
    }
}
