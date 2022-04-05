using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;

namespace Parcus.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        protected IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public BaseController(
            IUnitOfWork unitOfWork,
            IMapper mapper) 
            
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
    }
}
