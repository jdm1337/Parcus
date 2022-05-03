using Parcus.Domain.DTO.Entities;

namespace Parcus.Domain.DTO.Outgoing
{
    public class UserSelectResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
