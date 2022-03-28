using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class PatientInventoryWithInvalidItemResponse
    {
        public PatientInventory PatientInventory { get; set; }
        public Item CurrentItem { get; set; }
        public Item NewItem { get; set; }
    }
}
