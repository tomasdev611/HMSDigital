using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.Patient.SDK.ViewModels
{
    public class PatientInventoryResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public PatientInventory PatientInventory { get; set; }

        public int Hms2Id { get; set; }
    }
}
