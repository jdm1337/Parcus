using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels.Bonds;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using Parcus.Domain.Invest.InstrumentModels.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IUnitOfWorkConfiguration
{
    public interface IUnitOfWork
    {
        // Interfaces of repositories adding here.
        IUsersRepository Users { get; }
        IPortfoliosRepository Portfolios { get; }
        IBrokersRepository Brokers { get; }
        IInstrumentRepository<SharesInPortfolio> SharesInPortfolio { get; }
        IInstrumentRepository<FundsInPortfolio> FundsInPortfolio { get; }
        IInstrumentRepository<BondsInPortfolio> BondsInPortfolio { get; }

        Task CompleteAsync();
    }
}
