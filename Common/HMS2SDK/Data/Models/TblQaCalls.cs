using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblQaCalls
    {
        public int QacallId { get; set; }
        public int? UserId { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string ContactInfo { get; set; }
        public int? EquipFunction { get; set; }
        public int? EquipClean { get; set; }
        public int? EquipTimely { get; set; }
        public int? EquipAgent { get; set; }
        public int? AgentRating { get; set; }
        public int? InstRating { get; set; }
        public int? TechRating { get; set; }
        public int? HsRating { get; set; }
        public string Comments { get; set; }
        public int? CommRating { get; set; }
        public int? NotCompleted { get; set; }
    }
}
