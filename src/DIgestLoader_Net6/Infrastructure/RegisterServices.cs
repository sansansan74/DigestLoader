using DigestLoader_Net6.Repository;
using DigestLoader_Net6.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using DigestLoader_Net6.Repository.Interfaces;

namespace DigestLoader_Net6.Infrastructure
{
    /// <summary>
    /// Регистрирует сервисы
    /// </summary>
    public static class RegisterServices
    {
        public static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
                .AddTransient<ArticlePublisher>()
                .AddTransient<IIntranetRepository, IntranetRepository>()
                .AddTransient<IInternetRepository, InternetRepository>()
                .AddTransient<DigestLoaderService>()
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                })
                .AddSingleton(config)
                .AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<DigestLoaderService>>())
                .BuildServiceProvider();
        }
    }
}
