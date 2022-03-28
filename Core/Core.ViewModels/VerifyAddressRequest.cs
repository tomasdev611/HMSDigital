using HMSDigital.Common.ViewModels;

namespace HMSDigital.Core.ViewModels
{
    public class VerifyAddressRequest : AddressMinimal
    {
        public string Country { get; set; }
    }
}
