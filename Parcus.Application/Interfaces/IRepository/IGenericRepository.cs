using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int itemId, string userId);
        Task<bool> Update(T entity);
    }
}
