namespace JobPortal.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string email)
            : base($"User already exists with email: {email}")
        {
        }
    }
}
