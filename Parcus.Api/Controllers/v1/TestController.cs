using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;

namespace Parcus.Api.Controllers.v1
{
    public class TestController : BaseController
    {
        public TestController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpPost]
        [Route("testing")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}
