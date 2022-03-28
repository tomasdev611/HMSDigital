using HMSDigital.Common.SDK.Config;

namespace HospiceSource.Digital.Patient.SDK.Config
{
    public class PatientConfig
    {
        public string ApiUrl { get; set; }

        public IdentityClientConfig IdentityClient { get; set; }
    }
}