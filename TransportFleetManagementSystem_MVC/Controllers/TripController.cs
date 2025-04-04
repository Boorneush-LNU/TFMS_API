using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class TripController : Controller
        {
        private readonly ITripRepository _tripRepository;

        public TripController(ITripRepository tripRepository)
            {
            _tripRepository = tripRepository;
            }

        // GET: Trip
        public async Task<IActionResult> Index(int page = 1, string searchString = "")
            {
            int pageSize = 10; // Number of items per page

            // Fetch paginated trip records from repository
            var trips = await _tripRepository.GetPagedTripsAsync(page, pageSize, searchString);
            int totalItems = await _tripRepository.GetTotalTripCountAsync(searchString);

            // Pass pagination data to the view
            ViewBag.TotalTripsScheduled = totalItems;
            ViewBag.ActiveTrips = trips.Count(t => t.StartTime <= DateTime.Now && t.EndTime >= DateTime.Now);
            ViewBag.TripsInProgress = trips.Count(t => t.StartTime <= DateTime.Now && t.EndTime >= DateTime.Now);
            ViewBag.TripsScheduledToday = trips.Count(t => t.StartTime.Date == DateTime.Now.Date);
            ViewBag.TripsScheduledTomorrow = trips.Count(t => t.StartTime.Date == DateTime.Now.AddDays(1).Date);
            ViewBag.TripsScheduledThisWeek = trips.Count(t => t.StartTime.Date >= DateTime.Now.Date && t.StartTime.Date <= DateTime.Now.AddDays(7).Date);
            ViewBag.CurrentPage = page;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchString = searchString;

            return View(trips);
            }
        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int? id)
            {
            if (id == null)
                {
                return NotFound();
                }

            var trip = await _tripRepository.GetByIdAsync(id.Value);
            if (trip == null)
                {
                return NotFound();
                }

            return View(trip);
            }

        // GET: Trip/Create
        public async Task<IActionResult> Create()
            {
            ViewData["DriverId"] = new SelectList(await _tripRepository.GetDriversAsync(), "DriverId", "Name");
            ViewData["VehicleId"] = new SelectList(await _tripRepository.GetVehiclesAsync(), "VehicleId", "RegistrationNumber");
            return View();
            }

        // POST: Trip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,VehicleId,DriverId,StartLocation,EndLocation,StartTime,EndTime")] Trip trip)
            {
            if (ModelState.IsValid)
                {
                await _tripRepository.AddAsync(trip);
                return RedirectToAction(nameof(Index));
                }
            ViewData["DriverId"] = new SelectList(await _tripRepository.GetDriversAsync(), "DriverId", "Name", trip.DriverId);
            ViewData["VehicleId"] = new SelectList(await _tripRepository.GetVehiclesAsync(), "VehicleId", "RegistrationNumber", trip.VehicleId);
            return View(trip);
            }

        // GET: Trip/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
            if (id == null)
                {
                return NotFound();
                }

            var trip = await _tripRepository.GetByIdAsync(id.Value);
            if (trip == null)
                {
                return NotFound();
                }
            ViewData["DriverId"] = new SelectList(await _tripRepository.GetDriversAsync(), "DriverId", "Name", trip.DriverId);
            ViewData["VehicleId"] = new SelectList(await _tripRepository.GetVehiclesAsync(), "VehicleId", "RegistrationNumber", trip.VehicleId);
            return View(trip);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripId,VehicleId,DriverId,StartLocation,EndLocation,StartTime,EndTime")] Trip trip)
            {
            if (id != trip.TripId)
                {
                return NotFound();
                }

            if (ModelState.IsValid)
                {
                try
                    {
                    await _tripRepository.UpdateAsync(trip);
                    }
                catch (DbUpdateConcurrencyException)
                    {
                    if (await _tripRepository.GetByIdAsync(trip.TripId) == null)
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
            ViewData["DriverId"] = new SelectList(await _tripRepository.GetDriversAsync(), "DriverId", "Name", trip.DriverId);
            ViewData["VehicleId"] = new SelectList(await _tripRepository.GetVehiclesAsync(), "VehicleId", "RegistrationNumber", trip.VehicleId);
            return View(trip);
            }

        // GET: Trip/Delete/5
        public async Task<IActionResult> Delete(int? id)
            {
            if (id == null)
                {
                return NotFound();
                }

            var trip = await _tripRepository.GetByIdAsync(id.Value);
            if (trip == null)
                {
                return NotFound();
                }

            return View(trip);
            }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            await _tripRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
            }
        }
    }
