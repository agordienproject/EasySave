using EasySave.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EasySave.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                c.AddEnvironmentVariables();
            });


            host.ConfigureServices((hostContext, services) =>
            {
                IConfiguration config = hostContext.Configuration;
                
                services.Configure<AppSettings>(config);
            });

            return host;
        }
    }
}
