using System;

namespace Organizer.Common.Exceptions
{
    public class LoginFailedException : Exception
    {
        public const string DefaultErrorMessage = "Login failed.";

        public LoginFailedException() : base(DefaultErrorMessage)
        {
        }

        public LoginFailedException(string message) : base(message)
        {
        }
    }
}