using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using Parcus.Domain.Permission;

namespace Parcus.Api.Controllers.v1
{
    public class PortfoliosController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IPortfolioOperationService _portfolioOperationService;
        private readonly IFindInstrumentService _findInstrumentService;
        public PortfoliosController(
            IUnitOfWork unitOfWork,
            IPortfolioOperationService portfolioOperationService,
            IAuthService authService,
            IFindInstrumentService findInstrumentService) : base(unitOfWork)
        {
            _authService = authService;
            _portfolioOperationService = portfolioOperationService;
            _findInstrumentService = findInstrumentService;
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
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var userPortfolio = await _unitOfWork.Portfolios.GetByUserIdAndNameAsync(userId, addTransactionRequest.PortfolioName);

            if (userPortfolio == null)
            {
                return BadRequest();
            }
            var instrumentSearchResult = await _findInstrumentService.SearchAsync(addTransactionRequest.Figi);
            if (!instrumentSearchResult.Successed)
            {
                return NotFound("Finance instrument not found.");
            }
            var finInstrument = new FinInstrumentInPortfolio
            {
                Figi = addTransactionRequest.Figi,
                Amount = addTransactionRequest.Amount,
            };

            Enum.TryParse(addTransactionRequest.TransactionType, out Transactions transactionType);
            if(transactionType == null)
            {
                return BadRequest($"Invalid transaction format{addTransactionRequest.TransactionType}");
            }

            // example of datetime "2009-05-08 14:40:52,531"
            DateTime transactionDate; 
            try
            {
                transactionDate = DateTime.ParseExact(addTransactionRequest.TransactionDate, "yyyy-MM-dd HH:mm:ss,fff",
                                           System.Globalization.CultureInfo.InvariantCulture);
            }catch (FormatException)
            {
                return BadRequest("Invalid DateTimeFormat.");
            }
            var investTransaction = new InvestTransaction
            {
                TransactionType = transactionType,
                TransactionDate = transactionDate,

            };
            var transactionResult = await _portfolioOperationService.AddTransactionAsync
                (
                investTransaction,
                finInstrument,
                instrumentSearchResult.InstrumentType,
                addTransactionRequest.Price
                );

            // use service for getting an instrument by figi ( 1. go to your db if this is not exist them go to api)
            // get from instrument type
            //create and mapping share or bond or fundInAccount with gotted instrument
            //save created somethinginaccount - save id of this entity
            //create invest transaction by mapping request and edding figi to field of transaction entity
            // user operation service for add transaction, validate etc

        }
        
    }
}
