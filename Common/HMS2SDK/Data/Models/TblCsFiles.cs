using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblCsFiles
    {
        public int CsfileId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Filename { get; set; }
        public DateTime? Submitted { get; set; }
        public int? UserId { get; set; }
    }
}
