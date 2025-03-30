using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Data;
using TransportFleetManagementSystem.Models;

namespace TransportFleetManagementSystem.Repositories
    {
    public class DriverRepository : IDriverRepository
        {
        private readonly AppDbContext _context;

        public DriverRepository(AppDbContext context)
            {
            _context = context;
            }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
            {
            // Get all drivers
            return await _context.Drivers.ToListAsync();
            }

        public async Task<Driver> GetDriverByIdAsync(int id)
            {
            // Get a driver by ID
            return await _context.Drivers.FindAsync(id);
            }

        public async Task AddDriverAsync(Driver driver)
            {
            // Add a new driver
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            }

        public async Task UpdateDriverAsync(Driver driver)
            {
            // Update an existing driver
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
            }

        public async Task DeleteDriverAsync(int id)
            {
            // Delete a driver
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
                {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
                }
            }
        }
    }
    
