using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/features")]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeaturesService _featuresService;

        private readonly ILogger<FeaturesController> _logger;

        public FeaturesController(IFeaturesService featuresService, ILogger<FeaturesController> logger)
        {
            _featuresService = featuresService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetFeatures([FromQuery] SieveModel sieveModel)
        {

            var features = await _featuresService.GetAllFeatures(sieveModel);
            return Ok(features);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("{featureName}")]
        public async Task<IActionResult> GetFeatureByName([FromRoute] string featureName)
        {
            try
            {
                var feature = await _featuresService.GetFeatureByName(featureName);
                return Ok(feature);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting feature with name ({featureName}) :{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("")]
        public async Task<IActionResult> UpdateFeature([FromBody] Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(feature.Name))
            {
                return BadRequest($"Feature name cannot be empty");
            }

            try
            {
                var updatedFeature = await _featuresService.UpdateFeature(feature);
                
                return Ok(updatedFeature);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing members with issues:{ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// This is an example of how to validate if a method is enabled or not based on FeatureFlag
        /// </summary>
        /// <returns>CRISP_IN_APP_CHAT enabled when is enabled,
        /// Returns 404 when is disabled
        /// </returns>
        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [FeatureGate(FeatureFlagConstants.CRISP_IN_APP_CHAT)]
        [Route("isEnabled")]
        public IActionResult ValidateCrispInAppChat()
        {
            return Ok("CRISP_IN_APP_CHAT enabled");
        }
    }
}
