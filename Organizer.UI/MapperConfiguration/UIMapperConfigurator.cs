using AutoMapper;
using Organizer.BL.MapperConfiguration;

namespace Organizer.UI.MapperConfiguration
{
    public static class UIMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<DtoMappingProfile>());

            Mapper.AssertConfigurationIsValid();
        }
    }
}