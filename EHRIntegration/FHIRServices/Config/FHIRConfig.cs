using HMSDigital.Common.SDK.Config;

namespace HMSDigital.EHRIntegration.FHIRServices.Config
{
    public class FHIRConfig
    {
        public string ApiUrl { get; set; }       
        public string QueueConnectionString { get; set; }
        public IdentityClientConfig IdentityClient { get; set; }
    }
}
