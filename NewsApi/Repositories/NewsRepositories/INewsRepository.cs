using NewsApi.Models;
using NewsApi.Models.Dto;

namespace NewsApi.Repositories.NewsRepositories
{
    public interface INewsRepository
    {
        /// <summary>
        /// Добавить список новостей
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public void AddRangeNews(List<News> news);
        /// <summary>
        /// Сохранить изменения в бд
        /// </summary>
        /// <returns></returns>
        public void Save();
        /// <summary>
        /// Получить количество новостей
        /// </summary>
        /// <returns></returns>
        public int CountNews();
    }
}
