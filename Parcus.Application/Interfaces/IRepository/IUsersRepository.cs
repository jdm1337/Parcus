using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Identity;
using Parcus.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<PagedList<UserDto>> GetUsers(UserParameters paginationParameters);
    }
}
