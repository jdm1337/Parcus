using Parcus.Domain.Identity;

namespace Parcus.Api.Models.DTO.Outgoing
{
    public class UserSelectResponse
    {
        public IQueryable<User> Users { get; set; }
    }
}
