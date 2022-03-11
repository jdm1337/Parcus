namespace Parcus.Api.Models.DTO.Incoming
{
    public class CreateRoleRequest
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string[] Permissions { get; set; }
    }
}
