using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Shkolo.Data.Entities
{

    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public int? SchoolClassId { get; set; }
        public virtual SchoolClass? SchoolClass { get; set; }
        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
        public virtual ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();
    }
}
