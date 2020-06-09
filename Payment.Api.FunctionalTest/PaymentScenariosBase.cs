using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using Microsoft.Extensions.Configuration;
using Payment.Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;

namespace Payment.Api.FunctionalTest
{
    public class PaymentScenariosBase
    {
        public TestServer CreateServer()
        {
            var configuration = GetConfiguration();
            var hostBuilder = WebHost.CreateDefaultBuilder()
            .UseConfiguration(configuration)
            .UseStartup<Startup>();
            var testServer = new TestServer(hostBuilder);

            using (var scope = testServer.Host.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetService<PaymentDbContext>())
            {
                context.Database.EnsureCreated();
            }
            return testServer;
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
