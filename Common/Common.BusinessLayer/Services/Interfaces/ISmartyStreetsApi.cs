using HMSDigital.Common.ViewModels;
using RestEase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface ISmartyStreetsApi
    {
        [Header("Referer")]
        string Referer { get; set; }

        [Get("lookup?key={smartySecretKey}&search={addressLine}&prefer_cities={city}&prefer_states={state}")]
        Task<SmartyStreetsSuggestionList> GetSuggestionsAsync([Path] string smartySecretKey, [Path] string addressLine, [Path] string city, [Path] string state);
    }
}
