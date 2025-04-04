using Microsoft.AspNetCore.Mvc;
using TransportFleetManagementSystem.DTOs;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;

namespace TransportFleetManagementSystem_API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class DriverApiController : ControllerBase
        {
        private readonly IDriverRepository _driverRepository;

        public DriverApiController(IDriverRepository driverRepository)
            {
            _driverRepository = driverRepository;
            }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
            {
            var drivers = await _driverRepository.GetAllDriversAsync();
            var driverDtos = drivers.Select(d => new DriverDto
                {
                DriverId = d.DriverId,
                Name = d.Name,
                Phone = d.Phone,
                Address = d.Address,
                LicenseNumber = d.LicenseNumber
                });
            return Ok(driverDtos);
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
            {
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
                {
                return NotFound();
                }
            var driverDto = new DriverDto
                {
                DriverId = driver.DriverId,
                Name = driver.Name,
                Phone = driver.Phone,
                Address = driver.Address,
                LicenseNumber = driver.LicenseNumber
                };
            return Ok(driverDto);
            }

        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] DriverDto driverDto)
            {
            var driver = new Driver
                {
                DriverId = driverDto.DriverId,
                Name = driverDto.Name,
                Phone = driverDto.Phone,
                Address = driverDto.Address,
                LicenseNumber = driverDto.LicenseNumber
                };
            await _driverRepository.AddDriverAsync(driver);
            return CreatedAtAction(nameof(GetDriverById), new { id = driver.DriverId }, driverDto);
            }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverDto driverDto)
            {
            if (id != driverDto.DriverId)
                {
                return BadRequest();
                }
            var driver = new Driver
                {
                DriverId = driverDto.DriverId,
                Name = driverDto.Name,
                Phone = driverDto.Phone,
                Address = driverDto.Address,
                LicenseNumber = driverDto.LicenseNumber
                };
            await _driverRepository.UpdateDriverAsync(driver);
            return NoContent();
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
            {
            await _driverRepository.DeleteDriverAsync(id);
            return NoContent();
            }
        }
    }