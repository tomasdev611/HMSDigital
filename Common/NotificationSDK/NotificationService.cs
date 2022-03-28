using HMSDigital.Common.BusinessLayer;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.SDK.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationSDK.Constants;
using NotificationSDK.Interfaces;
using NotificationSDK.ViewModels;
using RestEase;
using Stubble.Core.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NotificationSDK
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationConfig _notificationConfig;

        private readonly AWSConfig _awsConfig;

        private readonly INotificationAPI _notificationAPI;

        private readonly ITokenService _tokenService;

        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IOptions<NotificationConfig> notificationOptions,
                                   IOptions<AWSConfig> awsOptions,
                                    ITokenService tokenService,
                                   ILogger<NotificationService> logger)
        {
            _notificationConfig = notificationOptions.Value;
            _awsConfig = awsOptions.Value;
            _notificationAPI = RestClient.For<INotificationAPI>(_notificationConfig.ApiUrl);
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task SendUserCreatedNotification(string userId, string firstName, string lastName)
        {
            var templateParams = new Dictionary<string, object>() {
                { "userId", userId }, 
                { "firstName", firstName }, 
                { "lastName", lastName } 
            };
            var attachments = GetAttachmentsList();
            var userCreatedNotificationRequest = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.EMAIL },
                PlainTextBody = $"A new user account has been created with {userId} for {firstName} {lastName}.",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.UserCreatedTemplate, templateParams),
                Subject = "New user created.",
                Emails = _notificationConfig.AuditEmail,
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(userCreatedNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending user created notification :{ex.Message}");
            }
        }

        public async Task SendSetPasswordNotification(string email, string phoneNumber, string password, IEnumerable<string> channels)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "redirectUri", _awsConfig.RedirectUri },
                { "password", password }
            };
            var attachments = GetAttachmentsList();
            var setPasswordNotificationRequest = new NotificationPostRequest()
            {
                Channels = channels,
                PlainTextBody = $"Hello! Your Hospice Source password has been changed. New password: {password} Login here: {_awsConfig.RedirectUri} " +
                                $"Thank you! Hospice Source Customer Service: 800-299-9277",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.SetPasswordTemplate, templateParams),
                Subject = "Hospice Source Password Reset",
                Emails = new List<string>() { email },
                PhoneNumbers = new List<string>() { phoneNumber },
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(setPasswordNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending Set Password Notification :{ex.Message}");
            }
        }

        public async Task SendPasswordResetLinkNotification(string email, string phoneNumber, string resetLink, IEnumerable<string> channels)
        {
            var substitutionData = new Dictionary<string, object>();
            substitutionData.Add("resetLink", resetLink);
            var attachments = GetAttachmentsList();
            var passwordResetLinkNotificationRequest = new NotificationPostRequest()
            {
                Channels = channels,
                Emails = new List<string>() { email },
                PhoneNumbers = new List<string>() { phoneNumber },
                PlainTextBody = $"Hello! Please visit {resetLink} to reset your password." +
                                $"Thank you! Hospice Source Customer Service: 800-299-9277",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.ResetPasswordTemplate, substitutionData),
                Subject = "Hospice Source Password Reset",
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(passwordResetLinkNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending password Reset link notification :{ex.Message}");
            }
        }

        public async Task SendConfirmationCode(string email, string phoneNumber, string nonce, IEnumerable<string> channels)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "nonce", nonce }
            };
            var attachments = GetAttachmentsList();
            var confirmationCodeNotification = new NotificationPostRequest()
            {
                Channels = channels,
                Emails = new List<string>() { email },
                PhoneNumbers = new List<string>() { phoneNumber },
                PlainTextBody = $"Hello! Your confirmation code is {nonce}." +
                                $"Thank you! Hospice Source Customer Service: 800-299-9277",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.ConfirmationCodeTemplate, templateParams),
                Subject = "Hospice Source Verification Code",
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(confirmationCodeNotification);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending Confirmation code :{ex.Message}");
            }
        }

        public async Task SendPasswordResetOtp(string email, string nonce)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "nonce", nonce }
            };
            var attachments = GetAttachmentsList();
            var passwordResetOtpNotification = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.EMAIL },
                Emails = new List<string>() { email },
                PlainTextBody = $"Hello! Your password reset verification code is {nonce}." +
                                $"Thank you! Hospice Source Customer Service: 800-299-9277",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.PasswordResetOtpTemplate, templateParams),
                Subject = "Hospice Source Password Reset Verification Code",
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(passwordResetOtpNotification);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending reset password OTP :{ex.Message}");
            }
        }

        public async Task SendHopiceCreatedNotification(string email)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "redirectUri", _awsConfig.RedirectUri }
            };
            var attachments = GetAttachmentsList();
            var userCreatedNotificationRequest = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.EMAIL },
                PlainTextBody = $"Hello! Your account for Hospice Source’s Online Ordering Platform has been setup. Login here: {_awsConfig.RedirectUri}" +
                                $"Thank you! Hospice Source Customer Service: 800-299-9277",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.HospiceCreatedTemplate, templateParams),
                Subject = "Hospice Source New User Login Information",
                Emails = new List<string>() { email },
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(userCreatedNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending user created notification :{ex.Message}");
            }
        }

        public async Task SendPartialOrderFulfillmentNotification(string orderNumber, string reason, string driverName, string truckCVN)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "orderNumber", orderNumber },
                { "drivername", driverName },
                { "truckCVN", truckCVN },
                { "dateNow", DateTime.UtcNow.ToString() },
                { "reason", reason }
            };
            var attachments = GetAttachmentsList();
            var partiallyFulfilledNotificationRequest = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.EMAIL },
                PlainTextBody = $"Order# {orderNumber} was partially fufilled by {driverName} on Truck# {truckCVN} at {DateTime.UtcNow} for {reason}",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.PartialOrderFulfillemntTemplate, templateParams),
                Subject = $"Order #{orderNumber} was partially fulfilled",
                Emails = _notificationConfig.AuditEmail,
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(partiallyFulfilledNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending order partially fulfilled notification: {ex.Message}");
            }
        }

        public async Task SendOrderAssignNotification(IEnumerable<string> orderNumbers, int userId, string truckCVN)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "orderNumbers", string.Join(",", orderNumbers) },
                { "truckCVN", truckCVN }
            };
            var attachments = GetAttachmentsList();
            var orderAssignNotificationRequest = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.PUSH },
                PlainTextBody = $"Orders {string.Join(",", orderNumbers)} have been assigned to your Truck# {truckCVN}.",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.OrderAssignedTemplate, templateParams),
                Subject = $"Order {string.Join(",", orderNumbers)} have been assigned",
                UserIds = new List<int>() { userId },
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(orderAssignNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending order assignment notification: {ex.Message}");
            }
        }

        public async Task SendCustomerFeedbackNotification(string email, FeedbackRequest feedbackRequest)
        {
            var templateParams = new Dictionary<string, object>()
            {
                { "feedbackRequestName", feedbackRequest.Name },
                { "feedbackRequestEmail", feedbackRequest.Email },
                { "hospiceLocation", feedbackRequest.HospiceLocation },
                { "feedbackRequestType", feedbackRequest.Type },
                { "feedbackRequestComments", feedbackRequest.Comments },
                { "shortDate", DateTime.UtcNow.Date.ToShortDateString() },
                { "shortTime", DateTime.UtcNow.Date.ToShortTimeString() }
            };
            var attachments = GetAttachmentsList();
            var feedbackNotificationRequest = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.EMAIL },
                PlainTextBody = $"From: {feedbackRequest.Name} Email: {feedbackRequest.Email} Hospice Location: {feedbackRequest.HospiceLocation} Type: {feedbackRequest.Type} {feedbackRequest.Comments} via HMS Digital feedback form {DateTime.UtcNow.Date} at {DateTime.UtcNow.ToShortTimeString()} {DateTime.UtcNow.ToString("tt")}",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.CustomerFeedbackTemplate, templateParams),
                Subject = feedbackRequest.Subject,
                Emails = new List<string>() { email },
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(feedbackNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending feedback notification: {ex.Message}");
            }
        }

        public async Task SendOrderCreatedNotification(string email, OrderNotification orderNotification)
        {
            var isOrderPriority = !string.IsNullOrEmpty(orderNotification.OrderPriority);
            var isOrderNotesAvailable = orderNotification.OrderNotes != null && orderNotification.OrderNotes.Count() > 0;
            var templateParams = new Dictionary<string, object>() {
                { "hospiceLocationName", orderNotification.HospiceLocationName },
                { "memberName", orderNotification.MemberName },
                { "orderStatus", orderNotification.OrderStatus },
                { "orderPriority", orderNotification.OrderPriority },
                { "orderNumber", orderNotification.OrderNumber },
                { "orderDashboardLink", orderNotification.OrderDashboardUrl },
                { "orderItems", orderNotification.OrderItems },
                { "orderNotes", orderNotification.OrderNotes },
                { "isOrderPriority", isOrderPriority },
                { "isOrderNotesAvailable", isOrderNotesAvailable }
            };
            var attachments = GetAttachmentsList();
            var orderCreatedNotificationRequest = new NotificationPostRequest()
            {
                Channels = new List<string>() { Channels.EMAIL },
                PlainTextBody = $@"Thank you for your order! Please find the details of your request below. Requested By: {orderNotification.MemberName}
                                   Hospice Office: {orderNotification.HospiceLocationName} Order Number: {orderNotification.OrderNumber} {orderNotification.OrderDashboardUrl} 
                                   Order Status: {orderNotification.OrderStatus} Order Priority: {orderNotification.OrderPriority};",
                RichTextBody = await RenderHtmlTemplate(EmailTemplateConstants.OrderCreatedTemplate, templateParams),
                Subject = $"Hospice Source Order Confirmation: Order Number# {orderNotification.OrderNumber}.",
                Emails = new List<string>() { email },
                Attachments = attachments
            };
            try
            {
                await SetAuthorizationToken();
                await _notificationAPI.PostNotification(orderCreatedNotificationRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending order created notification :{ex.Message}");
            }
        }

        private async Task SetAuthorizationToken()
        {
            var accessToken = await _tokenService.GetAccessTokenByClientCredentials(_notificationConfig.IdentityClient);
            _notificationAPI.BearerToken = $"Bearer {accessToken}";
        }

        private string GetHtml(string templateId)
        {
            return System.IO.File.ReadAllText(GetBuildDir() + @"\Pages\" + templateId);
        }

        private async Task<string> RenderHtmlTemplate(string templateId, Dictionary<string, object> templateData)
        {
            var template = GetHtml(templateId);
            var stubble = new StubbleBuilder().Build();
            var footerTemplate = new Dictionary<string, string>();
            if(!templateId.Equals("Footer.html"))
            {
                footerTemplate.Add("footer", await RenderHtmlTemplate(@"Footer.html", null));
            }
            return await stubble.RenderAsync(template, templateData, footerTemplate);
        }

        private string GetBuildDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        private IEnumerable<FileAttachment> GetAttachmentsList()
        {
            var attachments = new List<FileAttachment>();
            byte[] hsLogoBytes = File.ReadAllBytes(GetBuildDir() + @"\images\hs-logo.png");
            attachments.Add(new FileAttachment()
            {
                FileName = "hs-logo",
                Content = Convert.ToBase64String(hsLogoBytes),
                Contentid = "hmsLogo",
                Disposition = "inline",
                Type = "image/png"
            });
            return attachments;
        }
    }
}
