using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Data;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public class MaintenanceRepository : IMaintenanceRepository
        {
        private readonly AppDbContext _context;

        public MaintenanceRepository(AppDbContext context)
            {
            _context = context;
            }

        public IEnumerable<Vehicle> GetVehicles()
            {
            return _context.Vehicles.ToList();
            }

        public async Task<IEnumerable<Maintenance>> GetAllAsync()
            {
            return await _context.Maintenances.ToListAsync();
            }

        public async Task<Maintenance> GetByIdAsync(int id)
            {
            return await _context.Maintenances.FindAsync(id);
            }

        public async Task AddAsync(Maintenance maintenance)
            {
            await _context.Maintenances.AddAsync(maintenance);
            await _context.SaveChangesAsync();
            }

        public async Task UpdateAsync(Maintenance maintenance)
            {
            _context.Maintenances.Update(maintenance);
            await _context.SaveChangesAsync();
            }

        public async Task DeleteAsync(int id)
            {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance != null)
                {
                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
                }
            }

        }
    }
