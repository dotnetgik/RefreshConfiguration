using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using RefreshConfiguration;
using System;

[assembly: WebJobsStartup(typeof(Program))]
namespace RefreshConfiguration
{

    public class Program : FunctionsStartup
    {
        public static IConfigurationRefresher _configRefresher;

        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddAzureAppConfiguration();

            builder.Services.AddSingleton(_configRefresher);

            builder.Services.AddOptions<Settings>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(Settings.Name).Bind(settings);
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {

            builder.ConfigurationBuilder.AddUserSecrets<Program>();

            var configuration = builder.ConfigurationBuilder.Build();


            builder.ConfigurationBuilder.AddAzureAppConfiguration(opt =>
             {
                 var connectionString = configuration.GetConnectionString("AzureAppConfigurationEndPoint");
                 opt.Connect(connectionString).ConfigureRefresh(options =>
                {
                    options.Register("refresh", refreshAll: true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(15));
                });

                 _configRefresher = opt.GetRefresher();
             });

        }

    }

}

