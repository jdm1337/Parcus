using Hangfire;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Results;


namespace Parcus.Api.Initial
{
    public static class StartApp
    {
        public static async Task<Result<bool>> Invoke(IServiceScopeFactory scopedFactory)
        {
            var result = new Result<bool>();

            using (var scope = scopedFactory.CreateScope())
            {
                var seedDataService = scope.ServiceProvider.GetService<ISeedDataService>();
                var instrumentStateService = scope.ServiceProvider.GetService<IInstrumentStateService>();
                var hangfireInjectService = scope.ServiceProvider.GetService<IHangfireInjectService>();

                //await seedDataService.SeedInitIdentityAsync();
                //await seedDataService.SeedInstrumentInfoAsync();

                //hangfireInjectService.Initial();

            }
            result.Succeeded = true;
            return result;
        }
    }
}
