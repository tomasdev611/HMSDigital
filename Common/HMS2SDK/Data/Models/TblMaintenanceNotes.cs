using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblMaintenanceNotes
    {
        public int MnoteId { get; set; }
        public string MainNote { get; set; }
        public int? UserId { get; set; }
        public DateTime? NoteSubmitted { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
    }
}
