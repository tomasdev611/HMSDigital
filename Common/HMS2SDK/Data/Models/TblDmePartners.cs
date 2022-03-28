
namespace HMS2SDK.Data.Models
{
    public partial class TblDmePartners
    {
        public int DmeId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyEmails { get; set; }
        public int? LocationId { get; set; }
        public int? DmegroupId { get; set; }
        public string InvoiceEmail { get; set; }
        public int? MultiHospice { get; set; }
        public int? EmailOnly { get; set; }
        public string EmailOnlyUsername { get; set; }
        public string EmailOnlyPassword { get; set; }
        public int? ShareViewDmeId { get; set; }
    }
}
