using Microsoft.Extensions.DependencyInjection;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Results;
using System.Threading;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using ParcusInvestEntity = Parcus.Domain.Invest.InstrumentModels;

namespace Parcus.Services.Services
{
    public class FindInstrumentService : IFindInstrumentService
    {
        protected IUnitOfWork _unitOfWork;
        //private readonly InstrumentsService.InstrumentsServiceClient _apiService;
        private readonly InvestApiClient? investApiClient;
        
        public FindInstrumentService(IUnitOfWork unitOfWork
                                     )
        {
            _unitOfWork = unitOfWork;
            investApiClient = new ServiceCollection()
            .AddInvestApiClient((_, x) => x.AccessToken = "t.eq74p1ZM83imCpMPAdoBvqWeExQMwA0WTesE4KngWN6YbDQIR2v-0vX0qR9eZJAX_1j8_M4wp_93xpvoNKRNSA")
            .BuildServiceProvider()
            .GetService<InvestApiClient>();  
        }
       

        public async Task<FindInstrumentResult> GetByFigiAsync(string instrumentCode)
        {
            /*
            var instrument = investApiClient.Instruments.Shares();
            
            foreach (var p in instrument.Instruments)
                Console.WriteLine(p.Name);
            return null;
            */
            var result = new FindInstrumentResult();
            var instrumentRequest = new InstrumentRequest
            {
                Id = instrumentCode,
                IdType = InstrumentIdType.Figi
            };
            try
            {
                var instrumentResponse = await investApiClient?.Instruments?.GetInstrumentByAsync(instrumentRequest);
                var findedInstrument = instrumentResponse.Instrument;
                Console.WriteLine(findedInstrument);
                
                switch (findedInstrument.InstrumentType)
                {
                    //implement other cases
                    case "share":
                        result.InstrumentType = "share";
                        result.Shares.Add(new ParcusInvestEntity.Shares.Share
                        {
                            Figi = findedInstrument.Figi,
                            Tiker = findedInstrument.Ticker,
                            ShareName = findedInstrument.Name,
                            Currency = findedInstrument.Currency
                        });
                        break;
                    case "bond":
                        result.InstrumentType = "bond";
                        result.Bonds.Add(new ParcusInvestEntity.Bonds.Bond
                        {
                            Figi = findedInstrument.Figi,
                            Tiker = findedInstrument.Ticker,
                            BondName = findedInstrument.Name,
                            Currency = findedInstrument.Currency
                        });
                        break;
                    case "etf":
                        result.InstrumentType = "fund";
                        result.Funds.Add(new ParcusInvestEntity.Funds.Fund
                        {
                            Figi = findedInstrument.Figi,
                            Tiker = findedInstrument.Ticker,
                            FundName = findedInstrument.Name,
                            Currency = findedInstrument.Currency
                        });
                        break;

                }
                result.Successed = true;
                return result;
                
            }
            catch (Exception ex)
            {
                result.Successed = false;
                Console.WriteLine(ex.Message);
                return result;
            }  
        }

        public async Task<FindInstrumentResult> GetTypeByFigiAsync(string instrumentCode)
        {
            var result = new FindInstrumentResult();

            var instrumentRequest = new InstrumentRequest
            {
                Id = instrumentCode,
                IdType = InstrumentIdType.Figi
            };

            string instrumentType;
            try
            {
                instrumentType = (await investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest))
                    .Instrument
                    .InstrumentType;
                result.Successed = true;
                result.InstrumentType = instrumentType;
                return result;
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.Successed = false;
                return result;
            }

        }
    }
}
