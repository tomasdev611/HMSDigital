using HMSDigital.Notification.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Notification.BusinessLayer.Interfaces
{
    public interface INotificationService
    {
        Task PostNotification(NotificationPostRequest notificationPostRequest);
    }
}
