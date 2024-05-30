using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace DigestLoader_Net6.Repository
{
    public class MsSqlRepositoryBase
    {
        private string _connectionString = String.Empty;
        protected readonly ILogger _logger;

        public MsSqlRepositoryBase(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Осуществляет запрос или серию запросов к БД, логгируя ошибки и TRACE
        /// </summary>
        /// <param name="action">действие</param>
        /// <param name="LogMessageSuffix">суффикс для сообщения лога</param>
        /// <param name="connectionString">строка подключения к БД</param>
        public void Exec(string LogMessageSuffix, Action<SqlConnection> action)
        {
            _logger.LogTrace($"Начало вызова {LogMessageSuffix}");

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    action(con);

                    _logger.LogTrace($"Конец вызова {LogMessageSuffix}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка вызова {LogMessageSuffix}");
                    throw;
                }
            }
        }
    }

}
