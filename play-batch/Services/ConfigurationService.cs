using System.IO;
using Microsoft.Extensions.Configuration;

namespace batch.Services
{
    public class ConfigurationService
    {
        public static IConfigurationRoot Configuration { get; set; }

        static ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public static string GetValue(string key)
        {
            return Configuration[key] ?? string.Empty;
        }
    }
}
