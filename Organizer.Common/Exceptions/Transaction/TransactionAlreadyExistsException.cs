using System;

namespace Organizer.Common.Exceptions
{
    public class TransactionAlreadyExistsException : Exception
    {
        private const string _message = "Transaction cannot be started. Because another transaction is executing.";

        public TransactionAlreadyExistsException() : base(_message)
        {
        }
    }
}