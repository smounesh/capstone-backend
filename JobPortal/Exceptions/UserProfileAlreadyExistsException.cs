using System;

namespace JobPortal.Exceptions
{
    public class UserProfileAlreadyExistsException : Exception
    {
        public UserProfileAlreadyExistsException() : base("User profile already exists.")
        {
        }

        public UserProfileAlreadyExistsException(string message) : base(message)
        {
        }

        public UserProfileAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
