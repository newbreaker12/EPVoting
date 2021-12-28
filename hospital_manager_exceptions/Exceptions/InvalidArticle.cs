using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidVote : Exception
    {
        private string _Message;

        public InvalidVote(string message)
        {
            _Message = message;
        }

        public override string Message
        {
            get
            {
                return _Message;
            }
        }

    }
}
