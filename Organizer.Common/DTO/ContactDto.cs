using System.Collections.Generic;

namespace Organizer.Common.DTO
{
    public class ContactDto : BaseDto
    {
        public string PrimaryPhone { get; set; }
        public int UserId { get; set; }

        public PersonalInfoDto PersonalInfo { get; set; }

        public IEnumerable<SocialInfoDto> Socials { get; set; }
    }
}