using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/enumerations")]
    public class EnumerationsController : ControllerBase
    {
        private readonly IEnumerationService _enumerationService;

        public EnumerationsController(IEnumerationService enumerationService)
        {
            _enumerationService = enumerationService;
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetAllEnumerations()
        {
            var enumerations = _enumerationService.GetAllEnumerations();
            return Ok(enumerations);
        }
    }
}
