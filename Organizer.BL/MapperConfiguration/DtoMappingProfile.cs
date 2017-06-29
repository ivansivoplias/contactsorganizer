using AutoMapper;
using Organizer.Common.DTO;
using Organizer.Common.Entities;

namespace Organizer.BL.MapperConfiguration
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<SocialInfo, SocialInfoDto>()
                .ReverseMap();

            CreateMap<PersonalInfo, PersonalInfoDto>()
                .ReverseMap();

            CreateMap<Meeting, MeetingDto>()
                .ReverseMap();

            CreateMap<Note, NoteDto>()
                .ReverseMap();

            CreateMap<Contact, ContactDto>()
                .ForMember(x => x.PersonalInfo, opt => opt.Ignore())
                .ForMember(x => x.Socials, opt => opt.Ignore());

            CreateMap<ContactDto, Contact>()
                .ForSourceMember(x => x.PersonalInfo, opt => opt.Ignore())
                .ForSourceMember(x => x.Socials, opt => opt.Ignore());

            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>();
        }
    }
}