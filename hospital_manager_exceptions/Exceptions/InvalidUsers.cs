using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidUsers : Exception
    {
        private string _Message;

        public InvalidUsers(string message)
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
