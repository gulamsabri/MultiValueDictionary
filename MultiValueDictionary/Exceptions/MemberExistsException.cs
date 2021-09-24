using System;

namespace MultiValueDictionary.Exceptions
{
    public class MemberExistsException: Exception
    {
        public MemberExistsException(string message): base(message)
        {
            
        }
    }
}