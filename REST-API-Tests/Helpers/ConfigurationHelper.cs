using Microsoft.Extensions.Configuration;
using System.IO;

namespace REST_API_Tests.Helpers

{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Ensures it reads from the root directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Read appsettings.json

            return builder.Build();
        }
    }
}
