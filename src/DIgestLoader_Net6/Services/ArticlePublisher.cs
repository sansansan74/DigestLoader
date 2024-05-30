// Сервис реализует алгориим публикации новостей:
// читает файл, получает данные статей, публикует
using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Services
{
    public class ArticlePublisher
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IInfoRepository _infoRepository;
        private readonly INpktransRepository _npktransRepository;

        private bool _publishOnInfo = false;
        private bool _publishOnNpkTrans = false;

        public ArticlePublisher(ILogger logger, 
            IInfoRepository infoRepository, 
            INpktransRepository npktransRepository, 
            IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _infoRepository = infoRepository ?? throw new ArgumentNullException(nameof(infoRepository));
            _npktransRepository = npktransRepository ?? throw new ArgumentNullException(nameof(npktransRepository));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            InitialPublishFlags();
        }

        private void InitialPublishFlags()
        {
            _publishOnInfo = _config.GetValue<bool>("PublishOn:info");
            _publishOnNpkTrans = _config.GetValue<bool>("PublishOn:npktrans");
        }

        /// <summary>
        /// Метод парсит входной xml-файл, 
        /// проходит по всем статьям и публикует все на сайте info 
        /// и те, у которых 'Отрасль = Железнодорожный транспорт', на сайте npktrans.ru.
        /// </summary>
        /// <param name="fileName"></param>
        public void Publish(string fileName) 
        {
            Digest digest = new();
            digest.Init(fileName);
            
            _logger.LogTrace($"Получено {digest.Articles.Count} статей для публицации");

            foreach (var article in digest.Articles)
            {
                PublishInfo(article);
                PublishNpktrans(article);
            }
        }

        private void PublishNpktrans(Article article)
        {   
            if (_publishOnNpkTrans && article.HasRailTransport())
            {
                _npktransRepository.Publish(article);
                _logger.LogTrace($"Статья опубликована на npktrans.ru, {article.Title}");
            }
        }

        private void PublishInfo(Article article)
        {
            if (_publishOnInfo)
            {
                _infoRepository.Publish(article);
                _logger.LogTrace($"Статья опубликована на info/, {article.Title}");
            }
        }
    }
}
