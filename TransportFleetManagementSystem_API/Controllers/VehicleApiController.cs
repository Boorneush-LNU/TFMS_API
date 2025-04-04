using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportFleetManagementSystem.DTOs;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem_API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleApiController : ControllerBase
        {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleApiController(IVehicleRepository vehicleRepository)
            {
            _vehicleRepository = vehicleRepository;
            }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
            {
            var vehicles = await _vehicleRepository.GetAllAsync();
            var vehicleDtos = vehicles.Select(v => new VehicleDto
                {
                VehicleId = v.VehicleId,
                RegistrationNumber = v.RegistrationNumber,
                Capacity = v.Capacity,
                Status = v.Status,
                LastServicedDate = v.LastServicedDate
                });
            return Ok(vehicleDtos);
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
            {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                {
                return NotFound();
                }
            var vehicleDto = new VehicleDto
                {
                VehicleId = vehicle.VehicleId,
                RegistrationNumber = vehicle.RegistrationNumber,
                Capacity = vehicle.Capacity,
                Status = vehicle.Status,
                LastServicedDate = vehicle.LastServicedDate
                };
            return Ok(vehicleDto);
            }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto)
            {
            var vehicle = new Vehicle
                {
                VehicleId = vehicleDto.VehicleId,
                RegistrationNumber = vehicleDto.RegistrationNumber,
                Capacity = vehicleDto.Capacity,
                Status = vehicleDto.Status,
                LastServicedDate = vehicleDto.LastServicedDate
                };
            await _vehicleRepository.AddAsync(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.VehicleId }, vehicleDto);
            }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleDto vehicleDto)
            {
            if (id != vehicleDto.VehicleId)
                {
                return BadRequest();
                }
            var vehicle = new Vehicle
                {
                VehicleId = vehicleDto.VehicleId,
                RegistrationNumber = vehicleDto.RegistrationNumber,
                Capacity = vehicleDto.Capacity,
                Status = vehicleDto.Status,
                LastServicedDate = vehicleDto.LastServicedDate
                };
            await _vehicleRepository.UpdateAsync(vehicle);
            return NoContent();
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
            {
            await _vehicleRepository.DeleteAsync(id);
            return NoContent();
            }
        }
    }
