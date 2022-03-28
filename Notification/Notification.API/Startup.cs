using System;
using System.Net;
using System.Reflection;
using AutoMapper;
using HealthChecks.UI.Client;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.API.HealthCheck;
using HMSDigital.Common.API.Middleware;
using HMSDigital.Common.API.Swagger;
using HMSDigital.Common.BusinessLayer;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data;
using HMSDigital.Core.Data.Repositories;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Notification.BusinessLayer;
using HMSDigital.Notification.BusinessLayer.Interfaces;
using HMSDigital.Notification.Data;
using HMSDigital.Notification.Data.Repositories;
using HMSDigital.Notification.Data.Repositories.Interfaces;
using HMSDigital.Notification.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Sieve.Services;

namespace Notification.API
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

            services.AddDbContext<HMSNotificationContext>(options =>
            {
                options.UseSqlServer(dbConnection, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
            });

            services.AddDbContext<HMSNotificationAuditContext>(options =>
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

            IdentityModelEventSource.ShowPII = true;
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

            var notificationQueue = Configuration.GetSection("NotificationQueue").Get<NotificationQueueConfig>();

            var notificationHubConfigSection = Configuration.GetSection("NotificationHub");
            services.Configure<NotificationHubConfig>(notificationHubConfigSection);

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();

            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IPushNotificationMetaDataRepository, PushNotificationMetaDataRepository>();


            //DI for Core.Data
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IPermissionNounsRepository, PermissionNounsRepository>();
            services.AddScoped<IQueueClient>(queueClient => GetQueueClient(notificationQueue.SenderConnectionString));

            services.AddScoped<ISieveProcessor, SieveProcessor>();

            services.AddScoped<IUserVerifyRepository, UserVerifyRepository>();
            services.AddScoped<IVerifyService, VerifyService>();

            services.AddApplicationInsightsTelemetry();
            services.AddControllers();

            services.AddAuthorizationPolicies();
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
        private IQueueClient GetQueueClient(string connectionString)
        {
            var sbConnectionBuilder = new ServiceBusConnectionStringBuilder(connectionString);
            return new QueueClient(sbConnectionBuilder);
        }
    }
}
