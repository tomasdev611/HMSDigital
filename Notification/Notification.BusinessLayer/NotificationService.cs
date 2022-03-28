using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Notification.BusinessLayer.Interfaces;
using HMSDigital.Notification.Data.Models;
using HMSDigital.Notification.Data.Repositories.Interfaces;
using HMSDigital.Notification.ViewModels;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using NotificationSDK.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Notification.BusinessLayer
{
    public class NotificationService : INotificationService
    {
        private readonly IQueueClient _queueClient;

        private readonly IPushNotificationMetaDataRepository _pushNotificationMetaDataRepository;

        public NotificationService(IQueueClient queueClient,
                                   IPushNotificationMetaDataRepository pushNotificationMetaDataRepository)
        {
            _queueClient = queueClient;
            _pushNotificationMetaDataRepository = pushNotificationMetaDataRepository;
        }

        public async Task PostNotification(NotificationPostRequest notificationPostRequest)
        {
            await EnqueueNotification(notificationPostRequest.RichTextBody,
                                      notificationPostRequest.Subject,
                                      notificationPostRequest.PlainTextBody,
                                      notificationPostRequest.Emails,
                                      notificationPostRequest.PhoneNumbers,
                                      notificationPostRequest.UserIds,
                                      notificationPostRequest.Channels,
                                      notificationPostRequest.Attachments);
        }

        private async Task EnqueueNotification(string emailContent,
                                               string subject,
                                               string smsContent,
                                               IEnumerable<string> emails,
                                               IEnumerable<string> phoneNumbers,
                                               IEnumerable<int> userIds,
                                               IEnumerable<string> channels,
                                               IEnumerable<FileAttachment> attachments)
        {
            var messages = new List<Message>();
            var notificationRequest = new NotificationRequest()
            {
                Content = emailContent,
                Subject = subject
            };
            if (channels.Any(c => c == Channels.EMAIL))
            {
                notificationRequest.Channel = Channels.EMAIL;
                notificationRequest.Recipients = emails;
                notificationRequest.Attachments = attachments;
                var emailMessageBody = JsonConvert.SerializeObject(notificationRequest);
                messages.Add(new Message(Encoding.UTF8.GetBytes(emailMessageBody)));
            }
            if (channels.Any(c => c == Channels.SMS))
            {
                notificationRequest.Content = smsContent;
                notificationRequest.Channel = Channels.SMS;
                notificationRequest.Recipients = phoneNumbers;

                var smsMessageBody = JsonConvert.SerializeObject(notificationRequest);
                messages.Add(new Message(Encoding.UTF8.GetBytes(smsMessageBody)));
            }

            if (channels.Any(c => c == Channels.PUSH))
            {
                var deviceMetaData = await _pushNotificationMetaDataRepository.GetManyAsync(d => d.UserId.HasValue && userIds.Contains(d.UserId.Value));
                if (deviceMetaData != null && deviceMetaData.Count() > 0)
                {
                    var iosRequest = CreatePushNotificationRequest("ios", subject, smsContent, deviceMetaData);
                    if (iosRequest != null)
                    {
                        messages.Add(iosRequest);
                    }

                    var androidRequest = CreatePushNotificationRequest("android", subject, smsContent, deviceMetaData);
                    if (androidRequest != null)
                    {
                        messages.Add(androidRequest);
                    }
                }
            }

            await _queueClient.SendAsync(messages);
        }

        private Message CreatePushNotificationRequest(string platform, string subject, string content, IEnumerable<PushNotificationMetadata> deviceMetaData)
        {
            var platformDevices = deviceMetaData.Where(d => d.Platform.ToLower() == platform.ToLower());
            if (platformDevices != null && platformDevices.Count() > 0)
            {
                var notificationRequest = new NotificationRequest()
                {
                    Content = content,
                    Channel = Channels.PUSH,
                    Recipients = platformDevices.Select(d => d.DeviceId).ToList(),
                    Platform = platform,
                    Subject = subject
                };
                var pushNotificationBody = JsonConvert.SerializeObject(notificationRequest);
                return new Message(Encoding.UTF8.GetBytes(pushNotificationBody));
            }
            return null;
        }
    }
}

