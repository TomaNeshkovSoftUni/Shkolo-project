using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shkolo.Data.Entities;

namespace Shkolo.Data.ShkoloDbContext.Config
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // Simple many-to-many config

            builder.HasMany(s => s.Subjects)
                   .WithMany(sub => sub.Students);

        }
    }
}
