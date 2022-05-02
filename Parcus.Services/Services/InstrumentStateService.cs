using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Persistence.Data;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace Parcus.Services.Services
{
    public class InstrumentStateService : IInstrumentStateService
    {
        private readonly Domain.Settings.InvestApiSettings _investApiSettings;
        private readonly InvestApiClient? _investApiClient;
        protected AppDbContext _context;
        public readonly IMapper _mapper;

        private const int NanoConst = 1000000000;
        public InstrumentStateService(
            IOptionsMonitor<Domain.Settings.InvestApiSettings> optionsMonitor,
            AppDbContext context,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _context = context;
            _investApiSettings = optionsMonitor.CurrentValue;
            _investApiClient = new ServiceCollection()
                .AddInvestApiClient((_, x) => x.AccessToken = _investApiSettings.ReadonlyToken)
                .BuildServiceProvider()
                .GetService<InvestApiClient>();
        }
        public async Task SeedInfoAsync()
        {
            var shares = (await _investApiClient.Instruments.SharesAsync())
               .Instruments
               .Select(x => new Domain.Invest.InstrumentModels.Instrument()
               {
                    Name = x.Name,
                    Type = InstrumentTypes.Share,
                    Isin = x.Isin,
                    Figi = x.Figi,
                    Tiker = x.Ticker,
                    Country = x.CountryOfRiskName,
               });
            
            var bonds = (await _investApiClient.Instruments.BondsAsync())
                .Instruments
                .Select(x => new Domain.Invest.InstrumentModels.Instrument()
                {
                    Name = x.Name,
                    Type = InstrumentTypes.Bond,
                    Isin = x.Isin,
                    Figi = x.Figi,
                    Tiker = x.Ticker,
                    Country = x.CountryOfRiskName,
                });

            var etfs = (await _investApiClient.Instruments.EtfsAsync())
                .Instruments
                .Select(x => new Domain.Invest.InstrumentModels.Instrument()
                {
                    Name = x.Name,
                    Type = InstrumentTypes.Etf,
                    Isin = x.Isin,
                    Figi = x.Figi,
                    Tiker = x.Ticker,
                    Country = x.CountryOfRiskName,
                });
            await _context.Instruments.BulkInsertAsync(shares);
            await _context.Instruments.BulkInsertAsync(bonds);
            await _context.Instruments.BulkInsertAsync(etfs);
        }
        public void UpdatePrice()
        {
            var instrumentList = _context.Instruments
                .Select(x => new Domain.Invest.InstrumentModels.Instrument
                        {
                             Figi = x.Figi
                        })
                .ToList();
            var request = new GetLastPricesRequest();
            
            request.Figi.Add(instrumentList.Select(x=> x.Figi)); // adding list of figi to request

            var lastPriceResp = _investApiClient.MarketData.GetLastPrices(request);

            var lastPriceList = lastPriceResp.LastPrices;

            for (int i = 0; i < lastPriceList.Count; i++)
            {
                try
                {
                    instrumentList[i].CurrentPrice = lastPriceList[i].Price?.Units + ((double)lastPriceList[i].Price?.Nano / (double)NanoConst);   
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            _context.Instruments.BulkUpdate(instrumentList, options =>
            {
                options.ColumnInputExpression = instrument => new { instrument.CurrentPrice };
                options.ColumnPrimaryKeyExpression = instrument => instrument.Figi;
            }); 
        }     
    }
}
