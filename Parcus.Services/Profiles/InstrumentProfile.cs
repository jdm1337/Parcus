using AutoMapper;
using Parcus.Domain.Invest.InstrumentModels;
using Tinkoff.InvestApi.V1;

namespace Parcus.Services.Profiles
{
    public class InstrumentProfile : Profile
    { 
        public InstrumentProfile()
        {
            CreateMap<Share, Domain.Invest.InstrumentModels.Instrument>()
               .ForMember(
                   dest => dest.Type,
                   from => from.MapFrom(x => InstrumentTypes.Share)
               )
               .ForMember(
                   dest => dest.Name,
                   from => from.MapFrom(x => x.Name)
               )
               .ForMember(
                   dest => dest.Isin,
                   from => from.MapFrom(x => x.Isin)
               )
               .ForMember(
                   dest => dest.Figi,
                   from => from.MapFrom(x => x.Figi)
               )
               .ForMember(
                    dest => dest.Tiker,
                    from => from.MapFrom(x => x.Ticker)
                )
               .ForMember(
                   dest => dest.Country,
                   from => from.MapFrom(x => x.CountryOfRiskName)
               );

            CreateMap<Bond, Domain.Invest.InstrumentModels.Instrument>()
               .ForMember(
                   dest => dest.Type,
                   from => from.MapFrom(x => InstrumentTypes.Bond)
               )
               .ForMember(
                   dest => dest.Name,
                   from => from.MapFrom(x => x.Name)
               )
               .ForMember(
                   dest => dest.Isin,
                   from => from.MapFrom(x => x.Isin)
               )
               .ForMember(
                   dest => dest.Figi,
                   from => from.MapFrom(x => x.Figi)
               )
               .ForMember(
                    dest => dest.Tiker,
                    from => from.MapFrom(x => x.Ticker)
                )
               .ForMember(
                   dest => dest.Country,
                   from => from.MapFrom(x => x.CountryOfRiskName)
               );

            CreateMap<Etf, Domain.Invest.InstrumentModels.Instrument>()
               .ForMember(
                   dest => dest.Type,
                   from => from.MapFrom(x => InstrumentTypes.Etf)
               )
               .ForMember(
                   dest => dest.Name,
                   from => from.MapFrom(x => x.Name)
               )
               .ForMember(
                   dest => dest.Isin,
                   from => from.MapFrom(x => x.Isin)
               )
               .ForMember(
                   dest => dest.Figi,
                   from => from.MapFrom(x => x.Figi)
               )
               .ForMember(
                    dest => dest.Tiker,
                    from => from.MapFrom(x => x.Ticker)
                )
               .ForMember(
                   dest => dest.Country,
                   from => from.MapFrom(x => x.CountryOfRiskName)
               );
        }
    }
}
