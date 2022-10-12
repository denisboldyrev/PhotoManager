using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoManager.Data;
using PhotoManager.WebApi.Extensions;

namespace PhotoManager.WebApi.AppStart
{
    public static class ConfigureServicesBase
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureServices();
            services.ConfigureFilters();
            services.ConfigureSwagger();
            services.ConfigureAutoMapper();

            services.AddDbContext<ApplicationContext>(
                options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("PhotoManager.Infrastructure.Migrations")));

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.Authority = "https://localhost:7001";
                    opt.Audience = "angular-client";
                });


            services.AddAuthorization();

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            });
            services.AddControllers();
        }
    }
}
