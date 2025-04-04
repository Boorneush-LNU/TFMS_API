using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class VehicleController : Controller
        {
        private readonly HttpClient _httpClient;

        public VehicleController(HttpClient httpClient)
            {
            _httpClient = httpClient;
            }

        // GET: Vehicle/Index
        public async Task<IActionResult> Index(int? page, string searchString)
            {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var response = await _httpClient.GetAsync("https://localhost:7072/api/VehicleApi");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var allVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(responseData);
            var vehicles = allVehicles.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                {
                vehicles = vehicles.Where(v => v.RegistrationNumber.Contains(searchString));
                }

            int totalItems = vehicles.Count();

            var pagedVehicles = vehicles
                .OrderByDescending(v => v.VehicleId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalItems = totalItems;
            ViewBag.SearchString = searchString;

            ViewBag.TotalVehicles = allVehicles.Count();
            ViewBag.LiveVehicles = allVehicles.Count(v => v.Status == "Active");
            ViewBag.Maintenance = allVehicles.Count(v => v.Status == "Maintenance");
            ViewBag.Inactive = allVehicles.Count(v => v.Status == "Inactive");

            ViewBag.Cars = allVehicles.Count(v => v.Capacity >= 3 && v.Capacity <= 5);
            ViewBag.Minivans = allVehicles.Count(v => v.Capacity > 5 && v.Capacity <= 15);
            ViewBag.Buses = allVehicles.Count(v => v.Capacity > 15 && v.Capacity <= 50);

            var currentDate = DateTime.Now;
            var dueDate = currentDate.AddMonths(-4);
            var overdueDate = dueDate.AddDays(-14);

            ViewBag.ServiceDue = allVehicles.Count(v => v.LastServicedDate <= dueDate && v.LastServicedDate > overdueDate);
            ViewBag.ServiceOverdue = allVehicles.Count(v => v.LastServicedDate <= overdueDate);

            return View(pagedVehicles);
            }

        // GET: Vehicle/Details/5
        public async Task<IActionResult> Details(int? id)
            {
            if (id == null)
                {
                return NotFound();
                }

            var response = await _httpClient.GetAsync($"https://localhost:7072/api/VehicleApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                return NotFound();
                }

            var responseData = await response.Content.ReadAsStringAsync();
            var vehicle = JsonConvert.DeserializeObject<Vehicle>(responseData);

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

            var jsonContent = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7072/api/VehicleApi", content);
            if (!response.IsSuccessStatusCode)
                {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the vehicle.");
                return View(vehicle);
                }

            return RedirectToAction(nameof(Index));
            }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
            if (id == null)
                {
                return NotFound();
                }

            var response = await _httpClient.GetAsync($"https://localhost:7072/api/VehicleApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                return NotFound();
                }

            var responseData = await response.Content.ReadAsStringAsync();
            var vehicle = JsonConvert.DeserializeObject<Vehicle>(responseData);

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

            var jsonContent = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7072/api/VehicleApi/{id}", content);
            if (!response.IsSuccessStatusCode)
                {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the vehicle.");
                return View(vehicle);
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

            var response = await _httpClient.GetAsync($"https://localhost:7072/api/VehicleApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                return NotFound();
                }

            var responseData = await response.Content.ReadAsStringAsync();
            var vehicle = JsonConvert.DeserializeObject<Vehicle>(responseData);

            return View(vehicle);
            }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            var response = await _httpClient.DeleteAsync($"https://localhost:7072/api/VehicleApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the vehicle.");
                return RedirectToAction(nameof(Delete), new { id });
                }

            return RedirectToAction(nameof(Index));
            }
        }
    }