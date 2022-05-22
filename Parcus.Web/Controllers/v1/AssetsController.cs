using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.Permission;
using Parcus.Persistence.Data;

namespace Parcus.Web.Controllers.v1
{
    public class AssetsController : BaseController
    {
        protected AppDbContext _context;
        public AssetsController(
            IUnitOfWork unitOfWork,
            AppDbContext context,
            IMapper mapper) : base(unitOfWork, mapper)
        {
            _context = context;
        }

        /// <summary>
        /// Get data on an asset 
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("{figi}")]
        public async Task<IActionResult> GetInstrument(string figi)
        {
            var findedInstrument = await _unitOfWork.Instruments.GetByFigi(figi);
            if (findedInstrument == null)
                return NotFound("Instrument was not found.");
            
            return Ok(_mapper.Map<InstrumentDto>(findedInstrument));
        }

        /// <summary>
        /// Get dividend payments on an asset
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("{figi}/DividentYield")]
        public async Task<IActionResult> GetDividentYield(string figi)
        {
            var dividendYield = _context.Instruments.Where(x => x.Figi == figi).Select(x => x.DividendYield).FirstOrDefault();
            if (dividendYield == null)
                return NotFound("Instrument was not found.");

            return Ok(dividendYield);
        }

        /// <summary>
        /// Get the latest asset prices 
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("{figi}/LastPrice")]
        public async Task<IActionResult> GetLastPrice(string figi)
        {
            var lastPrice = _context.Instruments.Where(x => x.Figi == figi).Select(x => x.CurrentPrice).FirstOrDefault();
            if (lastPrice == null)
                return NotFound("Instrument was not found.");

            return Ok(lastPrice);
            return Ok(figi);
        }
        /// <summary>
        /// Get historical candles for an asset
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        /*
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("{figi}/OrderBook")]
        public async Task<IActionResult> GetOrderBook(string figi)
        {
            return Ok(figi);
        }
        */
    }
}
