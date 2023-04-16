using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace InfrastructureLayer.PresentationLayerConfigurations
{
    public class ServiceCollectionConfigurationSetter
    {
        private readonly IServiceCollection _services;

        public ServiceCollectionConfigurationSetter(
            IServiceCollection services)
        {
            _services = services;
        }

        public ServiceCollectionConfigurationSetter AddRouting()
        {
            _services.AddRouting();
            return this;
        }

        public ServiceCollectionConfigurationSetter AddControllers()
        {
            _services.AddControllers();
            return this;
        }

        public ServiceCollectionConfigurationSetter AddHealthChecks()
        {
            _services.AddHealthChecks();
            return this;
        }

        public ServiceCollectionConfigurationSetter AddHttpContextAccessor()
        {
            _services.AddHttpContextAccessor();
            return this;
        }

        public ServiceCollectionConfigurationSetter AddEndpointsApiExplorer()
        {
            _services.AddEndpointsApiExplorer();
            return this;
        }

        public ServiceCollectionConfigurationSetter AddAutofac()
        {
            _services.AddAutofac();
            return this;
        }


        public ServiceCollectionConfigurationSetter AddCors()
        {
            _services.AddCors();
            return this;
        }

        public ServiceCollectionConfigurationSetter AddMvcCore(
            Assembly presentationAssembly)
        {
            _services.AddMvcCore()
                     .AddApplicationPart(presentationAssembly);
            return this;
        }

        public ServiceCollectionConfigurationSetter AddAuthorization()
        {
            _services
                .AddAuthorization(
                    options =>
                        options.AddPolicy(
                            "Admin", policy
                               => policy.RequireAuthenticatedUser()
                                        .RequireRole("Admin")));
            return this;
        }

        public ServiceCollectionConfigurationSetter AddApiVersioning()
        {
            _services.AddApiVersioning(setup =>
            {
                setup.ReportApiVersions = true;
                setup.AssumeDefaultVersionWhenUnspecified = true;
            });

            _services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return this;
        }

        public ServiceCollectionConfigurationSetter AddSwaggerGen()
        {
            _services.AddRouting();

            _services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(_ => _.FullName);

                var versionProvider =
                        _services
                        .BuildServiceProvider()
                        .GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var apiVersionDescription in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        apiVersionDescription.GroupName,
                        CreateApiInfo(apiVersionDescription));
                }

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Example: \"Bearer token\""
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return this;
        }

        OpenApiInfo CreateApiInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"Phoenix Omega Architecture API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "API Documentation.",
                Contact = new OpenApiContact
                {
                    Name = "Phoenix Omega Architecture Team",
                    Email = "pranceoffire50@gmail.com"
                },
                TermsOfService = new Uri("https://pranceoffire50@gmail.com"),
                License = new OpenApiLicense
                {
                    Name = "GNU License v3.0",
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
