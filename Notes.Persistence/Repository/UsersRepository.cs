using Microsoft.Extensions.Logging;
using Parcus.Persistence.Data;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Repository
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        public UsersRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        //Override method here.
    }
}
