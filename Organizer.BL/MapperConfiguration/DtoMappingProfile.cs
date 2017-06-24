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

            CreateMap<Contact, ContactDto>()
                .ForMember(x => x.PersonalInfo, opt => opt.Ignore())
                .ForMember(x => x.Socials, opt => opt.Ignore());

            CreateMap<ContactDto, Contact>()
                .ForSourceMember(x => x.PersonalInfo, opt => opt.Ignore())
                .ForSourceMember(x => x.Socials, opt => opt.Ignore());

            CreateMap<User, UserDto>()
                .ForMember(x => x.Meetings, opt => opt.Ignore())
                .ForMember(x => x.Notes, opt => opt.Ignore())
                .ForMember(x => x.Contacts, opt => opt.Ignore());

            CreateMap<UserDto, User>()
                .ForSourceMember(x => x.Meetings, opt => opt.Ignore())
                .ForSourceMember(x => x.Notes, opt => opt.Ignore())
                .ForSourceMember(x => x.Contacts, opt => opt.Ignore());
        }
    }
}