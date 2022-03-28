using System.Collections.Generic;

namespace HMSDigital.Patient.ViewModels.FHIR
{
    public class Organization
    {
        public string ResourceType { get; set; }
        public List<Extension> Extension { get; set; }
        public List<Identifier> Identifier { get; set; }
        public string Name { get; set; }
        public List<OrgTelecom> Telecom { get; set; }
        public List<OrgAddress> Address { get; set; }
        public PartOf PartOf { get; set; }
    }
}
