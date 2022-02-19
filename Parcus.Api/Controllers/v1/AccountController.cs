using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;

namespace Parcus.Api.Controllers.v1
{
    public class PortfolioController : BaseController
    {
        public PortfolioController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }

    }
}
