using Parcus.Application.Interfaces.IRepository;
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

        Task CompleteAsync();
    }
}
