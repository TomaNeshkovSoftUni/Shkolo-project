using System;
using Microsoft.EntityFrameworkCore;
using Shkolo.Data.Entities;
using Shkolo.Data.Entities.Enums;

namespace Shkolo.Data
{
    public class ShkoloContext : DbContext
    {
        public ShkoloContext() { }

        public ShkoloContext(DbContextOptions<ShkoloContext> options)
            : base(options) { }

        // --- Database Tables ---
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<SchoolClass> SchoolClasses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Default connection string for local SQL Server // Change if using docker/other setup
                optionsBuilder.UseSqlServer("Server=.;Database=ShkoloDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically loads all IEntityTypeConfiguration classes (in Config folder)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShkoloContext).Assembly);
        }
    }
}