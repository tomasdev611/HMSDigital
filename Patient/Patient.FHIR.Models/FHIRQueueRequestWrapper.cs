namespace HMSDigital.Patient.FHIR.Models
{
    public class FHIRQueueRequestWrapper
    {
        public string ResourceType { get; set; }
        public object Resource { get; set; }
        public int RequestType { get; set; }
    }
}
