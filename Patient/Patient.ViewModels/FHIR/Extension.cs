using System;

namespace HMSDigital.Patient.ViewModels.FHIR
{
    public class Extension
    {
        public string Url { get; set; }
        public string ValueString { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
    }
}
