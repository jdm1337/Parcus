using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using Parcus.Domain.Permission;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

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
            var typeResult = await _findInstrumentService.GetByFigiAsync(addTransactionRequest.Figi);
            if (!typeResult.Successed)
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
                BrokeragePortfolioId = userPortfolio.Id,
                

            };
            var transactionResult = await _portfolioOperationService.AddTransactionAsync
                (
                investTransaction,
                finInstrument,
                typeResult.InstrumentType,
                addTransactionRequest.Price
                );
            if (!transactionResult.Successed)
            {
                return BadRequest();
            }
            return Ok();

            // use service for getting an instrument by figi ( 1. go to your db if this is not exist them go to api)
            // get from instrument type
            //create and mapping share or bond or fundInAccount with gotted instrument
            //save created somethinginaccount - save id of this entity
            //create invest transaction by mapping request and edding figi to field of transaction entity
            // user operation service for add transaction, validate etc

        }
        [HttpGet]
        [AllowAnonymous]
        [Route("tinkoff-api/GetTypeByFigi{figi}")]
        public async Task<IActionResult> Test(string figi)
        {
            await _findInstrumentService.GetByFigiAsync(figi);
            /*
            Console.WriteLine(figi);
            var investApiClient = new ServiceCollection()
            .AddInvestApiClient((_, x) => x.AccessToken = "t.eq74p1ZM83imCpMPAdoBvqWeExQMwA0WTesE4KngWN6YbDQIR2v-0vX0qR9eZJAX_1j8_M4wp_93xpvoNKRNSA")
            .BuildServiceProvider()
                .GetService<InvestApiClient>();
            var instrumentRequest = new InstrumentRequest();
            instrumentRequest.Id = figi;
            string instrumentType;
            var instrument = investApiClient.Instruments.Etfs();
            foreach(var p in instrument.Instruments)
                Console.WriteLine(p.Name);
            */
            /*
            try
            {
                var shares = await investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest);
                instrumentType = shares.Instrument.InstrumentType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            // var type = await _findInstrumentService.GetTypeByFigiAsync(figi);
            if (instrumentType == null)
            {
                return NotFound();
            }
            */
            return Ok();
        }



    }
}
