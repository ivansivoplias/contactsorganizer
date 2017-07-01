using System;

namespace Organizer.Common.Exceptions
{
    public class NoteCaptionAlreadyExistsException : Exception
    {
        public const string DefaultErrorMessage = "Note with such caption already exists in database.";

        public NoteCaptionAlreadyExistsException() : base(DefaultErrorMessage)
        {
        }

        public NoteCaptionAlreadyExistsException(string message) : base(message)
        {
        }
    }
}