using Microsoft.AspNetCore.Identity;
using Parcus.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
     public interface ISeedDataService
    {
        Task SeedInstrumentInfoAsync();
        Task SeedInitIdentityAsync();
    }
}
