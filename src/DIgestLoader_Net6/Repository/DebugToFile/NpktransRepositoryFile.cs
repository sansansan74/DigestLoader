/*
Сигнатура вызываемой хранимой процедуры
proc [dbo].[san_add_digest_new]
	@new_date 	datetime,	-- дата новости 		
	@title 		varchar(1024), 	-- наименование
	@annotation	text,		-- аннотация
	@body 		text		-- тело новости
*/
using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DigestLoader_Net6.Repository
{
    internal class NpktransRepositoryFile: INpktransRepository
    {
        private readonly IConfiguration _config;
        public NpktransRepositoryFile(IConfiguration config)
        {
            _config = config;
        }

        public void Publish(Article article)
        {
            var path = _config["Folders:Npktrans"];
            var fileName = Path.Combine(path, $"{Guid.NewGuid}.txt");

            File.AppendAllText(fileName, $"Title:\r\n{article.Title}\r\nBody\r\n:{article.GetBodyWithTag("para")}\r\n");
        }
    }
}
