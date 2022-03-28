using HMSDigital.Common.SDK.Services;
using HMSDigital.Common.SDK.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.EHRIntegration.FHIRServices.Config;

namespace HMSDigital.EHRIntegration.FHIRServices
{
    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()                
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    config.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", false, true)
                          .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                          .AddEnvironmentVariables()
                          .Build();

                })    
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((hostContext, services) =>
                {
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    
                    services.AddAutoMapper(currentAssembly);                  
                    services.AddHttpClient<ITokenService, TokenService>();
                    services.Configure<FHIRConfig>(hostContext.Configuration.GetSection("FHIR"));   
                    services.AddScoped(typeof(IFHIRQueueService<>), typeof(FHIRQueueService<>));
                });
    }
}
