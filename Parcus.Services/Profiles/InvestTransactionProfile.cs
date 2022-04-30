using AutoMapper;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.Invest.Transactions;
using Parcus.Services.Extensions;


namespace Parcus.Services.Profiles
{
    public class InvestTransactionProfile : Profile
    {
        private DateTime date = DateTime.UtcNow;
        private Transactions transactions = new Transactions();
        public InvestTransactionProfile()
        {
            CreateMap<AddTransactionRequest, InvestTransaction>()
                .ForMember(
                    dest => dest.InstrumentPrice,
                    from => from.MapFrom(x => x.Price)
                    )
                .ForMember(
                    dest => dest.Amount,
                    from => from.MapFrom(x => x.Amount)
                    )
                .ForMember(
                    dest => dest.TransactionDate,
                    from => from.MapFrom(x => date.ParseGivenTime(x.TransactionDate))
                    )
                .ForMember(
                    dest => dest.TransactionType,
                    from => from.MapFrom(x => transactions.GetTransactType(x.TransactionType))
                    )
                .ForMember(
                    dest => dest.BrokeragePortfolioId,
                    from => from.MapFrom(x => x.PortfolioId)
                    );
        }
    }
}
