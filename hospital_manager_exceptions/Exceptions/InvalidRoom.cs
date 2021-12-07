using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidRoom : Exception
    {
        private string _Message;

        public InvalidRoom(string message)
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
