using DigestLoader_Net6.Helper;
using Microsoft.Extensions.Configuration;
using NLog;

namespace DigestLoader_Net6.Infrastructure
{
    /// <summary>
    /// Инкапсулирует в себя инфраструктурную работу с:
    /// - чтениеи файла конфигурации
    /// - подключением логгера NLog
    /// - настройку DI-контейнера
    ///
    /// Осуществляет записи в лог о начале и конце работы программы.
    /// Отлавливает неперехваченные исключения и записывает их в лог.
    /// </summary>
    public class HostStarter
    {

        public static void Start(string[] args, Action<IServiceProvider, string[]> action)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("Start");

                CreateInfrastructure(args, action);

                logger.Info("Finish");
            }
            catch (Exception ex)
            {
                logger.Error($"Неперехваченная ошибка [{ex.Message}]");
                throw;
            }
            finally
            {
                LogManager.Flush();
                LogManager.Shutdown();
            }
        }

        private static void CreateInfrastructure(string[] args, Action<IServiceProvider, string[]> action)
        {

#if DEBUG
            IConfigurationRoot config = ConfigurationHelper.ReadJsonConfig("appsettings-debug.json");
#else
           IConfigurationRoot config = ConfigurationHelper.ReadJsonConfig("appsettings.json");
#endif
            IServiceProvider servicesProvider = RegisterServices.BuildDi(config);

            using (servicesProvider as IDisposable)
            {
                action(servicesProvider, args);
            }
        }
    }
}
