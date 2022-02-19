using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;

namespace Parcus.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        protected IUnitOfWork _unitOfWork;
        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
