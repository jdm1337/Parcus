using AutoMapper;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.Invest.InstrumentModels;

namespace Parcus.Api.Profiles
{
    public class InstrumentInPortfolioProfile : Profile
    {
        public InstrumentInPortfolioProfile()
        {
            CreateMap<InstrumentsInPortfolio, InstrumentInPortfolioDto>()
                .ForMember(
                    dest => dest.Id,
                    from => from.MapFrom(x => x.Id)
                )
                .ForMember(
                    dest => dest.Tiker,
                    from => from.MapFrom(x => x.Tiker)
                )
                .ForMember(
                    dest => dest.Name,
                    from => from.MapFrom(x => x.Name)
                )
                .ForMember(
                    dest => dest.Amount,
                    from => from.MapFrom(x => x.Amount)
                )
                .ForMember(
                    dest => dest.AveragePrice,
                    from => from.MapFrom(x => x.AveragePrice)
                )
                .ForMember(
                    dest => dest.InvestedValue,
                    from => from.MapFrom(x => x.InvestedValue)
                )
                .ForMember(
                    dest => dest.CurrentValue,
                    from => from.MapFrom(x => x.CurrentValue)
                )
                .ForMember(
                    dest => dest.CurrentPrice,
                    from => from.MapFrom(x => x.CurrentPrice)
                )
                .ForMember(
                    dest => dest.Profit,
                    from => from.MapFrom(x => x.Profit)
                )
                .ForMember(
                    dest => dest.DailyProfit,
                    from => from.MapFrom(x => x.DailyProfit)
                );

            CreateMap<AddTransactionRequest, InstrumentsInPortfolio>()
                .ForMember(
                    dest => dest.Figi,
                    from => from.MapFrom(x => x.Figi)
                )
                .ForMember(
                    dest => dest.Amount,
                    from => from.MapFrom(x => x.Amount)
                );
                
                
        }
      
    }
}
