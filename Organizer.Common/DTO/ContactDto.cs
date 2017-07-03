using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Common.DTO
{
    public class ContactDto : BaseDto
    {
        public string PrimaryPhone { get; set; }
        public int UserId { get; set; }

        public PersonalInfo PersonalInfo { get; set; }

        public IEnumerable<SocialInfo> Socials { get; set; }
    }
}