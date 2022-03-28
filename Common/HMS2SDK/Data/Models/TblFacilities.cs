
namespace HMS2SDK.Data.Models
{
    public partial class TblFacilities
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityState { get; set; }
        public string FacilityZip { get; set; }
        public string FacilityPhone { get; set; }
        public int? HospiceId { get; set; }
    }
}
