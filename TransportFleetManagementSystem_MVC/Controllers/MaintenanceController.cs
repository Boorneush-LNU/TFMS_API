using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class MaintenanceController : Controller
        {
        private readonly IMaintenanceRepository _maintenanceRepository;

        public MaintenanceController(IMaintenanceRepository maintenanceRepository)
            {
            _maintenanceRepository = maintenanceRepository;
            }

        public async Task<IActionResult> Index()
            {
            var maintenances = await _maintenanceRepository.GetAllAsync();

            // Calculate counts
            var goodConditionCount = maintenances.Count(m => m.Status == "Good Condition");
            var needMaintenanceCount = maintenances.Count(m => m.Status == "Need Maintenance");
            var underMaintenanceCount = maintenances.Count(m => m.Status == "Under Maintenance");

            // Pass counts to the view
            ViewBag.GoodConditionCount = goodConditionCount;
            ViewBag.NeedMaintenanceCount = needMaintenanceCount;
            ViewBag.UnderMaintenanceCount = underMaintenanceCount;

            return View(maintenances);
            }

        public async Task<IActionResult> Details(int id)
            {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id);
            if (maintenance == null)
                {
                return NotFound();
                }
            return View(maintenance);
            }
        public async Task<IActionResult> Create()
            {
            ViewBag.Vehicles = new SelectList(await _maintenanceRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber");
            return View();
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Maintenance maintenance)
            {
            // Check if a maintenance record already exists for the given vehicle ID
            var maintenanceExists = await _maintenanceRepository.MaintenanceExistsAsync(maintenance.VehicleId);
            if (maintenanceExists)
                {
                // Add an error message to the model state
                ModelState.AddModelError("", "A maintenance record for this vehicle already exists.");
                ViewBag.Vehicles = new SelectList(await _maintenanceRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", maintenance.VehicleId);
                return View(maintenance);
                }

            // Add the current maintenance record
            await _maintenanceRepository.AddAsync(maintenance);

            // Automatically set the next maintenance date to three months later
            var nextMaintenanceDate = DateTime.Today.AddMonths(3);
            maintenance.NextMaintenanceDate = nextMaintenanceDate;

            // Save the updated maintenance record
            await _maintenanceRepository.UpdateAsync(maintenance);

            return RedirectToAction(nameof(Index));
            ViewBag.Vehicles = new SelectList(await _maintenanceRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", maintenance.VehicleId);
            return View(maintenance);
            }
        // GET: Maintenance/Edit/{id}
        public async Task<IActionResult> Edit(int id)
            {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id);
            if (maintenance == null)
                {
                return NotFound();
                }

            ViewBag.Vehicles = new SelectList(await _maintenanceRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", maintenance.VehicleId);
            return View(maintenance);
            }

        // POST: Maintenance/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Maintenance maintenance)
            {
            if (maintenance.VehicleId > 0 && !string.IsNullOrEmpty(maintenance.Description) && maintenance.ScheduledDate <= DateTime.Now)
                {
                await _maintenanceRepository.UpdateAsync(maintenance);
                return RedirectToAction(nameof(Index));
                }
            else
                {
                if (maintenance.VehicleId <= 0)
                    ModelState.AddModelError(nameof(maintenance.VehicleId), "Please select a vehicle.");
                if (string.IsNullOrEmpty(maintenance.Description))
                    ModelState.AddModelError(nameof(maintenance.Description), "Description is required.");
                if (maintenance.ScheduledDate > DateTime.Now)
                    ModelState.AddModelError(nameof(maintenance.ScheduledDate), "Scheduled date cannot be in the future.");

                ViewBag.Vehicles = new SelectList(await _maintenanceRepository.GetAllVehiclesAsync(), "VehicleId", "RegistrationNumber", maintenance.VehicleId);
                return View(maintenance); // Re-render form with errors
                }
            }
        // GET: Maintenance/Delete/{id}
        public async Task<IActionResult> Delete(int id)
            {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id);
            if (maintenance == null)
                {
                return NotFound();
                }
            return View(maintenance);
            }

        // POST: Maintenance/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            await _maintenanceRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
            }
        }
    }
