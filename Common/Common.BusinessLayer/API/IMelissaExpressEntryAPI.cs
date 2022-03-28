using HMSDigital.Common.ViewModels;
using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.API
{
    public interface IMelissaExpressEntryAPI
    {
        [Query("id")]
        string LicenseKey { get; set; }

        [Get("/web/GlobalExpressAddress")]
        Task<StandardizedAddressResponse> GlobalExpressAddress([QueryMap] SortedDictionary<string, string> queryParams);
    }
}
