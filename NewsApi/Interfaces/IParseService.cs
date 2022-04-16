using NewsApi.Models.Dto;

namespace NewsApi.Interfaces
{
    public interface IParseService
    {
        /// <summary>
        /// Парсим новости с сайта
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<IEnumerable<NewsViewModel>> Parse(string url);
    }
}
