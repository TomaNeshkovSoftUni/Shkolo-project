using System;
using System.Collections.Generic;
using System.Linq;
using Shkolo.Data.DTOs;
using Shkolo.Data.Entities;
using Shkolo.Data.Entities.Enums;

namespace Shkolo.Data.Services
{
    public class UserService
    {
        private readonly ShkoloContext _context;

        public UserService()
        {
            _context = new ShkoloContext();
        }

        // --- 1. LOGIN ---
        public UserDto? Authenticate(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            // Access Check: Invalid credentials OR Blocked status prevents login
            if (user == null || user.Status == UserStatus.Blocked)
                return null;

            return new UserDto
            {
                Username = user.Username,
                Role = user.Role
            };
        }

        // --- 2. REGISTER (Matches Requirement: RegisteredUser) ---
        public string Register(string username, string password)
        {
            // Unique Username Check (Requirement: Username must be unique)
            if (_context.Users.Any(u => u.Username == username))
                return "Error: Username already exists!";

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = Role.RegisteredUser, // All app-created accounts are RegisteredUsers
                Status = UserStatus.Active
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return "Success: Account created! You can now login.";
        }

        // --- 3. ADMINISTRATOR ACTIONS ---

        // List all users for the Admin menu
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

        // Change Status (Requirement: Admin blocks users)
        public bool ChangeUserStatus(string username, UserStatus newStatus)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // Protection: Cannot block yourself/other admins
            if (user == null || user.Role == Role.Administrator)
                return false;

            user.Status = newStatus;
            _context.SaveChanges();
            return true;
        }

        // Delete (Requirement: Admin deletes users)
        public bool DeleteUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // Protection: Cannot delete the admin account via this menu
            if (user == null || user.Role == Role.Administrator)
                return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}