
namespace HMS2SDK.Data.Models
{
    public partial class MarkersUpdate
    {
        public string Zip { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
    }
}
