using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblResponsesOld
    {
        public int ResponseId { get; set; }
        public int? TicketId { get; set; }
        public int? UserId { get; set; }
        public string Response { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int? ResponseClosed { get; set; }
    }
}
