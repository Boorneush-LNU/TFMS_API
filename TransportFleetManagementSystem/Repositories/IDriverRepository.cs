using TransportFleetManagementSystem.Models;

namespace TransportFleetManagementSystem.Repositories
    {
    public interface IDriverRepository
        {
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<Driver> GetDriverByIdAsync(int id);
        Task AddDriverAsync(Driver driver);
        Task UpdateDriverAsync(Driver driver);
        Task DeleteDriverAsync(int id);
        }
    }
