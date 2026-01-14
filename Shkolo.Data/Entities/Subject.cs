using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Shkolo.Data.Entities
{

    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        // Many-to-Many: One Subject has many Students
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
