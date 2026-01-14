using System;
using System.Collections.Generic;
using System.Text;
using Shkolo.Data.DTOs;
using Shkolo.Data.Services;

namespace Shkolo.Data.Services
{
    public class UserService
    {
        private readonly ShkoloContext _context;

        public UserService()
        {
            _context = new ShkoloContext();
        }

        public UserDto? Authenticate(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Username = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}
