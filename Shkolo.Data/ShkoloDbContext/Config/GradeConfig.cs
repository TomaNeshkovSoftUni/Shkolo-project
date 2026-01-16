using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shkolo.Data.Entities;

namespace Shkolo.Data.ShkoloDbContext.Config
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            // Link Grade to Student (One-to-Many)
            builder.HasOne(g => g.Student)
                   .WithMany(s => s.Grades)
                   .HasForeignKey(g => g.StudentId);

            // Link Grade to Teacher (One-to-Many)
            builder.HasOne(g => g.Teacher)
                   .WithMany(t => t.Grades)
                   .HasForeignKey(g => g.TeacherId);

            // Link Grade to Subject (One-to-Many)
            builder.HasOne(g => g.Subject)
                   .WithMany(sub => sub.Grades)
                   .HasForeignKey(g => g.SubjectId);
        }
    }
}