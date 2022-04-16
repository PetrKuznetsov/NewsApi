using System.Net.Http;

namespace NewsApi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        public Worker(ILogger<Worker> _logger)
        {    
            logger = _logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {   
                    using(var client = new HttpClient())
                    {
                        var result = await client.GetAsync("http://localhost:5024/api/News/parse-news");
                        if (result.IsSuccessStatusCode)
                        {
                            logger.LogInformation("Данные успешно получены с сайта. Статус код: " + result.StatusCode);
                        }
                        else
                        {
                            logger.LogError("Возникла ошибка при получении данных.Статус код: " + result.StatusCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Сайт не доступен.", ex.Message);
                }
                finally
                {
                    await Task.Delay(1000 * 3600, stoppingToken);
                }
            }
        }
    }
}
