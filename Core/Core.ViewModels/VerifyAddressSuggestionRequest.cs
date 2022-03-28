namespace HMSDigital.Core.ViewModels
{
    public class VerifyAddressSuggestionRequest : VerifyAddressRequest
    {
        public string Suite { get; set; }
        public  string MaxRecords { get; set; }
    }
}
