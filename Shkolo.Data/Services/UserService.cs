using System;
using System.Collections.Generic;
using System.Linq;
using Shkolo.Data.DTOs;
using Shkolo.Data.Entities;
using Shkolo.Data.Entities.Enums;
using Shkolo.Data.ShkoloDbContext;

namespace Shkolo.Data.Services
{
    // UserService handles User accounts and Admin tasks
    public class UserService
    {
        private readonly ShkoloContext _context;

        public UserService(ShkoloContext context)
        {
            _context = context;
        }

        // Login logic: Validates credentials and checks if user is Blocked
        public UserDto? Authenticate(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null || user.Status == UserStatus.Blocked)
                return null;

            // Returns DTO to hide sensitive data (like Password) from the UI
            return new UserDto
            {
                Username = user.Username,
                Role = user.Role,
                Status = user.Status
            };
        }

        // Registration: Checks for unique username and sets default role
        public string Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
                return "Error: Username taken.";

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = Role.RegisteredUser,
                Status = UserStatus.Active
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return "Account created!";
        }

        // Admin Action: List all system users
        public List<UserDto> GetAllUsers()
        {
            return _context.Users
                .Select(u => new UserDto
                {
                    Username = u.Username,
                    Role = u.Role,
                    Status = u.Status
                })
                .ToList();
        }

        // Admin Action: Block a user (Safety check: Admins cannot block other Admins)
        public bool ChangeUserStatus(string username, UserStatus newStatus)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role == Role.Administrator)
                return false;

            user.Status = newStatus;
            _context.SaveChanges();
            return true;
        }

        // Admin Action: Delete a user
        public bool DeleteUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role == Role.Administrator)
                return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}