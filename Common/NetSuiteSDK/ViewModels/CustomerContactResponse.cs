namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class CustomerContactResponse
    {
        public bool Success { get; set; }

        public ContactResponse Contact { get; set; }

        public ErrorResponse Error { get; set; }
    }
}
