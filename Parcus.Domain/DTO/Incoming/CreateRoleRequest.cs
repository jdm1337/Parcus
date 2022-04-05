using System.ComponentModel.DataAnnotations;

namespace Parcus.Domain.DTO.Incoming
{
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string[] Permissions { get; set; }
    }
}
