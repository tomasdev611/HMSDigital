using HMSDigital.Common.BusinessLayer.Config;
using Microsoft.Extensions.Configuration;

namespace HMSDigital.Common.API.Features
{
    class FeatureFlagConfigurationSource : IConfigurationSource
    {
        private readonly DbConfigOptions _options;
        public FeatureFlagConfigurationSource(DbConfigOptions options)
        {
            _options = options;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new FeatureFlagConfigurationProvider(_options);
        }
    }
}
