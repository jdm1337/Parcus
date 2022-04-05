using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;

namespace Parcus.Api.Controllers.v1
{
    public class AssetsController : BaseController
    {
        public AssetsController(
            IUnitOfWork unitOfWork,
            IMapper mapper) :base(unitOfWork, mapper)
        {

        }
        
    }
}
