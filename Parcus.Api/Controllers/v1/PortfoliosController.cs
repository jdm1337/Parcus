using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Permission;

namespace Parcus.Api.Controllers.v1
{
    public class PortfoliosController : BaseController
    {
        public PortfoliosController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }
        [HttpPost]
        [Route("Add")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> Add([FromBody]AddPortfolioRequest addPortfolioRequest)
        {

            return Ok();
        }
        
    }
}
