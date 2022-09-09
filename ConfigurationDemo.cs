using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace RefreshConfiguration
{
    public  class ConfigurationDemo
    {
        private readonly Settings _settings;

        public ConfigurationDemo(IOptionsSnapshot<Settings> settings)
        {
            _settings = settings.Value;
        }

        [FunctionName("ConfigurationDemo")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
           log.LogWarning($"Executing Http Function and loading value is {_settings.Test}");
            
        }
    }
}
