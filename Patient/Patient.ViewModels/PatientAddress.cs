using HMSDigital.Common.ViewModels;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientAddress
    {
       public int AddressTypeId { get; set; }

        public Address Address { get; set; }
    }
}
