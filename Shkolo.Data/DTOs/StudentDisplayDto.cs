using System;
using System.Collections.Generic;
using System.Text;

namespace Shkolo.Data.DTOs
{
    public class StudentDisplayDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
    }

    public class StudentReportDto
    {
        public string FullName { get; set; } = null!;
        public double AverageGrade { get; set; }
    }
}
