using System;

namespace Organizer.Common.Exceptions
{
    public class SocialAlreadyExistException : Exception
    {
        public const string DefaultErrorMessage = "Social with such app name and app identifier already exist in db.";

        public SocialAlreadyExistException() : base(DefaultErrorMessage)
        {
        }

        public SocialAlreadyExistException(string message) : base(message)
        {
        }
    }
}