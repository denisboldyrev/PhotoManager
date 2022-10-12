using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoManager.Infrastructure.Repository;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Interfaces.Services;
using AutoMapper;
using PhotoManager.BusinessLogic;
using PhotoManager.Data.Seed;
using PhotoManager.Data;
using PhotoManager.WebApp.Mapper;
using PhotoManager.WebApp.Filters;
using PhotoManager.Core.Models;

namespace PhotoManager.WebApp
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();   
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateAlbumExistsAttribute>();
            services.AddScoped<ValidatePhotoExistsAttribute>();
            services.AddScoped<IRepository<Album>, AlbumRepository>();
            services.AddScoped<IRepository<Photo>, PhotoRepository>();
            services.AddScoped<ValidateUsersPermitionsAttribute<Album>>();
            services.AddScoped<ValidateUsersPermitionsAttribute<Photo>>();
            services.AddScoped<ValidateUserAttribute>();
            services.AddScoped<SeedAppDb>();

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<AlbumMapperConfiguration>();
                cfg.AddProfile<PhotoMapperConfiguration>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<ApplicationContext>(
                 options =>
                     options.UseSqlServer(
                         Configuration.GetConnectionString("DefaultConnection"),
                         x => x.MigrationsAssembly("PhotoManager.Infrastructure.Migrations")));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:7001";
            options.ClientId = "mvc";
            options.ClientSecret = "secret";
            options.SaveTokens = true;
            options.ResponseType = "code";
            options.GetClaimsFromUserInfoEndpoint = true;
            options.Scope.Add("roles");
        });

            services.AddControllersWithViews();
        }
     
        public void Configure(IApplicationBuilder app)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            app.UseExceptionHandler("/Home/Error");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
       
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
