using AutoMapper;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.EFCore.DataModel.Models;


namespace Revent.Common.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, Users>()
                .ForMember(dest => dest.Profileimage , opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.Firstname , opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Firstname , opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Lastname , opt => opt.MapFrom(src => src.LastName));

            CreateMap<EventCreateDto, Events>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventTypeId));

            CreateMap<EventUpdateDto, Events>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventTypeId));

            CreateMap<OrganizationCreateDto, Organizations>();
            CreateMap<OrganizationUpdateDto, Organizations>();


            CreateMap<GroupMessageDto, GroupMessages>();
            


        }
    }
}
