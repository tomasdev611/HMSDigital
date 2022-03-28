using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Notification.Data.Models
{
    public partial class PushNotificationMetadata
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public Guid InstallationId { get; set; }
        public string Platform { get; set; }
        public int? UserId { get; set; }
        public int? CurrentSiteId { get; set; }
    }
}
