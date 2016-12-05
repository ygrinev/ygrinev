namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblNotes")]
    public partial class tblNote
    {
        [DataMember]
        [Key]
        public int NotesID { get; set; }

        [DataMember]
        public int DealID { get; set; }

        [DataMember]
        public DateTime NotesDate { get; set; }

        [DataMember]
        public int? UserID { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string Notes { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Usertype { get; set; }

        [DataMember]
        public bool? ViewableToLender { get; set; }

        [DataMember]
        public bool IsNewNote { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }

        [DataMember]
        public int ActionableNoteStatus { get; set; }

        [DataMember]
        public bool? IsSubmitted { get; set; }

        [DataMember]
        public int? LenderNotesID { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string NoteType { get; set; }
    }
}
