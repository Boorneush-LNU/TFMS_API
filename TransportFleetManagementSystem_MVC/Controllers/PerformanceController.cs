using System.Reflection.Metadata;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Data;
using TransportFleetManagementSystem.Model;
using Document = iTextSharp.text.Document;
namespace TransportFleetManagementSystem_MVC.Controllers
    {
    public class PerformanceController : Controller

            {

            private readonly AppDbContext _context;

            private readonly ILogger<PerformanceController> _logger;

            public PerformanceController(AppDbContext context, ILogger<PerformanceController> logger)

                {

                _context = context;

                _logger = logger;

                }

            public async Task<IActionResult> Index()

                {

                var vehicles = await _context.Vehicles.ToListAsync();

                ViewBag.Vehicles = vehicles;

                ViewBag.EfficiencyData = null; // Initialize as null or with default data

                return View();

                }

            public async Task<IActionResult> GetVehicleEfficiency(int vehicleId)

                {

                // Calculate total distance traveled

                var totalDistance = await _context.Trips

                    .Where(t => t.VehicleId == vehicleId)

                    .SumAsync(t => (decimal)EF.Functions.DateDiffMinute(t.StartTime, t.EndTime) / 60); // Convert to decimal

                // Calculate total fuel consumed

                var totalFuelConsumed = await _context.Fuels

                    .Where(f => f.VehicleId == vehicleId)

                    .SumAsync(f => f.FuelQuantity);

                // Estimate total maintenance cost based on predefined values

                var maintenanceRecords = await _context.Maintenances

                    .Where(m => m.VehicleId == vehicleId)

                    .ToListAsync();

                decimal totalMaintenanceCost = 0;

                foreach (var maintenance in maintenanceRecords)

                    {

                    // Example: Assigning a cost based on maintenance description

                    if (maintenance.Description.Contains("Oil Change"))

                        {

                        totalMaintenanceCost += 50; // Example cost for oil change

                        }

                    else if (maintenance.Description.Contains("Tire Replacement"))

                        {

                        totalMaintenanceCost += 100; // Example cost for tire replacement

                        }

                    // Add more conditions as needed

                    }

                // Calculate fuel efficiency

                decimal fuelEfficiency = 0;

                if (totalFuelConsumed > 0)

                    {

                    fuelEfficiency = totalDistance / totalFuelConsumed;

                    }

                // Calculate maintenance cost per kilometer

                decimal maintenanceCostPerKm = 0;

                if (totalDistance > 0)

                    {

                    maintenanceCostPerKm = totalMaintenanceCost / totalDistance;

                    }

                var efficiencyData = new

                    {

                    FuelEfficiency = fuelEfficiency,

                    MaintenanceCostPerKm = maintenanceCostPerKm

                    };

                return Json(efficiencyData);

                }

            [HttpPost]

            public async Task<IActionResult> DownloadConsolidatedReport(string module, string format)

                {

                dynamic? report = null;

                if (module == "Vehicles")

                    {

                    var vehicles = await _context.Vehicles.ToListAsync();

                    report = new { Vehicles = vehicles ?? new List<Vehicle>() };

                    }

                else if (module == "Trips")

                    {

                    var trips = await _context.Trips.ToListAsync();

                    report = new { Trips = trips ?? new List<Trip>() };

                    }

                else if (module == "Fuels")

                    {

                    var fuels = await _context.Fuels.ToListAsync();

                    report = new { Fuels = fuels ?? new List<Fuel>() };

                    }

                else if (module == "Maintenances")

                    {

                    var maintenances = await _context.Maintenances.ToListAsync();

                    report = new { Maintenances = maintenances ?? new List<Maintenance>() };

                    }

                else if (module == "Performances")

                    {

                    var performances = await _context.Performances.ToListAsync();

                    report = new { Performances = performances ?? new List<Performance>() };

                    }

                if (format == "pdf")

                    {

                    var pdfBytes = GeneratePdfReport(report, module);

                    return File(pdfBytes, "application/pdf", $"{module.ToLower()}_report.pdf");

                    }

                else if (format == "excel")

                    {

                    var excelBytes = GenerateExcelReport(report, module);

                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{module.ToLower()}_report.xlsx");

                    }

                else

                    {

                    return RedirectToAction(nameof(Index));

                    }

                }

            private byte[] GeneratePdfReport(dynamic report, string module)

                {

                using (var memoryStream = new MemoryStream())

                    {

                    var document = new Document();

                    PdfWriter.GetInstance(document, memoryStream);

                    document.Open();

                    document.Add(new Paragraph($"{module} Report"));

                    document.Add(new Paragraph(" ")); // Add a blank line

                    if (module == "Vehicles")

                        {

                        document.Add(new Paragraph("ID\tRegistration Number\tCapacity\tStatus\tLast Serviced Date"));

                        foreach (var vehicle in report.Vehicles)

                            {

                            document.Add(new Paragraph($"{vehicle.VehicleId}\t{vehicle.RegistrationNumber}\t{vehicle.Capacity}\t{vehicle.Status}\t{vehicle.LastServicedDate}"));

                            }

                        }

                    else if (module == "Trips")

                        {

                        document.Add(new Paragraph("ID\tVehicle ID\tDriver ID\tStart Location\tEnd Location\tStart Time\tEnd Time"));

                        foreach (var trip in report.Trips)

                            {

                            document.Add(new Paragraph($"{trip.TripId}\t{trip.VehicleId}\t{trip.DriverId}\t{trip.StartLocation}\t{trip.EndLocation}\t{trip.StartTime}\t{trip.EndTime}"));

                            }

                        }

                    else if (module == "Fuels")

                        {

                        document.Add(new Paragraph("ID\tVehicle ID\tDate\tFuel Quantity\tCost"));

                        foreach (var fuel in report.Fuels)

                            {

                            document.Add(new Paragraph($"{fuel.FuelId}\t{fuel.VehicleId}\t{fuel.Date}\t{fuel.FuelQuantity}\t{fuel.Cost}"));

                            }

                        }

                    else if (module == "Maintenances")

                        {

                        document.Add(new Paragraph("ID\tVehicle ID\tDescription\tScheduled Date\tStatus"));

                        foreach (var maintenance in report.Maintenances)

                            {

                            document.Add(new Paragraph($"{maintenance.MaintenanceId}\t{maintenance.VehicleId}\t{maintenance.Description}\t{maintenance.ScheduledDate}\t{maintenance.Status.ScheduledDate}\t{maintenance.Status}"));

                            }

                        }

                    else if (module == "Performances")

                        {

                        document.Add(new Paragraph("ID\tReport Type\tData\tGenerated On"));

                        foreach (var performance in report.Performances)

                            {

                            document.Add(new Paragraph($"{performance.PerformanceId}\t{performance.ReportType}\t{performance.Data}\t{performance.GeneratedOn}"));

                            }

                        }

                    document.Close();

                    return memoryStream.ToArray();

                    }

                }

            private byte[] GenerateExcelReport(dynamic report, string module)

                {

                using (var workbook = new XLWorkbook())

                    {

                    var worksheet = workbook.Worksheets.Add($"{module} Report");

                    int row = 1;

                    if (module == "Vehicles")

                        {

                        worksheet.Cell(row, 1).Value = "ID";

                        worksheet.Cell(row, 2).Value = "Registration Number";

                        worksheet.Cell(row, 3).Value = "Capacity";

                        worksheet.Cell(row, 4).Value = "Status";

                        worksheet.Cell(row, 5).Value = "Last Serviced Date";

                        row++;

                        foreach (var vehicle in report.Vehicles)

                            {

                            worksheet.Cell(row, 1).Value = vehicle.VehicleId;

                            worksheet.Cell(row, 2).Value = vehicle.RegistrationNumber;

                            worksheet.Cell(row, 3).Value = vehicle.Capacity;

                            worksheet.Cell(row, 4).Value = vehicle.Status;

                            worksheet.Cell(row, 5).Value = vehicle.LastServicedDate;

                            row++;

                            }

                        }

                    else if (module == "Trips")

                        {

                        worksheet.Cell(row, 1).Value = "ID";

                        worksheet.Cell(row, 2).Value = "Vehicle ID";

                        worksheet.Cell(row, 3).Value = "Driver ID";

                        worksheet.Cell(row, 4).Value = "Start Location";

                        worksheet.Cell(row, 5).Value = "End Location";

                        worksheet.Cell(row, 6).Value = "Start Time";

                        worksheet.Cell(row, 7).Value = "End Time";

                        row++;

                        foreach (var trip in report.Trips)

                            {

                            worksheet.Cell(row, 1).Value = trip.TripId;

                            worksheet.Cell(row, 2).Value = trip.VehicleId;

                            worksheet.Cell(row, 3).Value = trip.DriverId;

                            worksheet.Cell(row, 4).Value = trip.StartLocation;

                            worksheet.Cell(row, 5).Value = trip.EndLocation;

                            worksheet.Cell(row, 6).Value = trip.StartTime;

                            worksheet.Cell(row, 7).Value = trip.EndTime;

                            row++;

                            }

                        }

                    else if (module == "Fuels")

                        {

                        worksheet.Cell(row, 1).Value = "ID";

                        worksheet.Cell(row, 2).Value = "Vehicle ID";

                        worksheet.Cell(row, 3).Value = "Date";

                        worksheet.Cell(row, 4).Value = "Fuel Quantity";

                        worksheet.Cell(row, 5).Value = "Cost";

                        row++;

                        foreach (var fuel in report.Fuels)

                            {

                            worksheet.Cell(row, 1).Value = fuel.FuelId;

                            worksheet.Cell(row, 2).Value = fuel.VehicleId;

                            worksheet.Cell(row, 3).Value = fuel.Date;

                            worksheet.Cell(row, 4).Value = fuel.FuelQuantity;

                            worksheet.Cell(row, 5).Value = fuel.Cost;

                            row++;

                            }

                        }

                    else if (module == "Maintenances")

                        {

                        worksheet.Cell(row, 1).Value = "ID";

                        worksheet.Cell(row, 2).Value = "Vehicle ID";

                        worksheet.Cell(row, 3).Value = "Description";

                        worksheet.Cell(row, 4).Value = "Scheduled Date";

                        worksheet.Cell(row, 5).Value = "Status";

                        row++;

                        foreach (var maintenance in report.Maintenances)

                            {

                            worksheet.Cell(row, 1).Value = maintenance.MaintenanceId;

                            worksheet.Cell(row, 2).Value = maintenance.VehicleId;

                            worksheet.Cell(row, 3).Value = maintenance.Description;

                            worksheet.Cell(row, 4).Value = maintenance.ScheduledDate;

                            worksheet.Cell(row, 5).Value = maintenance.Status;

                            row++;

                            }

                        }

                    else if (module == "Performances")

                        {

                        worksheet.Cell(row, 1).Value = "ID";

                        worksheet.Cell(row, 2).Value = "Report Type";

                        worksheet.Cell(row, 3).Value = "Data";

                        worksheet.Cell(row, 4).Value = "Generated On";

                        row++;

                        foreach (var performance in report.Performances)

                            {

                            worksheet.Cell(row, 1).Value = performance.PerformanceId;

                            worksheet.Cell(row, 2).Value = performance.ReportType;

                            worksheet.Cell(row, 3).Value = performance.Data;

                            worksheet.Cell(row, 4).Value = performance.GeneratedOn;

                            row++;

                            }

                        }

                    using (var stream = new MemoryStream())

                        {

                        workbook.SaveAs(stream);

                        return stream.ToArray();

                        }

                    }

                }

            }

        }

    
