using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class FeaturesService : IFeaturesService
    {
        private readonly IFeaturesRepository _featuresRepository;

        private readonly IMapper _mapper;

        private readonly IPaginationService _paginationService;

        private readonly IConfiguration _configuration;


        public FeaturesService(IFeaturesRepository featuresRepository,
                               IPaginationService paginationService,
                               IMapper mapper,
                               IConfiguration configuration)
        {
            _featuresRepository = featuresRepository;
            _paginationService = paginationService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<PaginatedList<ViewModels.Feature>> GetAllFeatures(SieveModel sieveModel)
        {
            _featuresRepository.SieveModel = sieveModel;
            var totalRecords = await _featuresRepository.GetCountAsync(s => true);
            var featuresModel = await _featuresRepository.GetAllAsync();
            var features = _mapper.Map<IEnumerable<ViewModels.Feature>>(featuresModel);
            return _paginationService.GetPaginatedList(features, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<Feature> GetFeatureByName(string featureName)
        {
            var featureModel = await _featuresRepository.GetAsync(f => f.Name.ToLower().Equals(featureName.ToLower()));
            return _mapper.Map<Feature>(featureModel);
        }

        public async Task<Feature> UpdateFeature(Feature feature)
        {
            var existingFeature = await _featuresRepository.GetAsync(f => f.Name.ToLower().Equals(feature.Name.ToLower()));
            if (existingFeature == null)
            {
                throw new KeyNotFoundException($"Could not found feature {feature.Name}");
            }

            await UpdateFeature(feature, existingFeature);

            UpdateFeatureConfiguration(feature);

            return feature;
        }

        private void UpdateFeatureConfiguration(Feature feature)
        {
            var config = _configuration.GetSection(FeatureFlagConstants.FEATURE_MANAGEMENT);
            var values = config.Get<Dictionary<string, string>>();

            if (feature != null && values?[feature.Name] != null)
            {
                values[feature.Name] = feature.IsEnabled.ToString();
            }
        }

        private async Task UpdateFeature(Feature feature, Features existingFeature)
        {
            var updatedFeature = _mapper.Map(feature, existingFeature);
            await _featuresRepository.UpdateAsync(updatedFeature);
        }
    }
}
