using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shkolo.Data.Entities;

namespace Shkolo.Data.ShkoloDbContext.Config
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasOne(g => g.Student)
                   .WithMany(s => s.Grades)
                   .HasForeignKey(g => g.StudentId);

            builder.HasOne(g => g.Teacher)
                   .WithMany(t => t.Grades)
                   .HasForeignKey(g => g.TeacherId);
        }
    }
}
