using System;
using System.IO;
using System.Threading.Tasks;
using CoreSDK;
using CoreSDK.Interfaces;
using HMSDigital.Common.SDK.Services;
using HMSDigital.Common.SDK.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.SubscriptionProcessor
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static async Task Main()
        {
            RegisterServices();
            var logger = _serviceProvider.GetService<ILogger<Program>>();
            var coreService = _serviceProvider.GetService<ICoreService>();
            var hospices = await coreService.GetAllHospices(new Sieve.Models.SieveModel());
            foreach(var hospice in hospices.Records)
            {
                try
                {
                    await coreService.UpsertHospiceSubscriptions(hospice.Id);
                }
                catch(Exception ex)
                {
                    logger.LogError($"Error Occurred while upserting hospice subscriptions with Id ({hospice.Id}) :{ex.Message}");
                }
            }
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var hostBuilder = Host.CreateDefaultBuilder()
               .ConfigureServices((hostingContext, services) =>
               {
                   var coreConfigSection = configuration.GetSection("Core");
                   services.Configure<CoreConfig>(coreConfigSection);

                   var contractConfigSection = configuration.GetSection("Contract");
                   services.Configure<ContractConfig>(contractConfigSection);

                   services.AddScoped<ICoreService, CoreService>();
                   services.AddHttpClient<ITokenService, TokenService>();
                   _serviceProvider = services.BuildServiceProvider();
               });

            var host = hostBuilder.Build();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
