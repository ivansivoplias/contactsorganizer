using System;

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

        public MeetingNameAlreadyExistsException(Exception originalException) : base(DefaultErrorMessage, originalException)
        {
        }

        public MeetingNameAlreadyExistsException(string message, Exception originalException) : base(message, originalException)
        {
        }
    }
}