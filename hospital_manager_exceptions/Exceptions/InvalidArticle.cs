using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidArticle : Exception
    {
        private string _Message;

        public InvalidArticle(string message)
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
