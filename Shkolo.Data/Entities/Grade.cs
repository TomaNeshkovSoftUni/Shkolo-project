using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shkolo.Data.Entities
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(2, 6)]
        public double Value { get; set; }
        public DateTime DateGiven { get; set; } = DateTime.Now;

        // Foreign Key to Student (One-to-Many)
        public int StudentId { get; set; }

        // Foreign Key to Subject (One-to-Many)
        public int SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; } = null!;

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        // Foreign Key to Teacher (One-to-Many)
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; } = null!;
    }
}