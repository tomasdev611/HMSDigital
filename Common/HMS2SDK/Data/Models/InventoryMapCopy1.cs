using System;

namespace HMS2SDK.Data.Models
{
    public partial class InventoryMapCopy1
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string SourceInventoryCode { get; set; }
        public string HmsInventoryCode { get; set; }
        public DateTime CreatedTimestamp { get; set; }
    }
}
