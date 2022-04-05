using System.ComponentModel.DataAnnotations;

namespace Parcus.Domain.DTO.Incoming
{
    public class AddPermissionRequest
    {
        [Required]
        public string PermissionName { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
