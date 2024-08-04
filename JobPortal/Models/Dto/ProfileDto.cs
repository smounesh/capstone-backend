namespace JobPortal.Models.Dto
{
    public class ProfileDto
    {
        public int ProfileID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string ProfilePictureBase64 { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Headline { get; set; }
        public string Summary { get; set; }
        public string PhoneNumber { get; set; }
        public string LinkedinUrl { get; set; }
        public string GitHubUrl { get; set; }
        public int YearsOfExperience { get; set; }

        // Navigation 
        public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();
        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
    }
}
