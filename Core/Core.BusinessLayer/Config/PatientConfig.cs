using HMSDigital.Common.SDK.Config;

namespace HMSDigital.Core.BusinessLayer.Config
{
    public class PatientConfig
    {
        public string ApiUrl { get; set; }

        public IdentityClientConfig IdentityClient { get; set; }
    }
}
