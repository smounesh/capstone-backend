using AutoMapper;
using JobPortal.Models;
using JobPortal.Models.Dto;
using JobPortal.Repositories.Interfaces;
using JobPortal.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class ProfileViewService : IProfileViewService
    {
        private readonly IProfileViewRepository _profileViewRepository;
        private readonly IMapper _mapper;

        public ProfileViewService(IProfileViewRepository profileViewRepository, IMapper mapper)
        {
            _profileViewRepository = profileViewRepository;
            _mapper = mapper;
        }

        public async Task LogProfileViewAsync(int viewerId, int profileId)
        {
            var profileView = new ProfileView
            {
                ViewerID = viewerId,
                ProfileID = profileId
            };

            await _profileViewRepository.LogProfileViewAsync(profileView);
        }

        public async Task<IEnumerable<ProfileViewDto>> GetProfileViewsAsync(int profileId)
        {
            var profileViews = await _profileViewRepository.GetProfileViewsByProfileIdAsync(profileId);
            return _mapper.Map<IEnumerable<ProfileViewDto>>(profileViews);
        }
    }
}
