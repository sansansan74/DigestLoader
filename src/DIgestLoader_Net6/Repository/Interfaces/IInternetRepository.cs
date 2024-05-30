using DigestLoader_Net6.Repository.Dto;

namespace DigestLoader_Net6.Repository.Interfaces
{
    public interface IInternetRepository
    {
        public void Publish(ArticleInternetDTO article);
    }
}
