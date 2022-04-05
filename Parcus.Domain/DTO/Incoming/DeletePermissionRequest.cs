using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.DTO.Incoming
{
    public class DeletePermissionRequest
    {
        [Required]
        public string PermissionName { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
