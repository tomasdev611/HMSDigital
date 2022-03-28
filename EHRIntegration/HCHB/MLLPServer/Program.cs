using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.Config;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.Contracts;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.AzureServiceBus.Config;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.AzureServiceBus;

namespace HMSDigital.EHRIntegration.HCHB.MLLPServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var processModule = Process.GetCurrentProcess().MainModule;
            
            if (processModule != null)
            {
                var pathToExe = processModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }            

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService() 
                .ConfigureAppConfiguration((hostContext, config) => 
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())                          
                          .AddJsonFile("appsettings.json", false, true)
                          .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true, true)
                          .AddEnvironmentVariables()
                          .Build();

                })
                .ConfigureServices((hostContext, services) =>
                {                   
                    #region MLLP Config

                    var mllpConfig = hostContext.Configuration.GetSection("MLLP");

                    services.Configure<MLLPConfig>(mllpConfig);                    

                    #endregion

                    #region AzureServiceBus Config

                    var asbSettings = hostContext.Configuration.GetSection("AzureServiceBus");

                    services.Configure<AzureServiceBusConfig>(asbSettings);
                    
                    services.AddTransient<IAzureServiceBusClient, AzureServiceBusClient>();

                    #endregion

                    #region services                    
                                       
                    services.AddHostedService<MLLPServer>();                                       

                    #endregion
                });
    }
}
