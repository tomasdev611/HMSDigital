using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using CoreSDK;
using CoreSDK.Interfaces;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.SDK.Services;
using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK;
using HospiceSource.Digital.Patient.SDK.Config;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Patient.FHIR.BusinessLayer.Services;
using CoreConfig = CoreSDK.CoreConfig;

namespace HMSDigital.Patient.FHIRIntegration
{
    public class Program
    {
        public static void Main()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    services.AddAutoMapper(currentAssembly);
                    services.AddScoped<IFHIRService, FHIRService>();
                    services.AddScoped<IPaginationService, PaginationService>();
                    services.AddScoped<IFHIRQueueService<PatientDetail>, FHIRQueueService<PatientDetail>>();
                    services.AddHttpClient<ITokenService, TokenService>();
                    services.AddScoped<IPatientService, PatientService>();
                    services.AddScoped<IFHIRStorageService, FHIRStorageService>();
                    services.AddScoped<IFileStorageService, AzureBlobStorageService>();
                    services.AddScoped<ICoreService, CoreService>();
                    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.Configure<PatientConfig>(configuration.GetSection("Patient"));
                    services.Configure<FHIRConfig>(configuration.GetSection("FHIR"));
                    services.Configure<BlobStorageConfig>(configuration.GetSection("BlobStorage"));
                    services.Configure<CoreConfig>(configuration.GetSection("Core"));
                })
                .Build();

            host.Run();
        }
    }
}