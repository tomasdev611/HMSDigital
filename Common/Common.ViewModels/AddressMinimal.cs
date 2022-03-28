namespace HMSDigital.Common.ViewModels
{
    public class AddressMinimal
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public virtual int ZipCode { get; set; }

        public int? Plus4Code { get; set; }
    }
}
