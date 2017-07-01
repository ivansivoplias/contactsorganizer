using System;

namespace Organizer.Common.Exceptions
{
    public class PrimaryPhoneAlreadyExistException : Exception
    {
        public const string DefaultErrorMessage = "Contact with such primary phone already exists in db.";

        public PrimaryPhoneAlreadyExistException() : base(DefaultErrorMessage)
        {
        }

        public PrimaryPhoneAlreadyExistException(string message) : base(message)
        {
        }
    }
}