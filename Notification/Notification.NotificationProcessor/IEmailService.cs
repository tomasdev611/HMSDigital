using NotificationSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.NotificationProcessor
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(IEnumerable<string> recipients, string subject ,string bodyContent ,string htmlBodyContent, IEnumerable<FileAttachment> attachments = null);
    }
}
