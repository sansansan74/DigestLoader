/*
Репозиторий публикует новость на сайте info

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
using DigestLoader_Net6.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigestLoader_Net6.Repository
{
    public class InfoRepository : MsSqlRepositoryBase, IInfoRepository
    {
        public InfoRepository(IConfiguration config, ILogger logger)
            : base(logger)
        {
            var connection = config.GetConnectionString("INFO_QUERY_STRING");
            SetConnectionString(connection);
        }

        public void Publish(Article article)
        {
            Exec("Публикуем новость на сайте info/ [dbo].san_load_new", con => {
                con.Execute(
                    sql: "[dbo].san_load_new",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        new_date = article.Date,
                        source = article.Sources,
                        classify = article.Classifies,
                        title = article.Title,
                        body = article.GetBodyWithTag("para")
                    }); 
            });
        }
    }
}
