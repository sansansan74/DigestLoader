/*
Сигнатура вызываемой хранимой процедуры
proc [dbo].[san_load_new]
    @new_date 	datetime,	-- дата новости 		
    @source 	varchar(2048), 	-- источник
    @classify 	varchar(4096), 	-- классификаторы
    @title 		varchar(1024), 	-- наименование
    @body 		text		-- тело новости
*/

using DigestLoader_Net6.Repository.Dto;

namespace DigestLoader_Net6.Repository.Interfaces
{
    public interface IIntranetRepository
    {
        public void Publish(ArticleIntranetDTO article);
    }
}