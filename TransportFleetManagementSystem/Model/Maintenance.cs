using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransportFleetManagementSystem.Model
    {
    public class Maintenance
        {
        [Key]
        public int MaintenanceId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        [StringLength(255)]
        public string? Description { get; set; }

        public DateTime ScheduledDate { get; set; }
        public string? Status { get; set; }
        }
    }
