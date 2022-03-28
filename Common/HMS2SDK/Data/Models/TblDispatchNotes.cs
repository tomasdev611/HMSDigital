using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblDispatchNotes
    {
        public int NoteId { get; set; }
        public int? UserId { get; set; }
        public string Notes { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? OrderId { get; set; }
    }
}
