using Parcus.Domain.Identity;

namespace Parcus.Domain.DTO.Outgoing
{
    public class UserSelectResponse
    {
        public IQueryable<User> Users { get; set; }
    }
}
