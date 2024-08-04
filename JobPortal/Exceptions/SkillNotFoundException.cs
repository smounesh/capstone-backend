namespace JobPortal.Exceptions
{
    public class SkillNotFoundException : Exception
    {
        public SkillNotFoundException(int skillId)
            : base($"Skill not found with ID: {skillId}")
        {
        }
    }
}
