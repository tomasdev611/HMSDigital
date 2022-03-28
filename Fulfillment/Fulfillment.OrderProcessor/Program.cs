using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreSDK;
using CoreSDK.Interfaces;
using HMSDigital.Common.SDK.Services;
using HMSDigital.Common.SDK.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fulfillment.OrderProcessor
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static async Task Main()
        {

            RegisterServices();
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            ILogger<CoreService> logger = loggerFactory.CreateLogger<CoreService>();
            try
            {
                var coreService = _serviceProvider.GetService<ICoreService>();
                var dispatchInstructions = await coreService.GetAllDispatchInstructions();
                if (dispatchInstructions != null && dispatchInstructions.Records != null && dispatchInstructions.Records.Count() > 0)
                {
                    foreach (var dispatchInstruction in dispatchInstructions.Records)
                    {
                        try
                        {
                            if (dispatchInstruction.OrderHeaderId.HasValue)
                            {
                                await coreService.UnAssignOrder(dispatchInstruction.OrderHeaderId.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning($"Error Occurred while un-assigning incomplete order id {dispatchInstruction.OrderHeaderId} :{ex.Message}");
                            continue;
                        }
                    }
                }

                DisposeServices();
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Error Occurred while Unassign order :{ex.Message}");
            }
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

