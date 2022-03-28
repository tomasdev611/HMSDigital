using System;
namespace MobileApp.Models
{
    public class DeviceRegisterRequest
    {
        public string Platform { get; set; }

        public Guid InstallationId { get; set; }

        public string DeviceId { get; set; }

        public int? CurrentSiteId { get; set; }
    }
}
