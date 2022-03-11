using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Identity
{
    public class RoleClaim: IdentityRoleClaim<int>
    {
        public string? Description { get; set; }
    }
}
