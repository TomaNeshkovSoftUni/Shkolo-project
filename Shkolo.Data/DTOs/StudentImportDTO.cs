using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

namespace Shkolo.Data.DTOs
{
    public class StudentImportDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Class { get; set; } = null!;
        public List<GradeImportDto> Grades { get; set; } = new();
    }

    public class GradeImportDto
    {
        public string SubjectName { get; set; } = null!;
        public double Score { get; set; }
    }
}