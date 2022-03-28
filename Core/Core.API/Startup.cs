using Audit.AzureTableStorage.ConfigurationApi;
using Audit.AzureTableStorage.Providers;
using Audit.Core;
using Audit.EntityFramework;
using AutoMapper;
using HealthChecks.UI.Client;
using Hms2BillingSDK;
using Hms2BillingSDK.Repositories;
using Hms2BillingSDK.Repositories.Interfaces;
using HMSDigital.Common.API;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.API.Features;
using HMSDigital.Common.API.HealthCheck;
using HMSDigital.Common.API.Middleware;
using HMSDigital.Common.API.Swagger;
using HMSDigital.Common.BusinessLayer;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.Data;
using HMSDigital.Common.SDK.Services;
using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Config;
using HMSDigital.Core.BusinessLayer.Formatter;
using HMSDigital.Core.BusinessLayer.Services;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Sieve;
using HMSDigital.Core.Data;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Patient.FHIR.Models;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.Services;
using HospiceSource.Digital.Patient.SDK;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NotificationSDK;
using NotificationSDK.Interfaces;
using Serilog;
using Sieve.Services;
using System;
using System.Linq;
using System.Reflection;
using WebApiContrib.Core.Formatter.Csv;
using Inventory = HMSDigital.Core.Data.Models.Inventory;
using PatientInventory = HMSDigital.Core.Data.Models.PatientInventory;
using PatientSDK = HospiceSource.Digital.Patient.SDK;
using PatientSDKInterface = HospiceSource.Digital.Patient.SDK.Interfaces;

