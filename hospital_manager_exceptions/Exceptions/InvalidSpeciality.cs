﻿using System;

namespace voting_exceptions.Exceptions
{
    public class InvalidSpeciality : Exception
    {
        private string _Message;

        public InvalidSpeciality(string message)
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
