using Microsoft.AspNetCore.Mvc;
using TransportFleetManagementSystem.Models;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class DriverController : Controller
        {
        private readonly IDriverRepository _driverRepository;

        public DriverController(IDriverRepository driverRepository)
            {
            _driverRepository = driverRepository;
            }

        public async Task<IActionResult> Index()
            {
            // Fetch all drivers
            var drivers = await _driverRepository.GetAllDriversAsync();
            return View(drivers);
            }

        public async Task<IActionResult> Details(int id)
            {
            // Get driver details
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
                {
                return NotFound();
                }
            return View(driver);
            }

        public IActionResult Create()
            {
            // Show the create form
            return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
            {
            if (!string.IsNullOrEmpty(driver.Name) && !string.IsNullOrEmpty(driver.Phone) && driver.Phone.Length == 10 && !string.IsNullOrEmpty(driver.Address) && !string.IsNullOrEmpty(driver.LicenseNumber) && driver.LicenseNumber.Length <= 16)
                {

                // Add the driver using the repository
                await _driverRepository.AddDriverAsync(driver);
                return RedirectToAction(nameof(Index));
                }
            return View(driver);
            }

        public async Task<IActionResult> Edit(int id)
            {
            // Fetch the driver for editing
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
                {
                return NotFound();
                }
            return View(driver);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Driver driver)
            {
            if (id != driver.DriverId)
                {
                return BadRequest();
                }
            if (!string.IsNullOrEmpty(driver.Name) && !string.IsNullOrEmpty(driver.Phone) && driver.Phone.Length == 10 && !string.IsNullOrEmpty(driver.Address) && !string.IsNullOrEmpty(driver.LicenseNumber) && driver.LicenseNumber.Length <= 16)
                {

                // Update the driver using the repository
                await _driverRepository.UpdateDriverAsync(driver);
                return RedirectToAction(nameof(Index));
                }
            return View(driver);
            }

        public async Task<IActionResult> Delete(int id)
            {
            // Fetch the driver for deletion
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
                {
                return NotFound();
                }
            return View(driver);
            }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            // Delete the driver using the repository
            await _driverRepository.DeleteDriverAsync(id);
            return RedirectToAction(nameof(Index));
            }
        }

    }
