using Post.Server.Models.Dto;
using AutoMapper;
using Post.Server.Models;

namespace Post.Server
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserPost, UserPostDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
