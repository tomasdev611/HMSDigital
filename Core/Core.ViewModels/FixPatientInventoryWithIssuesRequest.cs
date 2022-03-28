namespace HMSDigital.Core.ViewModels
{
    public class FixPatientInventoryWithIssuesRequest
    {
        public int PatientInventoryId { get; set; }
        public int? NewInventoryId { get; set; }
        public int? NewItemId { get; set; }
    }
}
