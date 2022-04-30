using Parcus.Domain.Results;


namespace Parcus.Application.Interfaces.IServices
{
    public interface IInstrumentStateService
    {
        Task SeedInfoAsync();
        void UpdatePrice();
    }
}
