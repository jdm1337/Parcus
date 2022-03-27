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
        private readonly IPortfolioOperationService _portfolioOperationService;
        public PortfoliosController(
            IUnitOfWork unitOfWork,
            IPortfolioOperationService portfolioOperationService,
            IAuthService authService) : base(unitOfWork)
        {
            _authService = authService;
            _portfolioOperationService = portfolioOperationService;
        }
        // Add portfolio method included in prime version and here need to implement
        // system of checking current amount of portfolios
        [HttpPost]
        [Route("Add")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> Add(AddPortfolioRequest addPortfolioRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity); 
            var userPortfolios = await _unitOfWork.Portfolios.GetByUserIdAsync(userId);
            var existedPortfolio = userPortfolios.Any(p => p.Name == addPortfolioRequest.PortfolioName);

            if (existedPortfolio)
            {
                return BadRequest();
            }
            var portfolio = new BrokeragePortfolio
            {
                UserId = Convert.ToInt32(userId),
                CreatedDate = DateTime.Now,
                Name = addPortfolioRequest.PortfolioName
            };
            var createdPortfolio =  await _unitOfWork.Portfolios.AddAsync(portfolio);
            if(createdPortfolio == null)
            { 
                return BadRequest();
            }
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpPost]
        [Route("AddBroker")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> AddBroker([FromBody]AddBrokerRequest addBrokerRequest)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var userPortfolio = await _unitOfWork.Portfolios.GetByUserIdAndNameAsync(userId, addBrokerRequest.PortfolioName);

            if(userPortfolio == null)
            {
                return BadRequest();
            }
            var isSucceeded = await _portfolioOperationService.AddBroker(userPortfolio, addBrokerRequest.BrokerName, addBrokerRequest.Percentage);

            if (!isSucceeded)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpPost]
        [Route("AddTransaction")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest addTransactionRequest)
        {
            return Ok();
        }
        
    }
}
