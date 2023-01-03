using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidUsersEmail : Exception
    {
        private string _Message;

        public InvalidUsersEmail(string message)
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
