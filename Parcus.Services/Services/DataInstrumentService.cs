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
                
                instrumentType = (await investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest))
                    .Instrument
                    .InstrumentType;
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
        public async Task<Result<string>> DefineInstrumentNameByFigi(string figi)
        {
            var result = new Result<string>();

            var instrumentRequest = new InstrumentRequest
            {
                Id = figi,
                IdType = InstrumentIdType.Figi
            };
            try
            {
                var instrumentName = (await investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest))
                    .Instrument
                    .Name;
                result.Succeeded = true;
                result.Item = instrumentName;
                return result;
            }
            catch(Exception ex)
            {
                return result;
            }
        }
    }
}
