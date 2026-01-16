using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shkolo.Data.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        public int? SchoolClassId { get; set; }

        [ForeignKey(nameof(SchoolClassId))]
        public virtual SchoolClass? SchoolClass { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();

        // This fulfills the Many-to-Many requirement
        public virtual ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();
    }
}