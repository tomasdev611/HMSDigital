using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IFeaturesService
    {
        Task<PaginatedList<Feature>> GetAllFeatures(SieveModel sieveModel);
        Task<Feature> GetFeatureByName(string featureName);
        Task<Feature> UpdateFeature(Feature feature);
    }
}
