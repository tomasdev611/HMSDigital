using NotificationSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSDK.Interfaces
{
    public interface INotificationService
    {
        Task SendUserCreatedNotification(string userId, string firstName, string lastName);

        Task SendSetPasswordNotification(string email, string phoneNumber, string password, IEnumerable<string> channels);

        Task SendPasswordResetLinkNotification(string email, string phoneNumber, string resetLink, IEnumerable<string> channels);

        Task SendConfirmationCode(string email, string phoneNumber, string nonce, IEnumerable<string> channels);

        Task SendHopiceCreatedNotification(string email);

        Task SendPartialOrderFulfillmentNotification(string orderNumber, string reason, string driverName, string truckCVN);

        Task SendOrderAssignNotification(IEnumerable<string> orderNumbers, int userId, string truckCVN);

        Task SendCustomerFeedbackNotification(string email, FeedbackRequest feedbackRequest);

        Task SendPasswordResetOtp(string email, string nonce);

        Task SendOrderCreatedNotification(string email, OrderNotification orderNotification);
    }
}
