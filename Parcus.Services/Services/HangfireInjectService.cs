using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using Parcus.Application.Interfaces.IServices;
using Parcus.Persistence.Data;

namespace Parcus.Web.Initial
{
    public class HangfireInjectService : IHangfireInjectService
    {
        private readonly IInstrumentStateService _instrumentStateService;
        private readonly IPortfolioStateService _portfolioStateService;
        private readonly AppDbContext _appDbContext;

        private const string UpdateInstrumentsStatementJobId = "update-instrument-statement";

        private const int UpdatePriceInterval = 30;
        public HangfireInjectService(
            IInstrumentStateService instrumentStateService,
            IPortfolioStateService portfolioStateService,
            AppDbContext appDbContext)
        {
            _instrumentStateService = instrumentStateService;
            _portfolioStateService = portfolioStateService;
            _appDbContext = appDbContext;
        }
        public void Initial()
        {
            UpdateInstrumentsStatementJob();
            RecurringJob.Trigger(UpdateInstrumentsStatementJobId);
        }
        public void UpdateInstrumentsStatementJob()
        {
            RecurringJob.AddOrUpdate(UpdateInstrumentsStatementJobId, () => UpdateInstrumentsAndCalcPortfolio(), Cron.MinuteInterval(UpdatePriceInterval));
        }
        public void UpdateInstrumentsAndCalcPortfolio() 
        {
            _instrumentStateService.UpdatePrice();
            _portfolioStateService.CalculateFields();
        }
    }
}
