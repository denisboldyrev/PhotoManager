using AutoMapper;
using Graph.ArgumentValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Infrastructure.Repository;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.BusinessLogic;
using PhotoManager.Data;
using PhotoManager.GraphQLApi.GraphQLCore;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace PhotoManager.GraphQLApi.Extensions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IPhotoService, PhotoService>();
        }

        public static void ConfigureMapper(this IServiceCollection services)
        {
            var profiles = GetProfiles();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                foreach (var item in profiles.Select(profile => (Profile)Activator.CreateInstance(profile)))
                {
                    cfg.AddProfile(item);
                }
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        private static List<Type> GetProfiles()
        {
            return (from t in typeof(Startup).GetTypeInfo().Assembly.GetTypes()
                    where typeof(Profile).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract
                    select t).ToList();

        }

        public static void ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(
             options =>
                 options.UseSqlServer(
                     configuration.GetConnectionString("DefaultConnection"),
                     x => x.MigrationsAssembly("PhotoManager.Infrastructure.Migrations")));
        }

        public static void ConfigureGraphQL(this IServiceCollection services)
        {
            services.AddScoped<AlbumQuery>();
            services.AddScoped<PhotoQuery>();
            services.AddScoped<AlbumMutation>();
            services.AddScoped<AlbumQuery>();

            services.AddScoped<AlbumQuery>();
            services.AddGraphQLServer()
                .AddAuthorization()
                .AddArgumentValidator()
                .AddQueryType(q => q.Name("Query"))
                .AddType<AlbumQuery>()
                .AddType<PhotoQuery>()
                .AddMutationType(q => q.Name("Mutation"))
                .AddType<AlbumMutation>()
                .AddType<PhotoMutation>();
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            
            services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", opt =>
              {
                  opt.RequireHttpsMetadata = false;
                  opt.Authority = "https://localhost:7001";
                  opt.Audience = "angular-client";
              });
            services.AddAuthorization();
        }
    }
}
