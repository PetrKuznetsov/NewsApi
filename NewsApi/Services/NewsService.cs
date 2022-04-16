using Microsoft.EntityFrameworkCore;
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
            return await _context.News.OrderByDescending(n => n.CreatedAt).FirstAsync();
        }

        public async Task<IEnumerable<News>> SearchNewsByQuery(string query)
        {
            return await _context.News.Where(n => n.Article.Contains(query)).ToListAsync();
        }

        public async Task<IEnumerable<News>> SearchNewsByRequest(NewsRequest request)
        {
            return await _context.News.AsQueryable().Where(n => n.CreatedAt >= request.DateFrom && n.CreatedAt <= request.DateTo).ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetTopWords()
        {
            var data = await _context.News.Select(s => s.Article).ToListAsync();
            StringBuilder fullText = new StringBuilder();
            foreach (var item in data)
            {
                fullText.Append(item);
            }
            IEnumerable<string> wordList = !string.IsNullOrEmpty(fullText.ToString()) ? fullText.ToString().ToLower().Split(' ') : Enumerable.Empty<string>();
            var grouped = wordList.Where(s => s.Length > 3)
                .GroupBy(i => i)
                .Select(i => new KeyValuePair<string, int>(i.Key, i.Count()))
                .OrderByDescending(i => i.Value).Take(10).ToDictionary(item => item.Key, item => item.Value);
            return grouped;
        }
    }
}
