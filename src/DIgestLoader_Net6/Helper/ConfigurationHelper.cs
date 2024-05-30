using Microsoft.Extensions.Configuration;

namespace DigestLoader_Net6.Helper
{
    /// <summary>
    /// Считывает конфигурацию из файла
    /// </summary>
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot ReadJsonConfig(string configFileName) =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName, optional: true, reloadOnChange: true)
                .Build();
    }
}
