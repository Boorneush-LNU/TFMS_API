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


        Task<decimal> GetLatestFuelPriceAsync(DateTime recordDate);

        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();


        Task<IEnumerable<object>> GetMonthlyExpensesAsync();
        Task<IEnumerable<Fuel>> GetPagedFuelsAsync(int page, int pageSize, string searchString);
        Task<int> GetTotalFuelCountAsync(string searchString);


        }
    }
