using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Data;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public class FuelRepository : IFuelRepository
        {
        private readonly AppDbContext _context;

        public FuelRepository(AppDbContext context)
            {
            _context = context;
            }

        // Get all fuel records
        public async Task<IEnumerable<Fuel>> GetAllAsync()
            {
            return await _context.Fuels.Include(f => f.Vehicle).ToListAsync();
            }

        // Get a fuel record by ID
        public async Task<Fuel> GetByIdAsync(int id)
            {
            // Use AsNoTracking to avoid tracking conflicts
            return await _context.Fuels.AsNoTracking().Include(f => f.Vehicle).FirstOrDefaultAsync(f => f.FuelId == id);
            }
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
            {
            return await _context.Vehicles.ToListAsync();
            }
        public async Task<IEnumerable<object>> GetMonthlyExpensesAsync()
            {
            // Fetch grouped data from the database
            var groupedData = await _context.Fuels
                .GroupBy(f => new { f.Date.Year, f.Date.Month }) // Group by Year and Month
                .Select(g => new
                    {
                    Year = g.Key.Year, // Keep the Year field
                    MonthNumber = g.Key.Month, // Keep the Month field as a number
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"), // Format the month
                    TotalExpense = g.Sum(f => f.Cost) // Calculate the sum
                    })
                .OrderBy(item => item.Year) // First order by Year
                .ThenBy(item => item.MonthNumber) // Then order by Month number
                .ToListAsync();

            // Select only the necessary fields to return to the calling code
            var formattedData = groupedData
                .Select(item => new
                    {
                    Month = item.Month, // Formatted "MMMM yyyy"
                    TotalExpense = item.TotalExpense
                    })
                .ToList();

            return formattedData;
            }
        // Add a new fuel record
        public async Task AddAsync(Fuel fuel)
            {
            fuel.Cost = fuel.FuelQuantity * (fuel.FuelPrice ?? 0); // Calculate cost
            await _context.Fuels.AddAsync(fuel);
            await _context.SaveChangesAsync();
            }

        // Update an existing fuel record
        public async Task UpdateAsync(Fuel fuel)
            {
            var existingFuel = await _context.Fuels.FindAsync(fuel.FuelId);
            if (existingFuel != null)
                {
                _context.Entry(existingFuel).State = EntityState.Detached; // Detach the tracked entity

                fuel.Cost = fuel.FuelQuantity * (fuel.FuelPrice ?? 0); // Recalculate cost
                _context.Fuels.Update(fuel); // Attach the updated entity
                await _context.SaveChangesAsync(); // Save changes
                }
            }

        // Delete a fuel record by ID
        public async Task DeleteAsync(int id)
            {
            var fuel = await _context.Fuels.FindAsync(id);
            if (fuel != null)
                {
                _context.Fuels.Remove(fuel);
                await _context.SaveChangesAsync();
                }
            }

        // Calculate total cost
        public async Task<decimal> CalculateTotalCostAsync()
            {
            return await _context.Fuels.SumAsync(f => f.Cost);
            }

        // Get the latest fuel price
        public async Task<decimal> GetLatestFuelPriceAsync()
            {
            // Sort by Date descending and then by ID descending to get the newest record in case dates are the same
            var latestFuel = await _context.Fuels
                .OrderByDescending(f => f.Date) // Sort by date to get the latest date
                .ThenByDescending(f => f.FuelId) // Sort by ID to ensure newest entry is picked when dates are equal
                .FirstOrDefaultAsync(); // Fetch the latest record

            // Return the FuelPrice of the newest record, or 0 if no records exist
            return latestFuel?.FuelPrice ?? 0;
            }




        }
    }
