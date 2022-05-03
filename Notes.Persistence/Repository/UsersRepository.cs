using Microsoft.Extensions.Logging;
using Parcus.Persistence.Data;
using Parcus.Application.Interfaces.IRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcus.Domain.Identity;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Parcus.Persistence.Repository
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        public UsersRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<PagedList<UserDto>> GetUsers(UserParameters paginationParameters)
        {
            var userDtoList = await _context.Users
                .OrderBy(on => on.Id)
                .Select(x => new UserDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                })
                .ToListAsync();

            return PagedList<UserDto>.ToPagedList(userDtoList,
                paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }
        //Override method here. 

    }
}
