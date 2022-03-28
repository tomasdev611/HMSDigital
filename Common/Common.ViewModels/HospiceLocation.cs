namespace HMSDigital.Common.ViewModels
{
    public class HospiceLocation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HospiceId { get; set; }

        public int NetSuiteCustomerId { get; set; }

        public int? SiteId { get; set; }

        public Site Site { get; set; }

        public Address Address { get; set; }

        public PhoneNumberMinimal PhoneNumber { get; set; }
    }
}
