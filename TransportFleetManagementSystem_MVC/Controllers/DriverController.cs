using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class DriverController : Controller
        {
        private readonly HttpClient _httpClient;

        public DriverController(HttpClient httpClient)
            {
            _httpClient = httpClient;
            }

        public async Task<IActionResult> Index()
            {
            var response = await _httpClient.GetAsync("https://localhost:7072/api/DriverApi");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var drivers = JsonConvert.DeserializeObject<List<Driver>>(responseData);

            return View(drivers);
            }

        public async Task<IActionResult> Details(int id)
            {
            var response = await _httpClient.GetAsync($"https://localhost:7072/api/DriverApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                return NotFound();
                }

            var responseData = await response.Content.ReadAsStringAsync();
            var driver = JsonConvert.DeserializeObject<Driver>(responseData);

            return View(driver);
            }

        public IActionResult Create()
            {
            return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
            {
            if (!ModelState.IsValid)
                {
                return View(driver);
                }

            var jsonContent = JsonConvert.SerializeObject(driver);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7072/api/DriverApi", content);
            if (!response.IsSuccessStatusCode)
                {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the driver.");
                return View(driver);
                }

            return RedirectToAction(nameof(Index));
            }

        public async Task<IActionResult> Edit(int id)
            {
            var response = await _httpClient.GetAsync($"https://localhost:7072/api/DriverApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                return NotFound();
                }

            var responseData = await response.Content.ReadAsStringAsync();
            var driver = JsonConvert.DeserializeObject<Driver>(responseData);

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

            if (!ModelState.IsValid)
                {
                return View(driver);
                }

            var jsonContent = JsonConvert.SerializeObject(driver);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7072/api/DriverApi/{id}", content);
            if (!response.IsSuccessStatusCode)
                {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the driver.");
                return View(driver);
                }

            return RedirectToAction(nameof(Index));
            }

        public async Task<IActionResult> Delete(int id)
            {
            var response = await _httpClient.GetAsync($"https://localhost:7072/api/DriverApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                return NotFound();
                }

            var responseData = await response.Content.ReadAsStringAsync();
            var driver = JsonConvert.DeserializeObject<Driver>(responseData);

            return View(driver);
            }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            var response = await _httpClient.DeleteAsync($"https://localhost:7072/api/DriverApi/{id}");
            if (!response.IsSuccessStatusCode)
                {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the driver.");
                return RedirectToAction(nameof(Delete), new { id });
                }

            return RedirectToAction(nameof(Index));
            }
        }
    }