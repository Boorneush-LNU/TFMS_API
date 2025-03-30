using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransportFleetManagementSystem.Models;

namespace TransportFleetManagementSystem.Model
    {
    public class Trip
        {
        [Key]
        public int TripId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        [Required]
        [StringLength(100)]
        public string? StartLocation { get; set; }

        [Required]
        [StringLength(100)]
        public string? EndLocation { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        }
    }
