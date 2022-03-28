using System;

namespace HMS2SDK.Data.Models
{
    public partial class OrderMap
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public int SourceOrderNumber { get; set; }
        public int HmsOrderNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
