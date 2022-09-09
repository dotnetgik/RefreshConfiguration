using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RefreshConfiguration
{
    public class ReloadConfiguration
    {
        private readonly Settings _settings;
        private readonly IConfiguration _config;
        private readonly IConfigurationRefresher _refresher;
        public ReloadConfiguration( IConfigurationRefresher refresher)
        {
            _refresher = refresher ?? throw new ArgumentNullException(nameof(refresher));
        }

        [FunctionName("ReloadConfiguration")]
        public async Task Run([TimerTrigger("30 * * * * *")]TimerInfo myTimer, ILogger log)
        {
         
             _refresher.TryRefreshAsync();
            
            log.LogInformation("Configuration Refreshed");
        }
    }
}