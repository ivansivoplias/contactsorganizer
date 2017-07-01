using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Common.Exceptions
{
    public class MeetingNameAlreadyExistsException : Exception
    {
        public const string DefaultErrorMessage = "Meeting with such name already exist in database.";

        public MeetingNameAlreadyExistsException() : base(DefaultErrorMessage)
        {
        }

        public MeetingNameAlreadyExistsException(string message) : base(message)
        {
        }
    }
}