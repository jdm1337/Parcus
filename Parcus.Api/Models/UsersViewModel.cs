using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Pagination;

namespace Parcus.Web.Models
{
    public class UsersViewModel
    {
        public PagedList<UserDto> Users{ get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
