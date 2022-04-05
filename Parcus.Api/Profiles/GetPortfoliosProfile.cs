using AutoMapper;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.DTO.Outgoing;

namespace Parcus.Api.Profiles
{
    public class GetPortfoliosProfile : Profile
    {
        public GetPortfoliosProfile()
        {
            CreateMap<IEnumerable<PortfolioDto>, GetPortfoliosResponse>()
                .ForMember(
                    dest => dest.Count,
                    from => from.MapFrom(x => x.Count())
                )
                .ForMember(
                    dest => dest.Portfolios,
                    from => from.MapFrom(x => x.ToList())
                );
        }
    }
}
