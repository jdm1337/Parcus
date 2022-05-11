using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IUnitOfWorkConfiguration
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        IRolesRepository Roles { get; }
        IPortfoliosRepository Portfolios { get; }
        IBrokersRepository Brokers { get; }
        IInstrumentRepository Instruments { get; }
        IInstrumentsInPortfolioRepository InstrumentsInPortfolio { get; }
        IInvestTransactionsRepository InvestTransactions { get; }

        Task CompleteAsync();
    }
}
