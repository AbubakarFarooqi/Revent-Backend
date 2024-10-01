using AutoMapper;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;


namespace Revent.Common.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserProfileDto>();

            //CreateMap<Review, ReviewGetDto>();
        }
    }
}
