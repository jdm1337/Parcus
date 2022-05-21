using AutoMapper;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Invest.InstrumentModels;
using Tinkoff.InvestApi.V1;

namespace Parcus.Services.Profiles
{
    public class InstrumentProfile : Profile
    { 
        public InstrumentProfile()
        {
            CreateMap<Domain.Invest.InstrumentModels.Instrument, InstrumentDto>()
                .ForMember(
                    dest => dest.Isin,
                    from => from.MapFrom(x => x.Isin)
                )
                .ForMember(
                    dest => dest.Type,
                    from => from.MapFrom(x => x.Type)
                )
                .ForMember(
                    dest => dest.Figi,
                    from => from.MapFrom(x => x.Figi)
                )
                .ForMember(
                    dest => dest.Name,
                    from => from.MapFrom(x => x.Name)
                )
                .ForMember(
                    dest => dest.Tiker,
                    from => from.MapFrom(x => x.Tiker)
                )
                .ForMember(
                    dest => dest.Country,
                    from => from.MapFrom(x => x.Country)
                )
                .ForMember(
                    dest => dest.Currency,
                    from => from.MapFrom(x => x.Currency)
                )
                .ForMember(
                    dest => dest.CurrentPrice,
                    from => from.MapFrom(x => x.CurrentPrice)
                );

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
