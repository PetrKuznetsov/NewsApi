using Microsoft.EntityFrameworkCore;
using NewsApi.DbContexts;
using NewsApi.Models;
using NewsApi.Models.Dto;
using System.Text;

namespace NewsApi.Repositories.NewsRepositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddRangeNews(List<News> news)
        {
            _context.News.AddRangeAsync(news);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public int CountNews() 
        {
            return _context.News.Count(); 
        }
    }
}
