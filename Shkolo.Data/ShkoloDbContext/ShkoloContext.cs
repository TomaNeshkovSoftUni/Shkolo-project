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
                optionsBuilder.UseSqlServer("Server=.;Database=ShkoloDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This automatically applies all configurations in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShkoloContext).Assembly);

            // Note: If your Config folder is in a different assembly, 
            // you can use: modelBuilder.ApplyConfiguration(new UserConfig()); for each one.
        }
    }
}