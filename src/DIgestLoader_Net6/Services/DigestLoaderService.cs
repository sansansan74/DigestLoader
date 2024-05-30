// Сервис читсет все файлы xml из указанной директории
// и передает их на олбработку сервису ArticlePublisher

using DigestLoader_Net6.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Services
{
    internal class DigestLoaderService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly ArticlePublisher _articlePublisher;

        private string _dirIncoming;
        private string _dirOkPath;
        private string _dirErrorPath;
        DirectoryInfo directoryInfo;

        public DigestLoaderService(IConfiguration config, ILogger logger, ArticlePublisher articlePublisher)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _articlePublisher = articlePublisher ?? throw new ArgumentNullException(nameof(articlePublisher));
        }

        private void InitDirectories()
        {
            _dirIncoming = _config["Folders:Incoming"];
            _dirOkPath = _config["Folders:Processed_Ok"];
            _dirErrorPath = _config["Folders:Processed_Error"];

            // Проверка наличия папок Входящие
            directoryInfo = new DirectoryInfo(_dirIncoming);
            if (!directoryInfo.Exists)
            {
                throw new Exception($"Incoming Directory Not Found {directoryInfo}");
            }

            //if (!Directory.Exists(_dirIncoming))
            //    throw new Exception($"Incoming Directory Not Found {directoryInfo}");

        }

        /// <summary>
        /// Вычитывание файлов из папки и вызов обработки
        /// </summary>
        public void ProcessXmlFiles()
        {
            InitDirectories();

            FileInfo[] files = directoryInfo.GetFiles("*.xml");

            _logger.LogTrace($"Получено {files.Length} файлов для обработки");

            foreach (FileInfo file in files)
            {
                _logger.LogTrace($"Обработка файла {file.FullName}");
                ProcessFile(file.FullName);
            }
        }

        /// <summary>
        /// Для файла вызывается метод публикации статей.
        /// Если файл обработан без ошибок, то он перемещается в папку "Processed_Ok", 
        /// иначе в "Processed_Error"
        /// </summary>
        /// <param name="fileName"></param>
        void ProcessFile(string fileName)
        {
            try
            {
                _articlePublisher.Publish(fileName);
                FileMover.MoveAsArchive(fileName, _dirOkPath);
            }
            catch (Exception ex)
            {
                ProcessError(fileName, ex);
            }

        }

        private void ProcessError(string fileName, Exception ex)
        {
            _logger.LogError(ex.Message);

            try
            {
                FileMover.MoveAsArchive(fileName, _dirErrorPath);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
