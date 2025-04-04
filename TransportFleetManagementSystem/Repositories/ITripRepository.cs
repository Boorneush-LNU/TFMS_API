using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public interface ITripRepository
        {
        Task<IEnumerable<Trip>> GetPagedTripsAsync(int page, int pageSize, string searchString);
        Task<int> GetTotalTripCountAsync(string searchString);
        Task<IEnumerable<Trip>> GetAllAsync();
        Task<Trip> GetByIdAsync(int id);
        Task AddAsync(Trip trip);
        Task UpdateAsync(Trip trip);
        Task DeleteAsync(int id);
        Task<IEnumerable<Driver>> GetDriversAsync(); // Add this method
        Task<IEnumerable<Vehicle>> GetVehiclesAsync(); // Add this method
        Task<bool> IsDriverOrVehicleAssignedAsync(Trip trip);

        }
    }
