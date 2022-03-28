using HMSDigital.Common.ViewModels;
using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.API
{
    public interface IMelissaPersonatorAPI
    {
        [Query("id")]
        string LicenseKey { get; set; }

        [Get("/ContactVerify/doContactVerify")]
        Task<StandardizedAddressResponse> DoContactVerify([QueryMap] SortedDictionary<string, string> queryParams);
    }
}
