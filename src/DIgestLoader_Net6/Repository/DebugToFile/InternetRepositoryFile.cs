/*
Сигнатура вызываемой хранимой процедуры
proc [dbo].[san_add_digest_new]
	@new_date 	datetime,	-- дата новости 		
	@title 		varchar(1024), 	-- наименование
	@annotation	text,		-- аннотация
	@body 		text		-- тело новости
*/
using DigestLoader_Net6.Repository.Dto;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DigestLoader_Net6.Repository
{
    public class InternetRepositoryFile: IInternetRepository
    {
        private readonly IConfiguration _config;
        public InternetRepositoryFile(IConfiguration config)
        {
            _config = config;
        }

        public void Publish(ArticleInternetDTO article)
        {
            var path = _config["Folders:Internet"];
            var fileName = Path.Combine(path, $"{Guid.NewGuid}.txt");

            File.AppendAllText(fileName, $"Title:\r\n{article.title}\r\nBody\r\n:{article.body}\r\n");
        }
    }
}
