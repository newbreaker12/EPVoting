using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidRoles : Exception
    {
        private string _Message;

        public InvalidRoles(string message)
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
