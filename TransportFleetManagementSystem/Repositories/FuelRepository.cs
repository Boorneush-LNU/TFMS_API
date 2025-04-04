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
            return await _context.Fuels
                .Include(f => f.Vehicle)
                .OrderByDescending(f => f.Date)
                .ThenByDescending(f => f.FuelId)
                .ToListAsync();
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
            var groupedData = await _context.Fuels
                .GroupBy(f => new { f.Date.Year, f.Date.Month })
                .Select(g => new
                    {
                    Year = g.Key.Year,
                    MonthNumber = g.Key.Month,
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                    TotalExpense = g.Sum(f => f.Cost)
                    })
                .OrderBy(item => item.Year)
                .ThenBy(item => item.MonthNumber)
                .ToListAsync();


            var formattedData = groupedData
                .Select(item => new
                    {
                    Month = item.Month,
                    TotalExpense = item.TotalExpense
                    })
                .ToList();

            return formattedData;
            }
        // Add a new fuel record
        public async Task AddAsync(Fuel fuel)
            {
            fuel.Cost = fuel.FuelQuantity * (fuel.FuelPrice ?? 0);
            await _context.Fuels.AddAsync(fuel);
            await _context.SaveChangesAsync();
            }

        // Update an existing fuel record
        public async Task UpdateAsync(Fuel fuel)
            {
            var existingFuel = await _context.Fuels.FindAsync(fuel.FuelId);
            if (existingFuel != null)
                {
                _context.Entry(existingFuel).State = EntityState.Detached;

                fuel.Cost = fuel.FuelQuantity * (fuel.FuelPrice ?? 0);
                _context.Fuels.Update(fuel);
                await _context.SaveChangesAsync();
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
        public async Task<decimal> GetLatestFuelPriceAsync(DateTime recordDate)
            {
            var latestFuel = await _context.Fuels
                .Where(f => f.FuelPrice.HasValue && f.FuelPrice > 0 && f.Date <= recordDate)
                .OrderByDescending(f => f.Date)
                .ThenByDescending(f => f.FuelId)
                .FirstOrDefaultAsync();

            return latestFuel?.FuelPrice ?? 0;
            }


        public async Task<IEnumerable<Fuel>> GetPagedFuelsAsync(int page, int pageSize, string searchString)
            {
            var fuelQuery = _context.Fuels.Include(f => f.Vehicle).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                {
                fuelQuery = fuelQuery.Where(f => f.Vehicle.RegistrationNumber.Contains(searchString));
                }

            return await fuelQuery
                .OrderByDescending(f => f.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }

        public async Task<int> GetTotalFuelCountAsync(string searchString)
            {
            var fuelQuery = _context.Fuels.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                {
                fuelQuery = fuelQuery.Where(f => f.Vehicle.RegistrationNumber.Contains(searchString));
                }

            return await fuelQuery.CountAsync();
            }



        }
    }
