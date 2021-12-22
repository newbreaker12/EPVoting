using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidGroups: Exception
    {
        private string _Message;

        public InvalidGroups(string message)
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
