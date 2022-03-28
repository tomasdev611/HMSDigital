using System.Collections.Generic;

namespace HMSDigital.Core.BusinessLayer.Config
{
    public class FeatureFlagsOptions
    {
        public ICollection<FeatureOptions> FeatureOptions { get; set; }
    }
    public  class FeatureOptions
    {
        public string Feature { get; set; }
        public bool IsEnabled { get; set; }
    }
}
