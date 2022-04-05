using AutoMapper;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Identity;

namespace Parcus.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.UserId,
                    from => from.MapFrom(x => x.Id)
                )
                .ForMember(
                    dest => dest.Email,
                    from => from.MapFrom(x => x.Email)
                )
                .ForMember(
                    dest => dest.UserName,
                    from => from.MapFrom(x => x.UserName)
                );
        }
    }
}
