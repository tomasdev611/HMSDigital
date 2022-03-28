namespace HMSDigital.Common.ViewModels
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Vin { get; set; }

        public string Cvn { get; set; }

        public string Name { get; set; }

        public string LicensePlate { get; set; }

        public decimal Length { get; set; }

        public decimal Capacity { get; set; }

        public bool IsActive { get; set; }

        public int? SiteId { get; set; }

        public string SiteName { get; set; }

        public string CurrentDriverName { get; set; }

        public int CurrentDriverId { get; set; }

        public int? NetSuiteLocationId { get; set; }

        public int? ParentNetSuiteLocationId { get; set; }
    }
}
