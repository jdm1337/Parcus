using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Results;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;


namespace Parcus.Services.Services
{
    public class DataInstrumentService : IDataInstrumentService
    {
        private readonly Domain.Settings.InvestApiSettings _investApiSettings ;
        private readonly InvestApiClient? _investApiClient;
        public DataInstrumentService(IOptionsMonitor<Domain.Settings.InvestApiSettings> optionsMonitor)
        {
            _investApiSettings = optionsMonitor.CurrentValue;
            _investApiClient = new ServiceCollection()
            .AddInvestApiClient((_, x) => x.AccessToken = _investApiSettings.ReadonlyToken)
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
                instrumentType = (await _investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest))
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
                var instrumentName = (await _investApiClient.Instruments.GetInstrumentByAsync(instrumentRequest))
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
