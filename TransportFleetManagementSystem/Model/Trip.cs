using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransportFleetManagementSystem.Model
    {
    public class Trip
        {
        [Key]
        [Required(ErrorMessage = "Trip ID is required.")]
        public int TripId { get; set; }

        [ForeignKey("Vehicle")]
        [Required(ErrorMessage = "Vehicle ID is required.")]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [ForeignKey("Driver")]
        [Required(ErrorMessage = "Driver ID is required.")]
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }


        [StringLength(100)]
        [Required(ErrorMessage = "Start Location is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Start location must be a string.")]
        public string? StartLocation { get; set; }

        [Required(ErrorMessage = "End Location is required.")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "End location must be a string.")]
        public string? EndLocation { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date and time format.")]

        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date and time format.")]
        public DateTime EndTime { get; set; }
        }
    }
