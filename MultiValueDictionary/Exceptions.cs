using System;

namespace MultiValueDictionary
{
    public class MemberExistsException : Exception
    {
        public MemberExistsException(string message) : base(message)
        {
        }
    }

    public class MemberDoesNotExistException : Exception
    {
        public MemberDoesNotExistException(string message) : base(message)
        {
        }
    }
}