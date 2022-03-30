﻿using Parcus.Application.Interfaces.IRepository;
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
        // Interfaces of repositories adding here.
        IUsersRepository Users { get; }
        IPortfoliosRepository Portfolios { get; }
        IBrokersRepository Brokers { get; }
        IInstrumentRepository Instruments { get; }
        IInstrumentsInPortfolioRepository InstrumentsInPortfolio { get; }
        IInvestTransactionsRepository InvestTransactions { get; }

        Task CompleteAsync();
    }
}
