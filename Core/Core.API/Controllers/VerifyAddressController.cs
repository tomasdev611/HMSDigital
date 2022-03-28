using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/")]
    public class VerifyAddressController : ControllerBase
    {
        private readonly IAddressStandardizerService _addressStandardizerService;
        private readonly ILogger<VerifyAddressController> _logger;
        private readonly IMapper _mapper;

        public VerifyAddressController(
            ILogger<VerifyAddressController> logger,
            IAddressStandardizerService addressStandardizerService,
            IMapper mapper)
        {
            _logger = logger;
            _addressStandardizerService = addressStandardizerService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("verifyaddress")]
        public async Task<IActionResult> VerifyAddress([FromBody] VerifyAddressRequest address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                return Ok(standardizedAddress);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while verify address: {ex.Message} \n Error StackTrace: {ex.StackTrace}");
                throw;
            }
        }


        [HttpPost]
        [Route("verifyaddress/suggestions")]
        public async Task<IActionResult> AddressSuggestions([FromBody] VerifyAddressSuggestionRequest address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var standardizedAddress = await _addressStandardizerService.GetAddressSuggestions(_mapper.Map<Address>(address), string.Empty, address.MaxRecords);
                return Ok(standardizedAddress);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting address suggestions: {ex.Message} \n Error StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
