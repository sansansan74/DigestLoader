/*
Репозиторий публикует новость на сайте npktrans.ru
 
Сигнатура вызываемой хранимой процедуры
proc [dbo].[san_add_digest_new]
	@new_date 	datetime,	-- дата новости 		
	@title 		varchar(1024), 	-- наименование
	@annotation	text,		-- аннотация
	@body 		text		-- тело новости
*/
using Dapper;
using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Repository
{
    internal class NpktransRepository : MsSqlRepositoryBase, INpktransRepository
    {
        public NpktransRepository(IConfiguration config, ILogger logger)
            : base(logger)
        {
            var connection = config.GetConnectionString("NPKTRANS_QUERY_STRING");
            SetConnectionString(connection);
        }
        

        public void Publish(Article article)
        {

            Exec("Публикуем новость на сайте npktrans.ru [dbo].san_add_digest_new", con => {
                
                con.Execute(
                    sql: "[dbo].san_add_digest_new",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        new_date = article.Date,
                        title = article.Title,
                        annotation= article.GetAnnotation(),
                        body = article.GetBodyForNbktrans()
                    });
            });
        }
    }
}
