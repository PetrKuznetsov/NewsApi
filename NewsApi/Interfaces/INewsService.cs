using NewsApi.Models;
using NewsApi.Models.Dto;

namespace NewsApi.Interfaces
{
    public interface INewsService
    {
        /// <summary>
        /// Поиск новостей по ключевому слову
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<News>> SearchNewsByQuery(string query);
        /// <summary>
        /// Поиск новостей по дате
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<News>> SearchNewsByRequest(NewsRequest request);
        /// <summary>
        /// Получить последнию новость
        /// </summary>
        /// <returns></returns>
        Task<News> GetLastNews();
        /// <summary>
        /// Получить топ 10 слов
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, int>> GetTopWords();
    }
}
