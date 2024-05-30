using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository.Dto;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DigestLoader_Net6.Repository
{
    internal class IntranetRepositoryFile : IIntranetRepository
    {
        private readonly IConfiguration _config;
        public IntranetRepositoryFile(IConfiguration config)
        {
            _config = config;
        }

        public void Publish(ArticleIntranetDTO article)
        {
            var path = _config["Folders:Intranet"];
            var fileName = Path.Combine(path, $"{Guid.NewGuid}.txt");

            File.AppendAllText(fileName, $"Title:{article.title}\r\nBody:{article.body}");
        }
    }
}
