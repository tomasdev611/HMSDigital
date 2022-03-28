using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HMSDigital.Common.API.Swagger
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            //Swagger 
            var swaggerConfig = configuration.GetSection("Swagger").Get<SwaggerConfig>();

            if (swaggerConfig.IsEnabled)
            {
                // Register the Swagger generator, defining one or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HMS Digital API", Version = "v1" });
                });
                services.AddSwaggerGenNewtonsoftSupport();
            }
        }

        public static void UseCustomSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerConfig = configuration.GetSection("Swagger").Get<SwaggerConfig>();
            if (swaggerConfig.IsEnabled)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HMS Digital API v1");
                });
            }
        }
    }

}
