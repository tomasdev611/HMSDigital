using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    public abstract class CoreBaseController : ControllerBase
    {
        public CoreBaseController()
        {
        }

        [NonAction]
        public void IgnoreGlobalFilter(GlobalFilters globalFilter)
        {
            HttpContext.Items.Add(Claims.IGNORE_GLOBAL_FILTER, globalFilter);
        }

        [NonAction]
        public override BadRequestObjectResult BadRequest(ModelStateDictionary ModelState)
        {
            var errorState = new ErrorState()
            {
                Errors = ModelState.Values.SelectMany(e => e.Errors.Select(em => em.ErrorMessage))
            };
            return BadRequest(errorState);
        }

        [NonAction]
        public string GetAuthorizationToken()
        {
            return HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        }

    }
}
