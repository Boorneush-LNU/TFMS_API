using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem.Controllers
    {
    public class FuelController : Controller
        {
        private readonly IFuelRepository _fuelRepository;

        public FuelController(IFuelRepository fuelRepository)
            {
            _fuelRepository = fuelRepository;
            }


        public async Task<IActionResult> Index(int page = 1, string searchString = "")
            {
            int pageSize = 10;


            var fuelRecords = await _fuelRepository.GetPagedFuelsAsync(page, pageSize, searchString);
            int totalItems = await _fuelRepository.GetTotalFuelCountAsync(searchString);


            ViewBag.CurrentPage = page;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchString = searchString;

            ViewBag.TotalFuelQuantity = fuelRecords.Sum(f => f.FuelQuantity);
            ViewBag.TotalCost = await _fuelRepository.CalculateTotalCostAsync();
            ViewBag.TodaysPrice = await _fuelRepository.GetLatestFuelPriceAsync(DateTime.Now);

            return View(fuelRecords);
            }


        // GET: Fuel/Create
        public async Task<IActionResult> Create()
            {
            ViewBag.Vehicles = new SelectList(await _fuelRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber");
            return View();
            }

        // POST: Fuel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fuel fuel)
            {
            if (!string.IsNullOrEmpty(fuel.VehicleId.ToString()) && fuel.FuelQuantity > 0 && fuel.Date <= DateTime.Now)
                {
                if (!fuel.FuelPrice.HasValue || fuel.FuelPrice == 0)
                    {
                    fuel.FuelPrice = await _fuelRepository.GetLatestFuelPriceAsync(fuel.Date);
                    }


                fuel.Cost = fuel.FuelQuantity * (fuel.FuelPrice ?? 0);


                await _fuelRepository.AddAsync(fuel);

                return RedirectToAction(nameof(Index));
                }
            else
                {
                ViewBag.Vehicles = new SelectList(await _fuelRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", fuel.VehicleId);

                return View(fuel);
                }
            }


        // GET: Fuel/Edit/{id}
        public async Task<IActionResult> Edit(int id)
            {
            var fuel = await _fuelRepository.GetByIdAsync(id);
            if (fuel == null)
                {
                return NotFound();
                }

            ViewBag.Vehicles = new SelectList(await _fuelRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", fuel.VehicleId);
            return View(fuel);
            }

        // POST: Fuel/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Fuel fuel)
            {
            if (fuel.VehicleId > 0 && fuel.FuelQuantity > 0 && fuel.Date <= DateTime.Now)
                {
                await _fuelRepository.UpdateAsync(fuel);
                return RedirectToAction(nameof(Index));
                }
            else
                {
                if (fuel.VehicleId <= 0)
                    ModelState.AddModelError(nameof(fuel.VehicleId), "Please select a vehicle.");
                if (fuel.FuelQuantity <= 0)
                    ModelState.AddModelError(nameof(fuel.FuelQuantity), "Fuel quantity must be greater than 0.");
                if (fuel.Date > DateTime.Now)
                    ModelState.AddModelError(nameof(fuel.Date), "Date cannot be in the future.");

                ViewBag.Vehicles = new SelectList(await _fuelRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", fuel.VehicleId);
                return View(fuel); // Re-render form with errors
                }
            }

        // GET: Fuel/Delete/{id}
        public async Task<IActionResult> Delete(int id)
            {
            var fuel = await _fuelRepository.GetByIdAsync(id);
            if (fuel == null)
                {
                return NotFound();
                }

            return View(fuel);
            }

        // POST: Fuel/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            await _fuelRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
            }
        public async Task<IActionResult> Details(int id)
            {
            var fuel = await _fuelRepository.GetByIdAsync(id);
            if (fuel == null)
                {
                return NotFound(); // Handle missing record
                }

            return View(fuel);
            }
        public async Task<IActionResult> TotalCost()
            {
            var totalCost = await _fuelRepository.CalculateTotalCostAsync();
            ViewBag.TotalCost = totalCost;
            return View();
            }


        public async Task<IActionResult> ExpensesChart()
            {
            var monthlyExpenses = await _fuelRepository.GetMonthlyExpensesAsync();
            ViewBag.MonthlyExpenses = monthlyExpenses;
            return View();
            }


        }
    }
    
