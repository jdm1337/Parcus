using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parcus.Persistence.Data;
using Parcus.Application.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        internal DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(AppDbContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
            dbSet = context.Set<T>();
        }
        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public Task<bool> Delete(int itemId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
