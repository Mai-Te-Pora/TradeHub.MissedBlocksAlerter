using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using TradeHub.MissedBlocksAlerter.Services;
using TradeHub.MissedBlocksAlerter.Settings;

namespace TradeHub.MissedBlocksAlerter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(cfg => cfg.AddConsole().SetMinimumLevel(LogLevel.Debug));

                    var config = BuildConfiguration();
                    services.Configure<AppSettings>(config);

                    services.AddScoped<PushbulletService>();
                    services.AddScoped<TradescanService>();

                    services.AddHostedService<AppWorkerService>();
                })
                .RunConsoleAsync();
        }

        static IConfigurationRoot BuildConfiguration() => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
    }
}
