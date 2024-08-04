using System;

namespace JobPortal.Exceptions
{
    public class EducationNotFoundException : Exception
    {
        public EducationNotFoundException() : base("Education ID not found.")
        {
        }

        public EducationNotFoundException(string message) : base(message)
        {
        }

        public EducationNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
