using AutoMapper;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.DTO.Outgoing;
using Parcus.Domain.Invest.PortfolioModels;

namespace Parcus.Api.Profiles
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<BrokeragePortfolio, PortfolioDto>()
                .ForMember(
                    dest => dest.Name,
                    from => from.MapFrom(x => x.Name)
                )
                .ForMember(
                    dest => dest.CreatedDate,
                    from => from.MapFrom(x => x.CreatedDate)
                );

            CreateMap<AddPortfolioRequest, BrokeragePortfolio>()
                .ForMember(
                    dest => dest.Name,
                    from => from.MapFrom(x => x.Name)
                )
                .ForMember(
                    dest => dest.CreatedDate,
                    from => from.MapFrom(x => DateTime.UtcNow)
                );

            CreateMap<BrokeragePortfolio, AddPortfolioResponse>()
               .ForMember(
                   dest => dest.Id,
                   from => from.MapFrom(x => x.Id)
               )
               .ForMember(
                   dest => dest.Name,
                   from => from.MapFrom(x => x.Name)
               );

        }
    }
}
