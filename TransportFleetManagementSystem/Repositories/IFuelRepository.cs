using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public interface IFuelRepository
        {
        Task<IEnumerable<Fuel>> GetAllAsync();
        Task<Fuel> GetByIdAsync(int id);
        Task AddAsync(Fuel fuel);
        Task UpdateAsync(Fuel fuel);
        Task DeleteAsync(int id);
        Task<decimal> CalculateTotalCostAsync();
        Task<decimal> GetLatestFuelPriceAsync();
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<object>> GetMonthlyExpensesAsync();
        }
    }