namespace HMSDigital.Core.API
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
            var dbConnectionString = Configuration.GetConnectionString("HMSDigitalDbConnection");
            var hms2BillingDbConnectionString = Configuration.GetConnectionString("HMS2BillingDbConnection");

            var currentAssembly = Assembly.GetExecutingAssembly();

            //Connection String Reference 
            services.AddDbContext<HMSDigitalAuditContext>(options =>
            {
                options.UseSqlServer(dbConnectionString, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            services.AddDbContext<HMSDigitalContext>(options =>
            {
                options.UseSqlServer(dbConnectionString, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            services.AddDbContext<Hms2BillingContext>(options =>
            {
                options.UseMySql(hms2BillingDbConnectionString, MySqlServerVersion.LatestSupportedServerVersion);
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


            var blobStorageConfigSection = Configuration.GetSection("BlobStorage");
            var blobStorageConfig = blobStorageConfigSection.Get<BlobStorageConfig>();

            services.Configure<AWSConfig>(awsConfigSection);
            services.Configure<BlobStorageConfig>(blobStorageConfigSection);

            var notificationConfigSection = Configuration.GetSection("Notification");
            services.Configure<NotificationConfig>(notificationConfigSection);

            services.Configure<NetSuiteConfig>(Configuration.GetSection("NetSuite"));

            services.Configure<DataBridgeConfig>(Configuration.GetSection("DataBridge"));

            services.Configure<MelissaConfig>(Configuration.GetSection("Melissa"));

            services.Configure<SmartyStreetsConfig>(Configuration.GetSection("SmartyStreets"));

            services.Configure<PatientConfig>(Configuration.GetSection("Patient"));

            services.Configure<SystemLogConfig>(Configuration.GetSection("SystemLog"));

            services.Configure<DBContextConfig>(Configuration.GetSection("ConnectionStrings"));

            services.Configure<FeedbackConfig>(Configuration.GetSection("CustomerFeedback"));

            services.Configure<PatientSDK.Config.PatientConfig>(Configuration.GetSection("Patient"));

            services.Configure<FHIRConfig>(Configuration.GetSection("FHIR"));

            services.Configure<ContractConfig>(Configuration.GetSection("Contract"));

            services.AddScoped<IIdentityService, AWSCognitoService>();

            //core services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAccessService, UserAccessService>();
            services.AddScoped<IVerifyService, VerifyService>();
            services.AddScoped<ISitesService, SitesService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IPermissionsService, PermissionsService>();
            services.AddScoped<IDriversService, DriversService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IHospiceService, HospiceService>();
            services.AddScoped<IHospiceLocationService, HospiceLocationService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IHospiceMemberService, HospiceMemberService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IOrderHeadersService, OrderHeadersService>();
            services.AddScoped<IOrderLineItemsService, OrderLineItemsService>();
            services.AddScoped<IVehiclesService, VehiclesService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemsService, ItemsService>();
            services.AddScoped<IItemCategoriesService, ItemCategoriesService>();
            services.AddScoped<IFileStorageService, AzureBlobStorageService>();
            services.AddScoped<ISiteMemberService, SiteMemberService>();
            services.AddScoped<IEnumerationService, EnumerationService>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddScoped<IDispatchService, DispatchService>();
            services.AddScoped<ITransferRequestsService, TransferRequestsService>();
            services.AddScoped<ISystemService, SystemService>();
            services.AddScoped<IFulfillmentService, FulfillmentService>();
            services.AddScoped<IDbContextFactoryService, DbContextFactoryService>();
            services.AddScoped<ISiteServiceAreaService, SiteServiceAreaService>();
            services.AddScoped<IFeaturesService, FeaturesService>();
            services.AddScoped<IHospiceV2Service, HospiceV2Service>();
            services.AddScoped<IFHIRQueueService<FHIRHospice>, FHIRQueueService<FHIRHospice>>();
            services.AddScoped<IFinanceService, FinanceService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<ITransferOrderService, TransferOrderService>();

            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilters>();
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSorts>();

            //core repositories
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IPermissionNounsRepository, PermissionNounsRepository>();
            services.AddScoped<IPermissionVerbsRepository, PermissionVerbsRepository>();
            services.AddScoped<IUserVerifyRepository, UserVerifyRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IHospiceRepository, HospiceRepository>();
            services.AddScoped<IHospiceLocationRepository, HospiceLocationRepository>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<ICsvMappingRepository, CsvMappingRepository>();
            services.AddScoped<ISitesRepository, SitesRepository>();
            services.AddScoped<IOrderHeadersRepository, OrderHeadersRepository>();
            services.AddScoped<IOrderLineItemsRepository, OrderLineItemsRepository>();
            services.AddScoped<IHospiceMemberRepository, HospiceMemberRepository>();
            services.AddScoped<IFacilityPatientRepository, FacilityPatientRepository>();
            services.AddScoped<IFacilityPatientHistoryRepository, FacilityPatientHistoryRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddScoped<IItemSubCategoryRepository, ItemSubCategoryRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IItemImageFilesRepository, ItemImageFilesRepository>();
            services.AddScoped<ISiteMemberRepository, SiteMemberRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IItemTransferRequestRepository, ItemTransferRequestRepository>();
            services.AddScoped<IDispatchInstructionsRepository, DispatchInstructionsRepository>();
            services.AddScoped<IPatientInventoryRepository, PatientInventoryRepository>();
            services.AddScoped<ISiteServiceAreaRepository, SiteServiceAreaRepository>();
            services.AddScoped<IOrderFulfillmentLineItemsRepository, OrderFulfillmentLineItemsRepository>();
            services.AddScoped<IHospiceLocationMemberRepository, HospiceLocationMemberRepository>();
            services.AddScoped<IItemImageRepository, ItemImageRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISubscriptionItemRepository, SubscriptionItemRepository>();
            services.AddScoped<IHMS2ContractRepository, HMS2ContractRepository>();
            services.AddScoped<IHMS2ContractItemRepository, Hms2ContractItemRepository>();
            services.AddScoped<IUserProfilePictureRepository, UserProfilePictureRepository>();
            services.AddScoped<IFeaturesRepository, FeaturesRepository>();
            services.AddScoped<IOrderNotesRepository, OrderNotesRepository>();
            services.AddScoped<ICreditHoldHistoryRepository, CreditHoldHistoryRepository>();
            services.AddScoped<IDispatchAuditLogRepository, DispatchAuditLogRepository>();
            services.AddScoped<IContractRecordsRepository, ContractRecordsRepository>();
            services.AddScoped<IEquipmentSettingTypeRepository, EquipmentSettingTypeRepository>();

            //DI for NotificationSDK
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<PatientSDKInterface.IPatientService, PatientSDK.PatientService>();
            services.AddScoped<IHms2BillingContractsRepository, Hms2BillingContractsRepository>();
            services.AddScoped<IHms2BillingContractItemsRepository, Hms2BillingContractItemsRepository>();

            services.AddScoped<INetSuiteService, NetSuiteService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAddressStandardizerService, SmartyStreetsAddressVerificationService>();
            services.AddHttpClient<ITokenService, TokenService>();

            //healthcheck
            var healthCheckBuilder = services.AddHealthChecks()
                                             .AddSqlServerHealthChecks(dbConnectionString);
            AddNetSuiteRestletHealthChecks(healthCheckBuilder, services.BuildServiceProvider().GetService<INetSuiteService>());

            services.AddApplicationInsightsTelemetry();
            var csvFormatterOptions = new CsvFormatterOptions()
            {
                CsvDelimiter = ","
            };
            services.AddControllers(options =>
            {
                options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
                options.OutputFormatters.Add(new MappedNameCsvOutputFormatter(csvFormatterOptions));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
            }).AddNewtonsoftJson(options =>
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
               .Include<Users>()
               .Include<Inventory>()
               .Include<UserRoles>()
               .Include<Hospices>()
               .Include<HospiceLocations>()
               .Include<Items>()
               .Include<OrderHeaders>()
               .Include<OrderLineItems>()
               .Include<Sites>()
               .Include<PatientInventory>();

            Audit.Core.Configuration.DataProvider = new AzureTableDataProvider()
            {
                ConnectionString = blobStorageConfig.ConnectionString,
                TableNameBuilder = ev => GetAuditTableName(ev),
                TableEntityMapper = ev => GetAuditEventTableEntity(ev),
            };


            //reload configuration when changed without restarting the application
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

            app.UseRequestResponseLogging();

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

        public static IHealthChecksBuilder AddNetSuiteRestletHealthChecks(IHealthChecksBuilder builder, INetSuiteService netSuiteService)
        {
            return builder.AddCheck("NetSuite", () =>
            {
                var isNetSuiteActive = netSuiteService.CheckSystemStatus().Result;
                if (isNetSuiteActive)
                {
                    return HealthCheckResult.Healthy("NetSuite is Up");
                }
                else
                {
                    return HealthCheckResult.Degraded("NeSuite is Down");
                }
            }, tags: new[] { "netsuite" });
        }


        private static string GetAuditTableName(AuditEvent ev)
        {
            var auditTableName = (ev as AuditEventEntityFramework).EntityFrameworkEvent.Entries.FirstOrDefault().Table;
            if (auditTableName.Equals("UserRoles", StringComparison.OrdinalIgnoreCase))
            {
                auditTableName = "Users";
            }
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
            if (auditLog.AuditAction == "UpdateRoles")
            {
                return true;
            }
            auditLog.User = null;
            AuditContextExtensions.ConfigureAuditContext(auditEvent, eventEntry, auditLog);
            if (auditLog.AuditData == null)
            {
                return false;
            }
            if (eventEntry.Table.Equals("UserRoles", StringComparison.OrdinalIgnoreCase))
            {
                var userRoles = eventEntry.Entity as dynamic;
                auditLog.EntityId = userRoles.UserId;
                auditLog.AuditAction = "UpdateRoles";
            }
            else
            {
                auditLog.EntityId = int.Parse(eventEntry.PrimaryKey.First().Value.ToString());
            }
            return true;

        }

    }
}
