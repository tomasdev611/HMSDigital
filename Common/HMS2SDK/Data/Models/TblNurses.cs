
namespace HMS2SDK.Data.Models
{
    public partial class TblNurses
    {
        public int NurseId { get; set; }
        public string NurseName { get; set; }
        public string NurseEmail { get; set; }
        public string NursePhone { get; set; }
        public int? HospiceId { get; set; }
    }
}
