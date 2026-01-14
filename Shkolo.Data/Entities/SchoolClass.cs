using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shkolo.Data.Entities
{
    public class SchoolClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!; // e.g., "12A"
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
