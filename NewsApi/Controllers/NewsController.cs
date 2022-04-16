using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsApi.DbContexts;
using NewsApi.Interfaces;
using NewsApi.Models;
using NewsApi.Models.Dto;
using NewsApi.Repositories.NewsRepositories;
using System.Net;
using System.Text;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IParseService parseService;
        private readonly string webSite;
        private readonly IMapper mapper;
        private readonly INewsRepository contextNewsRepository;
        private readonly ILogger<NewsController> logger;
        private readonly INewsService newsService;
        public NewsController(
            IParseService parseService, 
            IConfiguration configuration, 
            IMapper mapper, INewsRepository newsContext,
            ILogger<NewsController> logger,
            INewsService newsService
        )
        {
            this.parseService = parseService;
            webSite = configuration["WebSite"];
            this.mapper = mapper;
            contextNewsRepository = newsContext;
            this.logger = logger;
            this.newsService = newsService;
        }

        /// <summary>
        /// Получить новости за временной промежуток 
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> SearchNewsByDate([FromBody] NewsRequest request)
        {
            try
            {
                logger.LogInformation($"Запрос на поиск новостей по дате с {request.DateFrom} по {request.DateTo}") ;
                var news = await newsService.SearchNewsByRequest(request);
                if (news == null)
                {
                    logger.LogWarning($"Новости по дате с {request.DateFrom} по {request.DateTo} не найдены"); ;
                    return NotFound();
                }
                return Ok(news);
            }
            catch (Exception ex)
            {
                logger.LogError("Произошла ошибка: " + ex.Message);
                return StatusCode(500);
            }           
        }

        /// <summary>
        /// Парсим данные с сайта
        /// </summary>
        /// <returns></returns>
        [HttpGet(), Route("parse-news")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> ParseNews()
        {
            try
            {
                int newsCount = contextNewsRepository.CountNews();
                IEnumerable<NewsViewModel> models = await parseService.Parse(webSite);
                if (models == null)
                {
                    logger.LogWarning("Спарсить новости не получилось");
                    return NotFound();
                }
                List<News> news = new List<News>();
                foreach (NewsViewModel model in models)
                {
                    var article = mapper.Map<NewsViewModel, News>(model);
                    news.Add(article);
                }        
                if (newsCount == 0)
                {
                    logger.LogInformation("Заполняем базу данных новостей");
                    contextNewsRepository.AddRangeNews(news);
                    contextNewsRepository.Save();
                }
                else
                {
                    var article = await newsService.GetLastNews();
                    news = news.Where(news => news.CreatedAt >= article.CreatedAt && news.Title != article.Title).ToList();
                    if (news != null)
                    {
                        logger.LogInformation($"С момента последнего добавления {article.CreatedAt}, вышло еще:{news.Count} новых новостей");
                        contextNewsRepository.AddRangeNews(news);
                        contextNewsRepository.Save();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("Произошла ошибка: " + ex.Message);
                return StatusCode(500);
            }
            
        }


        /// <summary>
        /// Получить топ 10 слов с новостей 
        /// </summary>
        /// <returns></returns>
        [HttpGet(), Route("topten")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> GetTopNews()
        {
            try
            {
                var words = await newsService.GetTopWords();
                if (words == null | words.Count == 0)
                {
                    logger.LogWarning("Слова не найдены");
                    return NotFound();
                }
                return Ok(words);
            }
            catch (Exception ex)
            {
                logger.LogError("Произошла ошибка: " + ex.Message);
                return StatusCode(500);
            }
            
            
        }

        /// <summary>
        /// Поиск новости по запросу
        /// </summary>
        /// <returns></returns>
        [HttpGet(), Route("search")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> SearchNews([FromQuery] string query)
        {
            try
            {
                logger.LogInformation("Поиск новостей по ключевым словам: " + query);
                var news = await newsService.SearchNewsByQuery(query);
                if (news == null)
                {
                    logger.LogWarning("Новости по ключевым словам не найдены");
                    return NotFound();
                }
                return Ok(news);
            }
            catch (Exception ex)
            {
                logger.LogError("Произошла ошибка: " + ex.Message);
                return StatusCode(500);
            }    
        }
    }
}
