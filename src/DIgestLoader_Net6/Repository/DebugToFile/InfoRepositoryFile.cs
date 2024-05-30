using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DigestLoader_Net6.Repository
{
    internal class InfoRepositoryFile : IInfoRepository
    {
        private readonly IConfiguration _config;
        public InfoRepositoryFile(IConfiguration config)
        {
            _config = config;
        }

        public void Publish(Article article)
        {
            var path = _config["Folders:Info"];
            var fileName = Path.Combine(path, $"{Guid.NewGuid}.txt");

            File.AppendAllText(fileName, $"Title:{article.Title}\r\nBody:{article.GetBodyWithTag("para")}");
        }
    }
}
