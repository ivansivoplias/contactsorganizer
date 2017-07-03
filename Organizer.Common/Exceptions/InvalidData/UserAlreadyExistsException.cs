using System;

namespace Organizer.Common.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public const string DefaultErrorMessage = "User already exists in database.";

        public UserAlreadyExistsException() : base(DefaultErrorMessage)
        {
        }

        public UserAlreadyExistsException(string message) : base(message)
        {
        }

        public UserAlreadyExistsException(Exception originalException) : base(DefaultErrorMessage, originalException)
        {
        }

        public UserAlreadyExistsException(string message, Exception originalException) : base(message, originalException)
        {
        }
    }
}