using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IPortfolioOperationService
    {
        Task<bool> AddAsync();
        Task<bool> DeleteAsync(string id);
        Task<bool> AddTransactionAsync(InvestTransaction transaction);
    }
}
