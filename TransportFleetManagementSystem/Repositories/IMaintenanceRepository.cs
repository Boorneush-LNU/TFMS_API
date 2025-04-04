using System.Collections;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public interface IMaintenanceRepository
        {
        Task<IEnumerable<Maintenance>> GetAllAsync();
        Task<Maintenance> GetByIdAsync(int id);
        Task AddAsync(Maintenance maintenance);
        Task UpdateAsync(Maintenance maintenance);
        Task DeleteAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Maintenance> GetByVehicleIdAsync(int vehicleId);
        Task<bool> MaintenanceExistsAsync(int vehicleId);
        }
    }
