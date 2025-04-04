using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Data;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public class TripRepository : ITripRepository
        {
        private readonly AppDbContext _context;

        public TripRepository(AppDbContext context)
            {
            _context = context;
            }

        public async Task<IEnumerable<Trip>> GetAllAsync()
            {
            return await _context.Trips.Include(t => t.Driver).Include(t => t.Vehicle).ToListAsync();
            }

        public IQueryable<Trip> GetAll()
            {
            return _context.Trips.Include(t => t.Driver).Include(t => t.Vehicle).AsQueryable();
            }

        public async Task<Trip> GetByIdAsync(int id)
            {
            return await _context.Trips.Include(t => t.Driver).Include(t => t.Vehicle).FirstOrDefaultAsync(t => t.TripId == id);
            }

        public async Task AddAsync(Trip trip)
            {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
            }

        public async Task UpdateAsync(Trip trip)
            {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
            }

        public async Task DeleteAsync(int id)
            {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
                {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                }
            }

        public async Task<IEnumerable<Driver>> GetDriversAsync()
            {
            return await _context.Drivers.ToListAsync();
            }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
            {
            return await _context.Vehicles.ToListAsync();
            }

        public async Task<bool> IsDriverOrVehicleAssignedAsync(Trip trip)
            {
            var trips = await GetAllAsync();
            return trips.Any(t => t.DriverId == trip.DriverId && t.StartTime < trip.EndTime && t.EndTime > trip.StartTime) ||
                   trips.Any(t => t.VehicleId == trip.VehicleId && t.StartTime < trip.EndTime && t.EndTime > trip.StartTime);
            }

        public async Task<IEnumerable<Trip>> GetPagedTripsAsync(int page, int pageSize, string searchString)
            {
            var tripQuery = _context.Trips.Include(t => t.Driver).Include(t => t.Vehicle).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                {
                tripQuery = tripQuery.Where(t => t.Vehicle.RegistrationNumber.Contains(searchString) ||
                                                 t.Driver.Name.Contains(searchString) ||
                                                 t.StartLocation.Contains(searchString) ||
                                                 t.EndLocation.Contains(searchString));
                }

            return await tripQuery
                .OrderBy(t => t.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }

        public async Task<int> GetTotalTripCountAsync(string searchString)
            {
            var tripQuery = _context.Trips.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                {
                tripQuery = tripQuery.Where(t => t.Vehicle.RegistrationNumber.Contains(searchString) ||
                                                 t.Driver.Name.Contains(searchString) ||
                                                 t.StartLocation.Contains(searchString) ||
                                                 t.EndLocation.Contains(searchString));
                }

            return await tripQuery.CountAsync();
            }
        }
    }
