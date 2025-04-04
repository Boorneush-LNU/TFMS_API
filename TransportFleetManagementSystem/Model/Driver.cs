using System.ComponentModel.DataAnnotations;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Model
    {
    public class Driver
        {
        [Key]
        public int DriverId { get; set; }

        [Required(ErrorMessage = "Driver name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must only contain alphabets.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "License number is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "License number must only contain numbers and alphabets.")]
        [StringLength(16, ErrorMessage = "License number cannot exceed 16 characters.")]
        public string LicenseNumber { get; set; }


        public ICollection<Trip> Trips { get; set; }
        }
    }