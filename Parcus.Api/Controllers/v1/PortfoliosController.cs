using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.DTO.Outgoing;
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
            IMapper mapper,
            IPortfolioOperationService portfolioOperationService,
            IAuthService authService,
            IDataInstrumentService findInstrumentService) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _authService = authService;
            _portfolioOperationService = portfolioOperationService;
            _dataInstrumentService = findInstrumentService;
        }

        /// <summary>
        /// Добавление инвестиционного портфеля
        /// </summary>
        [HttpPost]
        [Route("Add/")]
        [Authorize(Permissions.Portfolios.Add)]
        public async Task<IActionResult> Add(AddPortfolioRequest addPortfolioRequest)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            if (userId == null) { return BadRequest(); }

            var userPortfolios = await _unitOfWork.Portfolios.GetByUserIdAsync(userId);
            var existedPortfolio = userPortfolios.Any(p => p.Name == addPortfolioRequest.Name);

            if (existedPortfolio)
            {
                return BadRequest("Portfolio already exist.");
            }
            var portfolio = _mapper.Map<BrokeragePortfolio>(addPortfolioRequest);
            portfolio.UserId = Convert.ToInt32(userId);
            
            var createdPortfolio =  await _unitOfWork.Portfolios.AddAsync(portfolio);
            
            if(createdPortfolio == null)
            { 
                return BadRequest();
            }

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<AddPortfolioResponse>(createdPortfolio));
        }
        /// <summary>
        /// Добавление брокера
        /// </summary>
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
            var broker = _mapper.Map<Broker>(addBrokerRequest);
            var addBrokerResult = await _portfolioOperationService.AddBroker(userPortfolio, broker);

            if (!addBrokerResult.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Добавление транзакции по портфелю
        /// </summary>
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

            var finInstrument = _mapper.Map<InstrumentsInPortfolio>(addTransactionRequest);
            finInstrument.UserId = Convert.ToInt32(userId);
            finInstrument.InstrumentType = defineTypeResult.Item;
            finInstrument.BrokeragePortfolioId = userPortfolio.Id;
            finInstrument.Name = instrumentNameResult.Item;

            // example of datetime "2009-05-08 14:40:52,531"
            
            var investTransaction = _mapper.Map<InvestTransaction>(addTransactionRequest);
            investTransaction.BrokeragePortfolioId = userPortfolio.Id;
            investTransaction.BrokeragePortfolio = userPortfolio;
            investTransaction.Instrument = finInstrument;

            if(investTransaction.TransactionDate == null || investTransaction.TransactionType == null) return BadRequest("Invalid transaction.");

            var transactionResult = await _portfolioOperationService.AddTransactionAsync(investTransaction);
           
            if (!transactionResult.Succeeded)
            {
                return BadRequest("Invalid Transaction.");
            }
            return Ok();
        }
        /// <summary>
        /// Получение активов портфеля
        /// </summary>
        [HttpGet]
        [Authorize(Permissions.Portfolios.GetInstruments)]
        [Route("GetInstruments/{id}")]
        public async Task<IActionResult> GetInstruments(string Id)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(Id);

            if (userId != Convert.ToString(portfolio.UserId) || portfolio == null) { return BadRequest(); }

            var response = new GetInstrumentsResponse();
            
            response.Shares = (await _unitOfWork.InstrumentsInPortfolio.GetByPortfolioIdAndType(portfolio.Id, InstrumentTypes.share))
                .Select(instrument => _mapper.Map<InstrumentInPortfolioDto>(instrument)).ToList();
            response.SharesAmount = response.Shares.Count();

            response.Bonds = (await _unitOfWork.InstrumentsInPortfolio.GetByPortfolioIdAndType(portfolio.Id, InstrumentTypes.bond))
                .Select(instrument => _mapper.Map<InstrumentInPortfolioDto>(instrument)).ToList();
            response.BondsAmount = response.Bonds.Count();

            response.Etf = (await _unitOfWork.InstrumentsInPortfolio.GetByPortfolioIdAndType(portfolio.Id, InstrumentTypes.etf))
                .Select(instrument => _mapper.Map<InstrumentInPortfolioDto>(instrument)).ToList();
            response.EtfAmount = response.Etf.Count();

            return Ok(response);
        }


    }
}
