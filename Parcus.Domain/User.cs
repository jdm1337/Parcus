
using Microsoft.AspNetCore.Identity;
using Parcus.Domain.Invest.PortfolioModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain
{
    public class User : IdentityUser<int>
    {
        public User() : base() { }

        public ICollection<BrokeragePortfolio>? BrokPortfolios { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
