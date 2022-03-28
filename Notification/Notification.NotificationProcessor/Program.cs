using System;
using System.Threading.Tasks;
using HMSDigital.Notification.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Notification.NotificationProcessor
{
    class Program
    {
        static async Task Main()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder().Build();
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices();
                    b.AddServiceBus();
                })
                .ConfigureLogging((context, b) =>
                {
                    b.AddConsole();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
                    configuration = config.Build();
                })
                .ConfigureServices((hostingContext, services) =>
                 {
                     var sendGridConfigSection = configuration.GetSection("SendGrid");
                     var twilioConfigSection = configuration.GetSection("Twilio");
                     var emailConfigSection = configuration.GetSection("Email");
                     var phoneNumberconfigSection = configuration.GetSection("PhoneNumber");
                     var pushNotificationConfigSection = configuration.GetSection("NotificationHub");

                     services.Configure<SendGridConfig>(sendGridConfigSection);
                     services.Configure<TwilioConfig>(twilioConfigSection);
                     services.Configure<EmailConfig>(emailConfigSection);
                     services.Configure<PhoneNumberConfig>(phoneNumberconfigSection);
                     services.Configure<NotificationHubConfig>(pushNotificationConfigSection);

                     services.AddScoped<IEmailService, SendGridService>();
                     services.AddScoped<ISmsService, TwilioServcie>();
                     services.AddScoped<IPushNotificationService, PushNotificationService>();
                 });


            var host = hostBuilder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
