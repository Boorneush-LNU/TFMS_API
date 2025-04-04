using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransportFleetManagementSystem.Model
    {
    public class Fuel
        {
        [Key]
        public int FuelId { get; set; }

        [Required(ErrorMessage = "Vehicle ID is required.")]
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        [Required(ErrorMessage = "Fuel quantity must be greater than 0.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Fuel quantity must be greater than 0.")]
        public decimal FuelQuantity { get; set; }


        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Fuel price must be greater than 0.")]
        public decimal? FuelPrice { get; set; }

        public decimal Cost { get; set; }
        }
    }
