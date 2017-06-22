using System;

namespace Organizer.Common.Exceptions
{
    public class TransactionCommitException : Exception
    {
        private const string _message = "Transaction is not opened and cannot be commited.";

        public TransactionCommitException() : base(_message)
        {
        }
    }
}