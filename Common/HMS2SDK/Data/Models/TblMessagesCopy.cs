using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblMessagesCopy
    {
        public int MessageId { get; set; }
        public int? MessageStatus { get; set; }
        public DateTime? MessageEnd { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
    }
}
