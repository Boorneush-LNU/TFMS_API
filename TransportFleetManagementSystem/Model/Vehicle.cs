using System.ComponentModel.DataAnnotations;

namespace TransportFleetManagementSystem.Model
    {
    public class Vehicle
        {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        [Display(Name = "Registartion Number")]
        [StringLength(10, ErrorMessage = "Registration number cannot be longer than 10 characters.")]
        public string? RegistrationNumber { get; set; }

        [Range(1, 60, ErrorMessage = "Capacity must be between 1 and 60.")]
        public int Capacity { get; set; }

        [Required]
        [RegularExpression("Active|Inactive|Maintenance", ErrorMessage = "Status must be either 'Active', 'Inactive', or 'Maintenance'.")]
        public string? Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Last Serviced Date")]
        public DateTime LastServicedDate { get; set; }

        public ICollection<Fuel> Fuels { get; set; } = new List<Fuel>();
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
        }
    }
