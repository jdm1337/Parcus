using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using Parcus.Persistence.Data;

namespace Parcus.Web.Controllers.v1
{
    public class PortfoliosController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IPortfolioOperationService _portfolioOperationService;
        
        private readonly UserManager<User> _userManager;
        protected AppDbContext _context;
        public PortfoliosController(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPortfolioOperationService portfolioOperationService,
            IAuthService authService,
            AppDbContext context) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _authService = authService;
            _portfolioOperationService = portfolioOperationService;
            _context = context;
        }

        /// <summary>
        /// Add an investment portfolio
        /// </summary>
        [Authorize(Permissions.Portfolios.Add)]
        [HttpPost]
        [Route("Add/")]
        public async Task<IActionResult> Add(AddPortfolioRequest addPortfolioRequest)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            if (userId == null) 
                return BadRequest(); 

            var userPortfolios = await _unitOfWork.Portfolios.GetByUserIdAsync(userId);
            var existedPortfolio = userPortfolios.Any(p => p.Name == addPortfolioRequest.Name);

            if (existedPortfolio)
                return BadRequest("Portfolio already exist.");
            
            var portfolio = _mapper.Map<BrokeragePortfolio>(addPortfolioRequest);
            portfolio.UserId = Convert.ToInt32(userId);
            
            var createdPortfolio =  await _unitOfWork.Portfolios.AddAsync(portfolio);
            
            if(createdPortfolio == null)
                return BadRequest();

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<AddPortfolioResponse>(createdPortfolio));
        }

        /// <summary>
        /// Add a Broker
        /// </summary>
        [Authorize(Permissions.Portfolios.Add)]
        [HttpPost]
        [Route("AddBroker")]
        public async Task<IActionResult> AddBroker([FromBody]AddBrokerRequest addBrokerRequest)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);

            if(userId == null)  
                return BadRequest(); 

            var userPortfolio = await _unitOfWork.Portfolios.GetByIdAsync(addBrokerRequest.PortfolioId);

            if(userPortfolio == null)
                return BadRequest();
            
            var broker = _mapper.Map<Broker>(addBrokerRequest);
            var addBrokerResult = await _portfolioOperationService.AddBroker(userPortfolio, broker);

            if (!addBrokerResult.Succeeded)
                return BadRequest();
            
            return Ok();
        }

        /// <summary>
        /// Add a Portfolio Transaction
        /// </summary>
        [Authorize(Permissions.Portfolios.Add)]
        [HttpPost]
        [Route("AddTransaction")]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest addTransactionRequest)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var userPortfolio = await _unitOfWork.Portfolios.GetByIdAsync(addTransactionRequest.PortfolioId);

            if (userPortfolio == null) 
                return BadRequest("Portfolio does not exist.");

            var findedInstrument = await _unitOfWork.Instruments.GetByFigi(addTransactionRequest.Figi);

            if (findedInstrument == null)
                return NotFound("Finance instrument not found.");
           
            InstrumentsInPortfolio instrumentInPortfolio = new InstrumentsInPortfolio()
            { 
                InstrumentId = findedInstrument.Id,
                UserId = Convert.ToInt32(userId),
                BrokeragePortfolioId = userPortfolio.Id
            };

            // datetime format "2009-05-08 14:40:52,531"
            
            var investTransaction = _mapper.Map<InvestTransaction>(addTransactionRequest);
            investTransaction.Instrument = instrumentInPortfolio;

            var transactionResult = await _portfolioOperationService.AddTransactionAsync(investTransaction);
           
            if (!transactionResult.Succeeded)
                return BadRequest("Invalid Transaction.");
            
            return Ok();
        }

        /// <summary>
        /// Get of portfolio assets
        /// </summary>
        [Authorize(Permissions.Portfolios.GetInstruments)]
        [HttpGet]
        [Route("{id}/GetInstruments")]
        public async Task<IActionResult> GetInstruments(string id)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return NotFound();

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(id);

            if (userId != Convert.ToString(portfolio.UserId) || portfolio == null)  
                return BadRequest(); 

            var response = new GetInstrumentsResponse();

            response.Shares = await _context.InstrumentsInPortfolio
                                    .Include(i => i.Instrument)
                                    .Where(i => i.BrokeragePortfolioId == portfolio.Id)
                                    .Where(i => i.Instrument.Type == InstrumentTypes.Share)
                                    .Select(x => _mapper.Map<InstrumentInPortfolioDto>(x))
                                    .ToListAsync();
            response.SharesAmount = response.Shares.Count();

            response.Bonds = await _context.InstrumentsInPortfolio
                                    .Include(i => i.Instrument)
                                    .Where(i => i.BrokeragePortfolioId == portfolio.Id)
                                    .Where(i => i.Instrument.Type == InstrumentTypes.Bond)
                                    .Select(x => _mapper.Map<InstrumentInPortfolioDto>(x))
                                    .ToListAsync();
            response.BondsAmount = response.Bonds.Count();

            response.Etf = await _context.InstrumentsInPortfolio
                                    .Include(i => i.Instrument)
                                    .Where(i => i.BrokeragePortfolioId == portfolio.Id)
                                    .Where(i => i.Instrument.Type == InstrumentTypes.Etf)
                                    .Select(x => _mapper.Map<InstrumentInPortfolioDto>(x))
                                    .ToListAsync();
            response.EtfAmount = response.Etf.Count();

            return Ok(response);
        }
    }
}
