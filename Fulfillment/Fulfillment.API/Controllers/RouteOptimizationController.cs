using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Fulfillment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using RestEase;

namespace HMSDigital.Fulfillment.API.Controllers
{
    [Produces("application/json")]
    [Route("/api/route-optimizer")]
    [Authorize]
    public class RouteOptimizationController : ControllerBase
    {
        private readonly IBingMapsAPI _bingMapsAPI;

        private readonly ILogger<RouteOptimizationController> _logger;

        private readonly IMapper _mapper;

        private readonly IFeatureManager _featureManager;

        public RouteOptimizationController(ILogger<RouteOptimizationController> logger,
            IOptions<BingMapsConfig> bingMapsOptions,
            IMapper mapper,
            IFeatureManager featureManager)
        {
            var bingMapsConfig = bingMapsOptions.Value;
            _bingMapsAPI = RestClient.For<IBingMapsAPI>(bingMapsConfig.BaseUrl);
            _bingMapsAPI.AccessKey = bingMapsConfig.AccessKey;
            _logger = logger;
            _mapper = mapper;
            _featureManager = featureManager;
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetOptimizedItinerary([FromBody] OptimizeRouteRequest optimizerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var featureIsEnable = await _featureManager.IsEnabledAsync(FeatureFlagConstants.DRIVER_OPTIMIZATION_FEATURE);
            if (!featureIsEnable && optimizerRequest.Agents.Count() > 1)
            {
                return BadRequest("Multiple drivers optimization is not allowed.");
            }
            try
            {
                var response = await _bingMapsAPI.GetOptimizedItinerary(optimizerRequest);
                return Ok(response.GetContent());
            }
            catch (ApiException ex)
            {
                return BadRequest(JsonConvert.DeserializeObject<ErrorResponse>(ex.Content));
            }
        }
    }
}
