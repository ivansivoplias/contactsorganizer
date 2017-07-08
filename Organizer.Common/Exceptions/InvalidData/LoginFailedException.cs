using System;

namespace Organizer.Common.Exceptions
{
    public class LoginFailedException : Exception
    {
        public const string DefaultErrorMessage = "Login failed.";

        public LoginFailedException() : base(DefaultErrorMessage)
        {
        }

        public LoginFailedException(Exception originalException) : base(DefaultErrorMessage, originalException)
        {
        }

        public LoginFailedException(string message) : base(message)
        {
        }

        public LoginFailedException(string message, Exception originalException) : base(message, originalException)
        {
        }
    }
}