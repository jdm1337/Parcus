using Microsoft.Extensions.DependencyInjection;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace Parcus.Services.Services
{
    public class DataInstrumentService : IDataInstrumentService
    {
        private readonly InvestApiClient? investApiClient;
        public DataInstrumentService()
        {
            investApiClient = investApiClient = new ServiceCollection()
            .AddInvestApiClient((_, x) => x.AccessToken = "t.eq74p1ZM83imCpMPAdoBvqWeExQMwA0WTesE4KngWN6YbDQIR2v-0vX0qR9eZJAX_1j8_M4wp_93xpvoNKRNSA")
            .BuildServiceProvider()
            .GetService<InvestApiClient>();
        }
        public async Task<Result<InstrumentTypes>> DefineTypeByFigi(string figi)
        {
            var result = new Result<InstrumentTypes>();

            var instrumentRequest = new InstrumentRequest
            {
                Id = figi,
                IdType = InstrumentIdType.Figi
            };
            
            string instrumentType;
            try
            {
                Console.WriteLine("mark");
                instrumentType = (await investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest))
                    .Instrument
                    .InstrumentType;
                Console.WriteLine(instrumentType);
                Console.WriteLine(InstrumentTypes.share.ToString());
                Console.WriteLine("mark2");
                
                Console.WriteLine("mark3");
                result.Succeeded = true;
                result.Item = (InstrumentTypes)Enum.Parse(typeof(InstrumentTypes), instrumentType);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }
    }
}
