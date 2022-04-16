using NewsApi.Models;
using NewsApi.Models.Dto;

namespace NewsApi.Repositories.NewsRepositories
{
    public interface INewsRepository
    {
        public void AddRangeNews(List<News> news);
        public void Save();
        public int CountNews();
    }
}
