using Parcus.Domain.Results;
using Parcus.Persistence.DataSeed;

namespace Parcus.Api.Initial
{
    public class StartApp
    {
        public static async Task<Result<bool>> Invoke(WebApplication app)
        {
            var result = new Result<bool>();
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var dataSeedService = scope.ServiceProvider.GetService<DataSeeder>();
                var seedResult = await dataSeedService.Seed();
                // TODO : Implement update method and them here 
            }
            result.Succeeded = true;
            return result;
        }
    }
}
