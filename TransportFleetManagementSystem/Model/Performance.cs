using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportFleetManagementSystem.Model
    {
    public class Performance
        {
        [Key]
        public int PerformanceId { get; set; }

        [Required]
        [StringLength(50)]
        public string? ReportType { get; set; }

        public string? Data { get; set; }
        public DateTime GeneratedOn { get; set; }
        }
    }
