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

        public async Task<Maintenance> GetByVehicleIdAsync(int vehicleId)
            {
            return await _context.Maintenances
                .FirstOrDefaultAsync(m => m.VehicleId == vehicleId && m.Status == "Under Maintenance");
            }

        public async Task<IEnumerable<Maintenance>> GetAllAsync()
            {
            return await _context.Maintenances.Include(m => m.Vehicle).ToListAsync();
            }

        public async Task<Maintenance> GetByIdAsync(int id)
            {
            return await _context.Maintenances.AsNoTracking().Include(m => m.Vehicle).FirstOrDefaultAsync(m => m.MaintenanceId == id);
            }

        public async Task AddAsync(Maintenance maintenance)
            {
            if (await MaintenanceExistsAsync(maintenance.VehicleId))
                {
                throw new InvalidOperationException("A maintenance record already exists for this vehicle.");
                }

            await _context.Maintenances.AddAsync(maintenance);
            await _context.SaveChangesAsync();
            }

        public async Task UpdateAsync(Maintenance maintenance)
            {
            var existingMaintenance = await _context.Maintenances.FindAsync(maintenance.MaintenanceId);
            if (existingMaintenance != null)
                {
                _context.Entry(existingMaintenance).State = EntityState.Detached;
                _context.Maintenances.Update(maintenance);
                await _context.SaveChangesAsync();
                }
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

        public async Task<bool> MaintenanceExistsAsync(int vehicleId)
            {
            return await _context.Maintenances
                .AnyAsync(m => m.VehicleId == vehicleId && m.Status == "Under Maintenance");
            }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
            {
            return await _context.Vehicles.ToListAsync();
            }
        }
    }
