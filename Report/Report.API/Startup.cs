using AutoMapper;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.API.Features;
using HMSDigital.Common.API.Middleware;
using HMSDigital.Common.API.Swagger;
using HMSDigital.Common.BusinessLayer;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data;
using HMSDigital.Core.Data.Repositories;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Report.BusinessLayer.Interfaces;
using HMSDigital.Report.BusinessLayer.Services;
using HMSDigital.Report.BusinessLayer.Sieve;
using HMSDigital.Report.Data;
using HMSDigital.Report.Data.Repositories;
using HMSDigital.Report.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Sieve.Services;
using System;
using System.Reflection;

namespace HMSDigital.Report.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString = Configuration.GetConnectionString("HMSDigitalDbConnection");
            var warehouseConnectionString = Configuration.GetConnectionString("HMSDataLakeDbConnection");

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


            // HMSDataLakeDbConnection
            services.AddDbContext<HMSReportAuditContext>(options =>
            {
                options.UseSqlServer(warehouseConnectionString, b => b.MigrationsAssembly(currentAssembly.GetName().Name));
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

            services.AddApplicationInsightsTelemetry();

            services.AddScoped<IReportService, ReportService>();

            //core repositories
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IPermissionNounsRepository, PermissionNounsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilters>();
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSorts>();

            services.AddScoped<IPaginationService, PaginationService>();

            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IPatientsRepository, PatientsRepository>();

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    });

            services.AddAuthorizationPolicies();

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
            });

            app.UseCustomSwagger(Configuration);

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .CreateLogger();
        }
    }
}
