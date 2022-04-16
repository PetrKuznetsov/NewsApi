using NewsApi.Models;
using NewsApi.Models.Dto;

namespace NewsApi.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<News>> SearchNewsByQuery(string query);
        Task<IEnumerable<News>> SearchNewsByRequest(NewsRequest request);
        Task<News> GetLastNews();
        Task<Dictionary<string, int>> GetTopWords();
    }
}
