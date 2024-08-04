namespace JobPortal.Exceptions
{
    public class ExperienceNotFoundException : Exception
    {
        public ExperienceNotFoundException(int experienceId)
            : base($"Experience not found with ID: {experienceId}")
        {
        }
    }
}
