using System.Collections.Generic;

namespace Organizer.Common.DTO
{
    public class UserDto : BaseDto
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public IEnumerable<ContactDto> Contacts { get; set; }

        public IEnumerable<MeetingDto> Meetings { get; set; }

        public IEnumerable<NoteDto> Notes { get; set; }
    }
}