using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Server.Auth;
using Warehouse.Server.Data;

namespace Warehouse.Server.Tests.Utils
{
    public class TestStartup
    {
        public TestStartup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });
            services.AddMediatR(typeof(Startup));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            services.AddTransient<IDataContext>(s => s.GetService<DataContext>());
        }

        public void Configure(IApplicationBuilder app, DataContext dataContext)
        {
            // Initialize database/migrations if not created
            dataContext.Database.EnsureCreated();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
