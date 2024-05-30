/*
Репозиторий публикует новость на сайте Intranet

Сигнатура вызываемой хранимой процедуры
proc [dbo].[san_load_new]
    @new_date 	datetime,	-- дата новости 		
    @source 	varchar(2048), 	-- источник
    @classify 	varchar(4096), 	-- классификаторы
    @title 		varchar(1024), 	-- наименование
    @body 		text		-- тело новости
*/
using Dapper;
using DigestLoader_Net6.Classes;
using DigestLoader_Net6.Repository.Dto;
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Repository
{
    public class IntranetRepository : MsSqlRepositoryBase, IIntranetRepository
    {
        public IntranetRepository(IConfiguration config, ILogger logger)
            : base(logger)
        {
            var connection = config.GetConnectionString("INTRANET_QUERY_STRING");
            SetConnectionString(connection);
        }

        public void Publish(ArticleIntranetDTO article)
        {
            Exec("Публикуем новость на сайте Intranet/ [dbo].san_load_new", con => {
                con.Execute(
                    sql: "[dbo].san_load_new",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        new_date = article.new_date,
                        source = article.source,
                        classify = article.classify,
                        title = article.title,
                        body = article.body
                    }); 
            });
        }
    }
}
