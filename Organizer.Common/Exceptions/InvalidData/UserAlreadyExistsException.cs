using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}