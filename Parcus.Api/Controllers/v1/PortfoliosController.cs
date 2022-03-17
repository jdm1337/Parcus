using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Permission;

namespace Parcus.Api.Controllers.v1
{
    public class PortfoliosController : BaseController
    {
        private readonly IAuthService _authService;
        public PortfoliosController(
            IUnitOfWork unitOfWork,
            IAuthService authService) : base(unitOfWork)
        {
            _authService = authService;
        }
        // Add portfolio method included in prime version and here need to implement
        // system of checking current amount of portfolios
        [HttpPost]
        [Route("Add")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> Add()
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var portfolio = new BrokeragePortfolio
            {
                UserId = Convert.ToInt32(userId)   
            };
            _unitOfWork.Portfolios.AddAsync(portfolio);
            _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpPost]
        [Route("AddBroker")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> AddBroker([FromBody]AddBrokerRequest addBrokerRequest)
        {
            return Ok();
        }
        [HttpPost]
        [Route("AddTransaction")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> AddTranscation([FromBody] AddTransactionRequest addTransactionRequest)
        {
            return Ok();
        }
        
    }
}
