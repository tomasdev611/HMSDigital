using HMSDigital.Common.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface IAddressStandardizerService
    {
        Task<Address> GetStandardizedAddress(Address address);
        Task<List<SuggestionResponse>> GetAddressSuggestions(Address address, string suite, string maxRecords);
    }
}
