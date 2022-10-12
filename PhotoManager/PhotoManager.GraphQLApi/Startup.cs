using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoManager.GraphQLApi.Extensions;

namespace PhotoManager.GraphQLApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.ConfigureAuthorization();
            services.ConfigureServices();
            services.ConfigureMapper();
            services.ConfigureDbConnection(Configuration);
            services.ConfigureGraphQL();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UsePlayground();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting().UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
