using AutoMapper;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.Invest.Brokers;

namespace Parcus.Api.Profiles
{
    public class BrokerPortfolio : Profile
    {
        public BrokerPortfolio()
        {
            CreateMap<AddBrokerRequest, Broker>()
               .ForMember(
                   dest => dest.Name,
                   from => from.MapFrom(x => x.BrokerName)
               )
               .ForMember(
                    dest => dest.Percentage,
                    from => from.MapFrom(x => x.Percentage)
                );
        }
    }
}
