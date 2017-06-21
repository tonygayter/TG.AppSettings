using System.Collections.Generic;
using TG.ConfigSettings.Interfaces;

namespace TG.ConfigSettings
{
    public static class ApplicationSettings
    {
        private static readonly IConfigurationProvider ConfigurationProvider = new ApplicationSettingsConfigConfigurationProvider();

        public static string ExampleValue => ConfigurationProvider.GetConfigurationValue<string>("ExampleValue");

        public static IEnumerable<string> ExampleList => ConfigurationProvider.GetEnumerableConfigurationValue<string>("ExampleList");
    }
}