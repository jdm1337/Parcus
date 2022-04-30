using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;


namespace Parcus.Api.Controllers.v1
{
    public class TestController : BaseController
    {
        private readonly IHangfireInjectService _hangfireInjectService;
        private readonly IInstrumentStateService _instrumentStateService;
        private readonly ISeedDataService _seedDataService;
        public TestController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IInstrumentStateService instrumentStateService,
            IHangfireInjectService hangfireInjectService) : base(unitOfWork, mapper)
        {
            _instrumentStateService = instrumentStateService;
            _hangfireInjectService = hangfireInjectService;
        }
        [HttpPost]
        [Route("testing")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}
