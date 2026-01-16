using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shkolo.Data.Entities;
using Shkolo.Data.Entities.Enums;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Requirement: Username is unique
        builder.HasIndex(u => u.Username).IsUnique();

        // Seed the mandatory Administrator
        builder.HasData(new User
        {
            Id = 1,
            Username = "admin",
            Password = "adminpassword",
            Role = Role.Administrator, // Updated role
            Status = UserStatus.Active
        });

        // Seed a Registered User (the one who does CRUD/JSON)
        builder.HasData(new User
        {
            Id = 2,
            Username = "user1",
            Password = "userpassword",
            Role = Role.RegisteredUser, // Updated role
            Status = UserStatus.Active
        });
    }
}