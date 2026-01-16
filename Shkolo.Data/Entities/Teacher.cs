using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Shkolo.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}