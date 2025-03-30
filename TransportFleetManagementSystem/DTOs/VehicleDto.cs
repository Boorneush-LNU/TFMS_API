namespace TransportFleetManagementSystem.DTOs
    {
    public class VehicleDto
        {
        public int VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public DateTime LastServicedDate { get; set; }
        }
    }
