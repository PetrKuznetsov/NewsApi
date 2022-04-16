using AutoMapper;
using NewsApi.Models;
using NewsApi.Models.Dto;

namespace NewsApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<NewsViewModel, News>();
                config.CreateMap<News, NewsViewModel>();
            });

            return mappingConfig;
        }
    }
}
