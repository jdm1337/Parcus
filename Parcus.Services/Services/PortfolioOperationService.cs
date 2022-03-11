using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.Transactions;
using Parcus.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Services.Services
{
    public class PortfolioOperationService : IPortfolioOperationService
    {
        protected IUnitOfWork _unitOfWork;
        public PortfolioOperationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<bool> AddAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddTransactionAsync(InvestTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
