using System;
using AutoMapper;
using HMSDigital.Common.API.Swagger;
using HMSDigital.Common.BusinessLayer;
using HMSDigital.Common.API.HealthCheck;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.Data.Repositories;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sieve.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using HMSDigital.Common.API.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using HMSDigital.Patient.BusinessLayer;
using HMSDigital.Patient.BusinessLayer.Interfaces;
using HMSDigital.Patient.Data;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using HMSDigital.Patient.Data.Repositories;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Patient.Data.Models;
using HMSDigital.Common.API;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Patient.BusinessLayer.Sieve;
using HMSDigital.Patient.BusinessLayer.Config;
using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.Common.SDK.Services;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Patient.FHIR.BusinessLayer.Services;
using Microsoft.Extensions.Primitives;
using Microsoft.FeatureManagement;
using HMSDigital.Common.API.Features;
using HMSDigital.Patient.FHIR.Models;
using HMSDigital.Common.Data;
using Audit.Core;
using Audit.EntityFramework;
using System.Linq;
using Audit.AzureTableStorage.ConfigurationApi;
using Audit.AzureTableStorage.Providers;
using HMSDigital.Common.ViewModels;

namespace HMSDigital.Patient.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnection = Configuration.GetConnectionString("HMSDigitalDbConnection");
            var currentAssembly = Assembly.GetExecutingAssembly();

            //healthcheck
            services.AddHealthChecks()
                .AddSqlServerHealthChecks(dbConnection);

            //Connection String Reference 
            services.AddDbContext<HMSDigitalContext>(options =>
            {
                options.UseSqlServer(dbConnection, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            services.AddDbContext<HMSDigitalAuditContext>(options =>
            {
                options.UseSqlServer(dbConnection, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            services.AddDbContext<HMSPatientContext>(options =>
            {
                options.UseSqlServer(dbConnection, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            services.AddDbContext<HMSPatientAuditContext>(options =>
            {
                options.UseSqlServer(dbConnection, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            //CORS services
            services.AddCors(options =>
            {
                options.AddPolicy("AllowWebAppOrigin",
                builder => builder
                .SetPreflightMaxAge(TimeSpan.FromMinutes(100))
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });

            var awsConfigSection = Configuration.GetSection("AWS");
            var awsConfig = awsConfigSection.Get<AWSConfig>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                    options.Authority = $"https://cognito-idp.{awsConfig.Region}.amazonaws.com/{awsConfig.UserPoolId}";
                });

            services.AddSwagger(Configuration);

            services.AddAutoMapper(currentAssembly);

            services.Configure<AWSConfig>(awsConfigSection);
            services.Configure<MelissaConfig>(Configuration.GetSection("Melissa"));
            services.Configure<FHIRConfig>(Configuration.GetSection("FHIR"));
            services.Configure<NetSuiteConfig>(Configuration.GetSection("NetSuite"));
            services.Configure<DataBridgeConfig>(Configuration.GetSection("DataBridge"));
            services.Configure<CoreConfig>(Configuration.GetSection("Core"));
            services.Configure<SmartyStreetsConfig>(Configuration.GetSection("SmartyStreets"));

            var blobStorageConfigSection = Configuration.GetSection("BlobStorage");
            var blobStorageConfig = blobStorageConfigSection.Get<BlobStorageConfig>();

            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientV2Service, PatientV2Service>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddScoped<IFHIRService, FHIRService>();
            services.AddScoped<IFHIRQueueService<FHIRPatientDetail>, FHIRQueueService<FHIRPatientDetail>>();
            services.AddHttpClient<ITokenService, TokenService>();

            services.AddScoped<IAddressStandardizerService, SmartyStreetsAddressVerificationService>();

            services.AddScoped<IPatientsRepository, PatientsRepository>();
            services.AddScoped<IPatientNotesRepository, PatientNotesRepository>();
            services.AddScoped<IPatientAddressRepository, PatientAddressRepository>();
            services.AddScoped<IAddressesRepository, AddressesRepository>();
            services.AddScoped<IPatientMergeHistoryRepository, PatientMergeHistoryRepository>();

            //DI for Core.Data
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IPermissionNounsRepository, PermissionNounsRepository>();
            services.AddScoped<IHospiceRepository, HospiceRepository>();

            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilters>();
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSorts>();

            services.AddApplicationInsightsTelemetry();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

            services.AddAuthorizationPolicies();

            services.ConfigureAuditLogs();

            Audit.EntityFramework.Configuration.Setup()
              .ForAnyContext(config => config
                  .IncludeEntityObjects()
                  .AuditEventType("{context}:{database}"))
               .UseOptIn()
               .Include<PatientNotes>()
               .Include<PatientDetails>();

            Audit.Core.Configuration.DataProvider = new AzureTableDataProvider()
            {
                ConnectionString = blobStorageConfig.ConnectionString,
                TableNameBuilder = ev => GetAuditTableName(ev),
                TableEntityMapper = ev => GetAuditEventTableEntity(ev),
            };


            ChangeToken.OnChange(() => Configuration.GetReloadToken(), () => { });

            //Feature Flags integration
            services.AddFeatureManagement(Configuration).UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //CORS
            app.UseCors("AllowWebAppOrigin");

            app.UseAuthentication();

            app.UseSerilogRequestLogging();

            app.UseSetPermissionClaims();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseCustomSwagger(Configuration);

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .CreateLogger();
        }


        private static string GetAuditTableName(AuditEvent ev)
        {
            var auditTableName = (ev as AuditEventEntityFramework).EntityFrameworkEvent.Entries.FirstOrDefault().Table;
            return auditTableName + "AuditLog";
        }

        private static AuditEventTableEntity GetAuditEventTableEntity(AuditEvent ev)
        {
            var entries = (ev as AuditEventEntityFramework).EntityFrameworkEvent.Entries;
            var auditLog = new AuditLog();
            foreach (var entry in entries)
            {
                HandleEventLog(ev, entry, auditLog);
            }

            return new Common.ViewModels.AuditLogAzureTable()
            {
                AuditEvent = JsonConvert.SerializeObject(auditLog.AuditData),
                RowKey = DateTime.UtcNow.Ticks.ToString(),
                PartitionKey = auditLog.EntityId.ToString(),
                AuditAction = auditLog.AuditAction,
                UserId = auditLog.UserId,
                EntityId = auditLog.EntityId,
                AuditDate = auditLog.AuditDate,
                ClientIpAddress = auditLog.ClientIpaddress
            };
        }

        internal static bool HandleEventLog(AuditEvent auditEvent, EventEntry eventEntry, object auditLogInput)
        {
            var auditLog = auditLogInput as dynamic;

            AuditContextExtensions.ConfigureAuditContext(auditEvent, eventEntry, auditLog);

            if (auditLog.AuditData == null)
            {
                return false;
            }
            auditLog.EntityId = int.Parse(eventEntry.PrimaryKey.First().Value.ToString());
            return true;
        }

    }
}
