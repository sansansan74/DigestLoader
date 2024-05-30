// Сервис реализует алгориим публикации новостей:
// читает файл, получает данные статей, публикует
using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository;
using DigestLoader_Net6.Repository.Dto;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Services
{
    public class ArticlePublisher
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IIntranetRepository _intranetRepository;
        private readonly IInternetRepository _internetRepository;

        private bool _publishOnIntranet = false;
        private bool _publishOnInternet = false;

        public ArticlePublisher(ILogger logger, 
            IIntranetRepository intranetRepository, 
            IInternetRepository internetRepository, 
            IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _intranetRepository = intranetRepository ?? throw new ArgumentNullException(nameof(intranetRepository));
            _internetRepository = internetRepository ?? throw new ArgumentNullException(nameof(internetRepository));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            InitialPublishFlags();
        }

        private void InitialPublishFlags()
        {
            _publishOnIntranet = _config.GetValue<bool>("PublishOn:Intranet");
            _publishOnInternet = _config.GetValue<bool>("PublishOn:Internet");
        }

        /// <summary>
        /// Метод парсит входной xml-файл, 
        /// проходит по всем статьям и публикует все на сайте intranet 
        /// и те, у которых 'Отрасль = Железнодорожный транспорт', на сайте Internet.
        /// </summary>
        /// <param name="fileName"></param>
        public void Publish(string fileName) 
        {
            Digest digest = new();
            digest.Init(fileName);
            
            _logger.LogTrace($"Получено {digest.Articles.Count} статей для публицации");

            foreach (var article in digest.Articles)
            {
                PublishIntranet(article);
                PublishInternet(article);
            }
        }

        private void PublishInternet(Article article)
        {   
            if (_publishOnInternet && article.HasRailTransport())
            {
                _internetRepository.Publish( new ArticleInternetDTO
                {
                    new_date = article.Date,
                    title = article.Title,
                    annotation = article.GetAnnotation(),
                    body = article.GetBodyForNbktrans()
                });
                _logger.LogTrace($"Статья опубликована на интернет-портале, {article.Title}");
            }
        }

        private void PublishIntranet(Article article)
        {
            if (_publishOnIntranet)
            {
                _intranetRepository.Publish(new ArticleIntranetDTO
                {
                    new_date = article.Date,
                    source = article.Sources,
                    classify = article.Classifies,
                    title = article.Title,
                    body = article.GetBodyWithTag("para")
                });

                _logger.LogTrace($"Статья опубликована на intranet/, {article.Title}");
            }
        }
    }
}
