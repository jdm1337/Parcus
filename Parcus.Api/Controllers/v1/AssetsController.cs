using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Permission;

namespace Parcus.Api.Controllers.v1
{
    public class AssetsController : BaseController
    {
        public AssetsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet]
        [Route("{figi}")]
        [Authorize(Permissions.Account.Base)]
        public async Task<IActionResult> Add(string figi)
        {
            return Ok();
        }
    }
}
