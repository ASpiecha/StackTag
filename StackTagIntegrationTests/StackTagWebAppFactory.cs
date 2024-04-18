using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StackTag.Data;
using Testcontainers.MsSql;

namespace StackTagIntegrationTests
{
    public class StackTagWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public Mock<IConfiguration> Configuration { get; }

        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            //.WithDatabase("TagDatabase") not available for mssql - postgre only
            //.WithUsername("sa")
            .WithPassword("ssmsPass1!")
            .Build();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DataContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(_dbContainer.GetConnectionString());
                });
            });
        }
        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }
        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
