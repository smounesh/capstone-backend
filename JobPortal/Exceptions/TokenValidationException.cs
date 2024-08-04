namespace JobPortal.Exceptions
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
