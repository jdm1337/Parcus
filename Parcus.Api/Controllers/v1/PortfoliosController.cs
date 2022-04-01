using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Identity;
using Parcus.Domain.Invest.Brokers;
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
        private readonly IDataInstrumentService _dataInstrumentService;
        private readonly UserManager<User> _userManager;
        public PortfoliosController(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IPortfolioOperationService portfolioOperationService,
            IAuthService authService,
            IDataInstrumentService findInstrumentService) : base(unitOfWork)
        {
            _userManager = userManager;
            _authService = authService;
            _portfolioOperationService = portfolioOperationService;
            _dataInstrumentService = findInstrumentService;
        }
        // Add portfolio method included in prime version and here need to implement
        // system of checking current amount of portfolios
        [HttpPost]
        [Route("Add")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> Add(AddPortfolioRequest addPortfolioRequest)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            if (userId == null) { return BadRequest(); }

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
            return Ok(createdPortfolio.Id);
        }
        [HttpPost]
        [Route("AddBroker")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> AddBroker([FromBody]AddBrokerRequest addBrokerRequest)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);

            if(userId == null) { return BadRequest(); }

            var userPortfolio = await _unitOfWork.Portfolios.GetByIdAsync(addBrokerRequest.PortfolioId);

            if(userPortfolio == null)
            {
                return BadRequest();
            }
            var broker = new Broker
            {
                Name = addBrokerRequest.BrokerName,
                Percentage = addBrokerRequest.Percentage
            };
            var addBrokerResult = await _portfolioOperationService.AddBroker(userPortfolio, broker);

            if (!addBrokerResult.Succeeded)
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
            var userPortfolio = await _unitOfWork.Portfolios.GetByIdAsync(addTransactionRequest.PortfolioId);

            if (userPortfolio == null)
            {
                return BadRequest();
            }
            var defineTypeResult = await _dataInstrumentService.DefineTypeByFigi(addTransactionRequest.Figi);
            if (!defineTypeResult.Succeeded)
            {
                return NotFound("Finance instrument not found.");
            }
            var instrumentNameResult = await _dataInstrumentService.DefineInstrumentNameByFigi(addTransactionRequest.Figi);
            Console.WriteLine("stock name: " + instrumentNameResult.Item);
            var finInstrument = new InstrumentsInPortfolio
            {
                UserId = Convert.ToInt32(userId),

                Figi = addTransactionRequest.Figi,
                Amount = addTransactionRequest.Amount,
                InstrumentType = defineTypeResult.Item,
                BrokeragePortfolioId = userPortfolio.Id,
                Name = instrumentNameResult.Item,


            };
            Transactions transactionType;
            try
            {
                transactionType = (Transactions)Enum.Parse(typeof(Transactions), addTransactionRequest.TransactionType);
            }
            catch(Exception ex)
            {
                return BadRequest($"Invalid transaction format {addTransactionRequest.TransactionType}.");
            }

            Console.WriteLine(transactionType.ToString());
            // example of datetime "2009-05-08 14:40:52,531"
            DateTime transactionDate; 
            try
            {
                transactionDate = DateTime.ParseExact(addTransactionRequest.TransactionDate, 
                    "yyyy-MM-dd HH:mm:ss,fff",
                    System.Globalization.CultureInfo.InvariantCulture);
            }catch (FormatException)
            {
                return BadRequest("Invalid DateTimeFormat.");
            }
            var investTransaction = new InvestTransaction
            {
                TransactionType = transactionType,
                TransactionDate = transactionDate,
                InstrumentPrice = addTransactionRequest.Price,
                BrokeragePortfolioId = userPortfolio.Id,
                BrokeragePortfolio = userPortfolio,
                Instrument = finInstrument,
                Amount = addTransactionRequest.Amount,
            };
            var transactionResult = await _portfolioOperationService.AddTransactionAsync(investTransaction);
           
            if (!transactionResult.Succeeded)
            {
                return BadRequest("Invalid Transaction");
            }
            return Ok();
        }
        [HttpGet]
        [Authorize(Permissions.Portfolios.GetInstruments)]
        [Route("Instruments/{portfolioId}")]
        public async Task<IActionResult> GetInstruments(string portfolioId)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(portfolioId);

            if (userId != Convert.ToString(portfolio.UserId)) { return BadRequest(); }

            var instruments = await _unitOfWork.InstrumentsInPortfolio.GetByPortfolioId(portfolio.Id);
            return Ok(instruments);
        }


    }
}
