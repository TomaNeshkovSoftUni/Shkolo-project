using Shkolo.Data.Entities.Enums;

namespace Shkolo.Data.DTOs
{
    public class UserDto
    {
        public string Username { get; set; } = null!;
        public Role Role { get; set; }
        public UserStatus Status { get; set; }

        //Added UserStatus
    }
}