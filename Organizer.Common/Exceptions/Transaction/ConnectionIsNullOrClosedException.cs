using System;

namespace Organizer.Common.Exceptions
{
    public class ConnnectionIsNullOrClosedException : Exception
    {
        private const string _message = "Connection is null or in closed state.";

        public ConnnectionIsNullOrClosedException() : base(_message)
        {
        }

        public ConnnectionIsNullOrClosedException(Exception originalException) : base(_message, originalException)
        {
        }
    }
}