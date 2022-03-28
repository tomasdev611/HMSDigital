using System;

namespace HMS2SDK.Data.Models
{
    public partial class Semaphore
    {
        public int Id { get; set; }
        public string Process { get; set; }
        public DateTime TimeStarted { get; set; }
    }
}
