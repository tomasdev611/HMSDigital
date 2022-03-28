using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class PatientInventoryResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public PatientInventory PatientInventory { get; set; }

        public int Hms2Id { get; set; }
    }
}
