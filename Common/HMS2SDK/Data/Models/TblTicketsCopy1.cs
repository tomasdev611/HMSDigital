using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblTicketsCopy1
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
        public string Driver { get; set; }
        public string Csr { get; set; }
        public int Survey { get; set; }
        public string ReviewOrder { get; set; }
        public int PendingClose { get; set; }
        public string AttachmentClose { get; set; }
    }
}
