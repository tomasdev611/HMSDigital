using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblTicketsOld
    {
        public int TicketId { get; set; }
        public DateTime? TicketCreated { get; set; }
        public int? TicketLocationId { get; set; }
        public int? TicketStatus { get; set; }
        public DateTime? TicketModified { get; set; }
        public int? TicketUserId { get; set; }
        public string TicketInfo { get; set; }
        public int? TicketAssignment { get; set; }
        public int? TicketType { get; set; }
        public string Attachment { get; set; }
    }
}
