using HMSDigital.Notification.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationSDK.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Notification.NotificationProcessor
{
    public class SendGridService : IEmailService
    {
        private readonly ILogger _logger;

        private readonly SendGridConfig _sendGridConfig;

        private readonly EmailConfig _emailConfig;

        public SendGridService(ILogger<SendGridService> logger,
                                IOptions<EmailConfig> emailConfigOptions,
                            IOptions<SendGridConfig> sendGridConfig)
        {
            _logger = logger;
            _emailConfig = emailConfigOptions.Value;
            _sendGridConfig = sendGridConfig.Value;
        }

        public async Task<bool> SendEmailAsync(IEnumerable<string> recipients, string subject, string bodyContent, string htmlBodyContent, IEnumerable<FileAttachment> attachments = null)
        {
            if (recipients == null)
            {
                return false;
            }

            if (_emailConfig.IsGatedByAllowedList)
            {
                var allowedEmailsPattern = $"^({string.Join("|", _emailConfig.AllowedList)})$";
                var allowedEmailsRegex = new Regex(allowedEmailsPattern, RegexOptions.IgnoreCase);
                recipients = recipients.Where(r => allowedEmailsRegex.IsMatch(r));
            }
            if (recipients.Count() == 0)
            {
                return true;
            }


            var client = new SendGridClient(_sendGridConfig.APIKey);

            var sendGridMessage = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridConfig.DefaultFromEmail),
                Subject = subject,
                PlainTextContent = bodyContent,
                HtmlContent = htmlBodyContent
            };
            var emails = new List<EmailAddress>();

            foreach (var email in recipients)
            {
                emails.Add(new EmailAddress()
                {
                    Email = email
                });
            }
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var mailAttachment = new Attachment()
                    {
                        Content = attachment.Content,
                        Type = attachment.Type,
                        Filename = attachment.FileName,
                        Disposition = attachment.Disposition,
                        ContentId = attachment.Contentid
                    };
                    sendGridMessage.AddAttachment(mailAttachment);
                }
            }
            sendGridMessage.AddTos(emails);
            var response = await client.SendEmailAsync(sendGridMessage);
            _logger.LogInformation($"send email status : {response.StatusCode.ToString()}");
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }
            return false;
        }
    }
}
