/*
Репозиторий публикует новость на сайте Internet
 
Сигнатура вызываемой хранимой процедуры
proc [dbo].[san_add_digest_new]
	@new_date 	datetime,	-- дата новости 		
	@title 		varchar(1024), 	-- наименование
	@annotation	text,		-- аннотация
	@body 		text		-- тело новости
*/
using Dapper;
using DigestLoader_Net6.Repository.Dto;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Repository
{
    internal class InternetRepository : MsSqlRepositoryBase, IInternetRepository
    {
        public InternetRepository(IConfiguration config, ILogger logger)
            : base(logger)
        {
            var connection = config.GetConnectionString("INTERNET_QUERY_STRING");
            SetConnectionString(connection);
        }
        

        public void Publish(ArticleInternetDTO article)
        {

            Exec("Публикуем новость на интернет сайте [dbo].san_add_digest_new", con => {
                
                con.Execute(
                    sql: "[dbo].san_add_digest_new",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        article.new_date,
                        article.title,
                        article.annotation,
                        article.body
                    });
            });
        }
    }
}
