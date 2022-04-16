using HtmlAgilityPack;
using NewsApi.Interfaces;
using NewsApi.Models.Dto;
using System.Net;
using System.Text;

namespace NewsApi.Services
{
    public class ParseService : IParseService
    {
        private readonly ILogger<ParseService> _logger;
        public ParseService(ILogger<ParseService> logger)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<NewsViewModel>> Parse(string url)
        {
            try
            {
                _logger.LogInformation("Сайт, который пытаемся спарсить: " + url);                           
                HtmlWeb indexHtml = new HtmlWeb();
                var htmlDoc = indexHtml.Load(url);
                if (htmlDoc != null)
                {
                    var Newsletter = htmlDoc.DocumentNode.SelectNodes(".//ul[@id='lentach']//li//div[@class='body_lenta_b']");
                    if (Newsletter != null)
                    {
                        var links = new List<string>();
                        foreach (var node in Newsletter)
                        {
                            var linksNewsNode = node.SelectSingleNode(".//a[@class='lenta_news_title']");
                            if (linksNewsNode != null)
                            {
                                links.Add(url + linksNewsNode.Attributes["href"].Value);
                            }
                        }
                        if (links != null && links.Count > 0)
                        {
                            _logger.LogInformation($"Получли ссылки на новости в колличестве:{links.Count}");
                            List<NewsViewModel> news = new List<NewsViewModel>();
                            foreach (var item in links)
                            {
                                htmlDoc = indexHtml.Load(item);
                                if (htmlDoc != null)
                                {
                                    var article = htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='article_news_block']");
                                    if (article != null)
                                    {
                                        var titleNews = article.SelectSingleNode(".//div[@class='article_title']//h1").InnerText;
                                        var dateCreated = article.SelectSingleNode(".//div[@class='date_public_art']").InnerText;
                                        var bodyArticle = article.SelectNodes(".//div[@class='article_news_body']//p");
                                        StringBuilder data = new StringBuilder();
                                        foreach (var elm in bodyArticle)
                                        {
                                            data.Append(elm.InnerText);
                                        }
                                        if (titleNews != null && dateCreated != null && data != null)
                                        {
                                            NewsViewModel model = new NewsViewModel()
                                            {
                                                Article = data.ToString(),
                                                Title = titleNews,
                                                CreatedAt = Convert.ToDateTime(dateCreated)
                                            };
                                            news.Add(model);
                                            _logger.LogInformation($"Новость :{model.Title} успешно создана");
                                        }
                                    }
                                }
                            }
                            _logger.LogInformation($"Успешно спарсили новости");
                            return news;
                        }
                    }
                }
                _logger.LogWarning($"Новостей нет!");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка" + ex);
                throw;
            }
        }
    }
}
