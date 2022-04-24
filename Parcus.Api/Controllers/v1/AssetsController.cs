using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Permission;

namespace Parcus.Api.Controllers.v1
{
    public class AssetsController : BaseController
    {
        public AssetsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Получение данных по активу 
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{figi}")]
        [Authorize(Permissions.Account.Base)]
        public async Task<IActionResult> GetInstrumentData(string figi)
        {
            return Ok();
        }

        /// <summary>
        /// Получение дивидендных выплат по активу 
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DividentYield/{figi}")]
        [Authorize(Permissions.Account.Base)]
        public async Task<IActionResult> GetDividentYield(string figi)
        {
            return Ok();
        }

        /// <summary>
        /// Получение последних цен по активам  
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("LastPrice/{figi}")]
        [Authorize(Permissions.Account.Base)]
        public async Task<IActionResult> GetLastPrice(string figi)
        {
            return Ok(figi);
        }
        /// <summary>
        /// Получение исторических свечей по активу
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OrderBook/{figi}")]
        [Authorize(Permissions.Account.Base)]
        public async Task<IActionResult> GetOrderBook(string figi)
        {
            return Ok(figi);
        }
    }
}
