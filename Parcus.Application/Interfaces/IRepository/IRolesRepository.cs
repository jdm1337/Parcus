﻿using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Identity;
using Parcus.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IRolesRepository : IGenericRepository<Role>
    {
        Task<PagedList<RoleDto>> GetRoles(RoleParameters paginationParameters);
    }
}
