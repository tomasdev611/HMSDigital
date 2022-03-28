using HMSDigital.Notification.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Notification.NotificationProcessor
{
    public class TwilioServcie : ISmsService
    {
        private readonly ILogger _logger;

        private readonly PhoneNumberConfig _phoneNumberConfig;

        private readonly TwilioConfig _twilioConfig;
        public TwilioServcie(ILogger<TwilioServcie> logger,
            IOptions<PhoneNumberConfig> phoneNumberOptions,
            IOptions<TwilioConfig> twilioConfig)
        {
            _logger = logger;
            _phoneNumberConfig = phoneNumberOptions.Value;
            _twilioConfig = twilioConfig.Value;
        }

        public async Task<bool> SendSmsAsync(IEnumerable<string> recipients, string bodyContent)
        {

            if (recipients == null)
            {
                return false;
            }

            if (_phoneNumberConfig.IsGatedByAllowedList)
            {
                var allowedPhonePattern = $"^\\+({string.Join("|", _phoneNumberConfig.AllowedList)})$";
                var allowedPhoneRegex = new Regex(allowedPhonePattern, RegexOptions.IgnoreCase);
                recipients = recipients.Where(r => allowedPhoneRegex.IsMatch(r));
            }
            if (recipients.Count() == 0)
            {
                return true;
            }

            TwilioClient.Init(_twilioConfig.KeySid, _twilioConfig.KeySecret);
            foreach (var recipient in recipients)
            {
                var message = await MessageResource.CreateAsync(
                    body: bodyContent,
                    from: new Twilio.Types.PhoneNumber(_twilioConfig.FromNumber),
                    to: new Twilio.Types.PhoneNumber(recipient)
                );
                _logger.LogInformation($"Send Message Id: ({message.Sid}");
            }
            return true;
        }
    }
}
