using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.ViewModels;
using HMSDigital.Notification.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Test.MockProvider
{
    class MockViesModels
    {
        public NotificationPostRequest GetNotificationPostRequest()
        {
            return new NotificationPostRequest()
            {
                Emails = new List<string>() { "test@example.com" },
                PlainTextBody = "test body",
                RichTextBody = "<h2>test body</h2>",
                PhoneNumbers = new List<string>() { "+11234567890" },
                Subject = "test subject",
                Channels = new List<string>() { Channels.EMAIL, Channels.SMS }
            };
        }

        public IEnumerable<Email> GetEmails()
        {
            return new List<Email>()
            {
                new Email()
                {
                    EmailAddress = "test@example.co.in",
                    EmailType = "WORK",
                    IsPrimary = true
                }
            };
        }

        public IEnumerable<PhoneNumberRequest> GetPhoneNumbers()
        {
            return new List<PhoneNumberRequest>()
            {
                new PhoneNumberRequest()
                {
                    CountryCode = 91,
                    Number = 1234567890,
                    NumberType = "WORK",
                    IsPrimary = true
                }
            };
        }
    }
}
