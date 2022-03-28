using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class GetPatientInventoryWithIssuesResponce
    {
        public PatientInventory PatientInventory { get; set; }
        public Inventory CurrentInventory { get; set; }
        public bool CurrentInventoryIsDeleted { get; set; }
        public IEnumerable<Inventory> NewInventory { get; set; }
    }
}
