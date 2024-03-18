using Message.Server.Models.Dto;
using AutoMapper;
using Message.Server.Models;

namespace Message.Server
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserMessage, UserMessageDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
