using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using PhotoManager.BusinessLogic;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using PhotoManager.Infrastructure.Repository;
using PhotoManager.WebApi.Filters;
using PhotoManager.WebApi.Mapper;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace PhotoManager.WebApi.Extensions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddTransient<IFileService, FileService>();
            services.AddScoped<IRepository<Album>, AlbumRepository>();
            services.AddScoped<IRepository<Photo>, PhotoRepository>();
        }
        public static void ConfigureFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateEntityExistsAttribute<Album>>();
            services.AddScoped<ValidateEntityExistsAttribute<Photo>>();
            services.AddScoped<ValidateImgExtAndSizeAttribute>();
            services.AddScoped<ValidateUsersPermitionsAttribute<Album>>();
            services.AddScoped<ValidateUsersPermitionsAttribute<Photo>>();
        }
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<AlbumMapperConfiguration>();
                cfg.AddProfile<PhotoMapperConfiguration>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
            public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoManager.WebApi", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2, 
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:7001/connect/token"),
                            AuthorizationUrl = new Uri("https://localhost:7001/connect/authorize"),

                            Scopes = new Dictionary<string, string>
                            {
                                { "api-scope", "api-scope"},
                                { "openid", "openid"},
                                { "profile", "profile"},
                                { "roles", "roles"}
                            }
                        }
                    },
                });
                c.OperationFilter<SwaggerFileOperationFilter>();
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },

                        new List<string>()
                        {
                        }
                    }
                });
                c.IncludeXmlComments(XmlCommentsFilePath);
            });
        }
        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMime = "multipart/form-data";
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
            operation.RequestBody.Content[fileUploadMime].Schema.Properties =
                fileParams.ToDictionary(k => k.Name, v => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                });
        }
    }
}
