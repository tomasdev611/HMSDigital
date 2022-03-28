using HMSDigital.Common.ViewModels;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientAddressRequest
    {
       public int AddressTypeId { get; set; }

        public AddressMinimal Address { get; set; }

        public bool DoNotVerifyAddress { get; set; }
    }
}
