using System.ComponentModel.DataAnnotations;

namespace Parcus.Domain.DTO.Incoming
{
    public class AddToRoleRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
