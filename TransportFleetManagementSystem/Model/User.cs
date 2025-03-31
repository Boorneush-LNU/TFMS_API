using System.ComponentModel.DataAnnotations;

namespace TransportFleetManagementSystem.Model
    {
    public class User
        {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        }
    }
