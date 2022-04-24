using AutoMapper;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Invest.InstrumentModels;

namespace Parcus.Api.Profiles
{
    public class InstrumentinPortfolioProfile: Profile
    {
        public InstrumentinPortfolioProfile()
        {
            CreateMap<InstrumentsInPortfolio, InstrumentInPortfolioDto>()
                .ForMember(
                    dest => dest.Id,
                    from => from.MapFrom(x => x.Id)
                )
                .ForMember(
                    dest => dest.Tiker,
                    from => from.MapFrom(x => x.Instrument.Tiker)
                )
                .ForMember(
                    dest => dest.Name,
                    from => from.MapFrom(x => x.Instrument.Name)
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
                    dest => dest.Profit,
                    from => from.MapFrom(x => x.Profit)
                )
                .ForMember(
                    dest => dest.DailyProfit,
                    from => from.MapFrom(x => x.DailyProfit)
                );
        }
    }
}
