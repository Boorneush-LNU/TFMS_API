using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public interface IVehicleRepository
        {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task<Vehicle> GetByRegistrationNumberAsync(string registrationNumber); // Add this line
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(int id);
        }
    }
