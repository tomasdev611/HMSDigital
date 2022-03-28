using System;

namespace HMSDigital.Notification.ViewModels
{
    public class DeviceRegister
    {
        /// <summary>
        /// Specify the device platform. i.e Android, IOS
        /// Using this set device installation platform. i.e Apns for IOS and Fcm for Android 
        /// </summary>
        public string Platform { get; set; }

        public Guid InstallationId { get; set; }

        public string DeviceId { get; set; }

        public int? CurrentSiteId { get; set; }
    }
}
