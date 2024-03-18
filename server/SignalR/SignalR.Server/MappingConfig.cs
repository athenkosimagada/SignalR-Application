using SignalR.Server.Models;
using SignalR.Server.Models.Dto;
using AutoMapper;

namespace SignalR.Server
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Connection, ConnectionDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
