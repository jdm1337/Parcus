using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IInstrumentRepository<TEntity>: IGenericRepository<TEntity> where TEntity : class
    {

    }
}
