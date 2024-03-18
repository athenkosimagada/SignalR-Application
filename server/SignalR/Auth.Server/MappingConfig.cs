using Auth.Server.Models;
using Auth.Server.Models.Dto;
using AutoMapper;

namespace Auth.Server
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
