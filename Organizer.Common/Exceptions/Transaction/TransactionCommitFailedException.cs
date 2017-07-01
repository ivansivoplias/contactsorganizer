using System;

namespace Organizer.Common.Exceptions
{
    public class TransactionCommitFailedException : Exception
    {
        private const string _message = "Transaction is not opened and cannot be commited.";

        public TransactionCommitFailedException() : base(_message)
        {
        }
    }
}