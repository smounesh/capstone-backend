using AutoMapper;
using JobPortal.Models;
using JobPortal.Models.Dto;

namespace JobPortal.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            // User to UserRegistrationDto
            CreateMap<User, UserRegistrationDto>();

            // UserRegistrationDto to User
            CreateMap<UserRegistrationDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            // User to UserRegistrationResponseDto
            CreateMap<User, UserRegistrationResponseDto>();

            // User to UserLoginDto
            CreateMap<User, UserLoginDto>();

            // JobPosting to JobPostingDto and vice versa
            CreateMap<JobPosting, JobPostingDto>().ReverseMap();
            CreateMap<JobPosting, JobPostingCreateDto>().ReverseMap();
            CreateMap<JobPosting, JobPostingUpdateDto>().ReverseMap();

            // Profile to ProfileDto and vice versa
            CreateMap<JobPortal.Models.Profile, ProfileDto>().ReverseMap();
            CreateMap<JobPortal.Models.Profile, ProfileCreateDto>().ReverseMap();
            CreateMap<JobPortal.Models.Profile, ProfileUpdateDto>().ReverseMap();

            // Experience to ExperienceDto and vice versa
            CreateMap<Experience, ExperienceDto>().ReverseMap();
            CreateMap<Experience, ExperienceCreateDto>().ReverseMap();
            CreateMap<Experience, ExperienceUpdateDto>().ReverseMap();

            // Education to EducationDto and vice versa
            CreateMap<Education, EducationDto>().ReverseMap();
            CreateMap<Education, EducationCreateDto>().ReverseMap();
            CreateMap<Education, EducationUpdateDto>().ReverseMap();

            // Skill to SkillDto and vice versa
            CreateMap<Skill, SkillDto>().ReverseMap();
            CreateMap<Skill, SkillCreateDto>().ReverseMap();
            CreateMap<Skill, SkillUpdateDto>().ReverseMap();

            // Resume to ResumeDto and vice versa
            CreateMap<Resume, ResumeDto>().ReverseMap();
            CreateMap<Resume, ResumeCreateDto>().ReverseMap();
            CreateMap<Resume, ResumeUpdateDto>().ReverseMap();

            // JobApplication to JobApplicationDto and vice versa
            CreateMap<JobApplication, JobApplicationDto>().ReverseMap();
            CreateMap<JobApplication, JobApplicationCreateDto>().ReverseMap();
            CreateMap<JobApplication, JobApplicationUpdateDto>().ReverseMap();
        }
    }
}
