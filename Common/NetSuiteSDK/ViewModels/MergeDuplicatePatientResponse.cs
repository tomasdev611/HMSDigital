using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class MergeDuplicatePatientResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
