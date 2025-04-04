using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportFleetManagementSystem.DTOs
    {
    public class DriverDto
        {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }
        }
    }
