namespace JobPortal.Exceptions
{
    public class DuplicateJobApplicationException : Exception
    {
        public DuplicateJobApplicationException(string message) : base(message)
        {
        }
    }
}
