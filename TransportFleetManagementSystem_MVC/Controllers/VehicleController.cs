using Microsoft.AspNetCore.Mvc;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
        public class VehicleController : Controller
            {
            private readonly IVehicleRepository _vehicleRepository;

            public VehicleController(IVehicleRepository vehicleRepository)
                {
                _vehicleRepository = vehicleRepository;
                }

        // GET: Vehicle/Index        
        public async Task<IActionResult> Index()
                {
                var vehicles = await _vehicleRepository.GetAllAsync();
            ViewBag.TotalVehicles = vehicles.Count();
            ViewBag.LiveVehicles = vehicles.Count(v => v.Status == "Active");
            ViewBag.Maintenance = vehicles.Count(v => v.Status == "Maintenance");
            ViewBag.Inactive = vehicles.Count(v => v.Status == "Inactive");

            ViewBag.Cars = vehicles.Count(v => v.Capacity >= 3 && v.Capacity <= 5);
            ViewBag.Minivans = vehicles.Count(v => v.Capacity > 5 && v.Capacity <= 15);
            ViewBag.Buses = vehicles.Count(v => v.Capacity > 15 && v.Capacity <= 50);

            var currentDate = DateTime.Now;
            var dueDate = currentDate.AddMonths(-4);
            var overdueDate = dueDate.AddDays(-14);

            ViewBag.ServiceDue = vehicles.Count(v => v.LastServicedDate <= dueDate && v.LastServicedDate > overdueDate);
            ViewBag.ServiceOverdue = vehicles.Count(v => v.LastServicedDate <= overdueDate);
            var sortedVehicles = vehicles.OrderByDescending(v => v.VehicleId).ToList();
            return View(sortedVehicles);
                }

            // GET: Vehicle/Details/5
            public async Task<IActionResult> Details(int? id)
                {
                if (id == null)
                    {
                    return NotFound();
                    }

                var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);
                if (vehicle == null)
                    {
                    return NotFound();
                    }

                return View(vehicle);
                }

            // GET: Vehicle/Create
            public IActionResult Create()
                {
                return View();
                }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,RegistrationNumber,Capacity,Status,LastServicedDate")] Vehicle vehicle)
            {
            if (!ModelState.IsValid)
                {
                return View(vehicle);
                }

            // Check if a vehicle with the same RegistrationNumber already exists
            var existingVehicle = await _vehicleRepository.GetByRegistrationNumberAsync(vehicle.RegistrationNumber);
            if (existingVehicle != null)
                {
                ModelState.AddModelError("RegistrationNumber", "A vehicle with this registration number already exists.");
                return View(vehicle);
                }

            await _vehicleRepository.AddAsync(vehicle);
            return RedirectToAction(nameof(Index));
            }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(int? id)
                {
                if (id == null)
                    {
                    return NotFound();
                    }

                var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);
                if (vehicle == null)
                    {
                    return NotFound();
                    }
                return View(vehicle);
                }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,RegistrationNumber,Capacity,Status,LastServicedDate")] Vehicle vehicle)
            {
            if (id != vehicle.VehicleId)
                {
                return NotFound();
                }

            if (!ModelState.IsValid)
                {
                return View(vehicle);
                }

            try
                {
                await _vehicleRepository.UpdateAsync(vehicle);
                }
            catch (Exception)
                {
                if (await _vehicleRepository.GetByIdAsync(vehicle.VehicleId) == null)
                    {
                    return NotFound();
                    }
                else
                    {
                    throw;
                    }
                }
            return RedirectToAction(nameof(Index));
            }

        // GET: Vehicle/Delete/5
        public async Task<IActionResult> Delete(int? id)
                {
                if (id == null)
                    {
                    return NotFound();
                    }

                var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);
                if (vehicle == null)
                    {
                    return NotFound();
                    }

                return View(vehicle);
                }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
                {
                await _vehicleRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
                }
            }
        }
