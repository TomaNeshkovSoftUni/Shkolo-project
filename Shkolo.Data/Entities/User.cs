using System.ComponentModel.DataAnnotations;
using Shkolo.Data.Entities.Enums;

namespace Shkolo.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        // Requirement: Username must be unique
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public Role Role { get; set; }

        // Requirement: Status (Active/Blocked)
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
}