using NewsApi.DbContexts;
using NewsApi.Interfaces;
using NewsApi.Models;
using NewsApi.Models.Dto;
using System.Text;

namespace NewsApi.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;
        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<News> GetLastNews()
        {
            return _context.News.OrderByDescending(n => n.CreatedAt).First();
        }

        public async Task<IEnumerable<News>> SearchNewsByQuery(string query)
        {
            return _context.News.Where(n => n.Article.Contains(query));
        }

        public async Task<IEnumerable<News>> SearchNewsByRequest(NewsRequest request)
        {
            return _context.News.Where(n => n.CreatedAt >= request.DateFrom && n.CreatedAt <= request.DateTo);
        }

        public async Task<Dictionary<string, int>> GetTopWords()
        {
            var data = _context.News.Select(s => s.Article);
            StringBuilder fullText = new StringBuilder();
            foreach (var item in data)
            {
                fullText.Append(item);
            }
            IEnumerable<string> wordList = !string.IsNullOrEmpty(fullText.ToString()) ? fullText.ToString().Split(' ') : Enumerable.Empty<string>();
            var grouped = wordList.Where(s => s.Length > 3)
                .GroupBy(i => i)
                .Select(i => new KeyValuePair<string, int>(i.Key, i.Count()))
                .OrderByDescending(i => i.Value).Take(10).ToDictionary(item => item.Key, item => item.Value);
            return grouped;
        }
    }
}
