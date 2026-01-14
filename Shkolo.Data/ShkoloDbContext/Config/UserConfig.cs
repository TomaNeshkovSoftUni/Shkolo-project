using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shkolo.Data.Entities;
using Shkolo.Data.Entities.Enums;


namespace Shkolo.Data.ShkoloDbContext.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Unique Username
            builder.HasIndex(u => u.Username).IsUnique();

            // Seed Admin
            builder.HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "password123",
                Role = Role.Teacher,
                Status = UserStatus.Active
            });
        }
    }
}
