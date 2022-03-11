using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;

namespace Parcus.Api.Controllers.v1
{
    public class PortfoliosController : BaseController
    {
        public PortfoliosController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
    }
}
