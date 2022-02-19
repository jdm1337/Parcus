using Microsoft.EntityFrameworkCore;
using Parcus.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
