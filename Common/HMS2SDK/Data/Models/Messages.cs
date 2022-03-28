using System;

namespace HMS2SDK.Data.Models
{
    public partial class Messages
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
