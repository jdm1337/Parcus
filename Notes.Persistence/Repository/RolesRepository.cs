using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Identity;
using Parcus.Domain.Pagination;
using Parcus.Persistence.Data;

namespace Parcus.Persistence.Repository
{
    public class RolesRepository : GenericRepository<Role>, IRolesRepository
    {
        public RolesRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<PagedList<RoleDto>> GetRoles(RoleParameters paginationParameters)
        {
            var rolesDtoList = await _context.Roles
                .OrderBy(on => on.Id)
                .Select(x => new RoleDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync();

            return PagedList<RoleDto>.ToPagedList(rolesDtoList,
                paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }
        //Override method here. 
    }
}
