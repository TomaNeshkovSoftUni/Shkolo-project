using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

namespace Shkolo.Data.DTOs
{
    // This matches the "Student" objects in your JSON
    public class StudentImportDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        // 1. Change 'ClassName' to 'Class' to match JSON
        public string Class { get; set; } = null!;

        // 2. Change 'Results' to 'Grades' to match JSON
        public List<GradeImportDto> Grades { get; set; } = new();
    }

    // This matches the objects inside the "Results" array
    public class GradeImportDto
    {
        public string SubjectName { get; set; } = null!;
        public int Score { get; set; }
    }
}