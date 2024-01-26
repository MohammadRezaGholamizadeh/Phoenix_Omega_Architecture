using Hangfire;
using InfrastructureLayer.MigrationLayerConfigurations.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InfrastructureLayer.PresentationLayerConfigurations
{
    public class ApplicationBuilderConfigurationSetter
    {
        private readonly IApplicationBuilder _app;

        public ApplicationBuilderConfigurationSetter(IApplicationBuilder app)
        {
            _app = app;
        }

        public ApplicationBuilderConfigurationSetter UseRouting()
        {
            _app.UseRouting();
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseAuthorization()
        {
            _app.UseAuthorization();
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseAuthentication()
        {
            _app.UseAuthentication();
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseEndpointsToConfigHealthCheck()
        {
            _app.UseEndpoints(_ =>
                 {
                     _.MapHealthChecks("/health");
                 });
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseEndpointsToMapControllers()
        {
            _app.UseEndpoints(_ =>
            {
                _.MapHealthChecks("/health");
                _.MapControllers();
            });
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseHttpsRedirection()
        {
            _app.UseHttpsRedirection();
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseHealthChecks()
        {
            _app.UseHealthChecks("/health");
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseCors()
        {
            _app.UseCors(cors =>
            {
                cors.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
            return this;
        }
        public ApplicationBuilderConfigurationSetter UseHangfireDashboard()
        {
            _app.UseHangfireDashboard();
            return this;
        }

        public ApplicationBuilderConfigurationSetter UseSwaggerAndSwaggerUI()
        {

            var provider =
                _app.ApplicationServices
                .GetRequiredService<IApiVersionDescriptionProvider>();
            var environment =
                _app.ApplicationServices
                    .GetRequiredService<IHostEnvironment>();

            if (environment.IsDevelopment())
            {
                _app.UseSwagger(options =>
                {
                    options.RouteTemplate = "docs/{documentName}/swagger.json";
                });

                _app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "docs";

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }

                    options.DocumentTitle = $"Phoenix Omega Architecture API Documentation";
                });
            }
            return this;
        }

        public ApplicationBuilderConfigurationSetter InitializeDataBase()
        {
            var dbInitializer =
                _app.ApplicationServices
                   .GetRequiredService<IMigrationRunner>();

            dbInitializer.Initialize(null);
            return this;
        }
    }
}
