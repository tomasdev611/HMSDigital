using System;

namespace HMS2SDK.Data.Models
{
    public partial class Logging
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public int Instance { get; set; }
        public DateTime Created { get; set; }
    }
}
