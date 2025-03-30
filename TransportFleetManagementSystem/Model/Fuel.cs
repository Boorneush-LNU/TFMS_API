using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransportFleetManagementSystem.Model
    {
    public class Fuel
        {
        [Key]
        public int FuelId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public DateTime Date { get; set; }
        public decimal FuelQuantity { get; set; }
        public decimal Cost { get; set; }

        // FuelPrice is now optional
        public decimal? FuelPrice { get; set; }

        }
    }
