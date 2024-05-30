using DigestLoader_Net6.Infrastructure;
using DigestLoader_Net6.Services;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Старт приложения \"DigestLoader\"!");
        HostStarter.Start(args, DoWork);

        static void DoWork(IServiceProvider servicesProvider, string[] args)
        {
            var configurationProcessor = servicesProvider.GetRequiredService<DigestLoaderService>();
            configurationProcessor.ProcessXmlFiles();
        }
    }
}