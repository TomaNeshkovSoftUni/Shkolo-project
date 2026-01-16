using System;
using System.Collections.Generic;
using System.Text;

namespace Shkolo.Data.DTOs
{
    namespace Shkolo.Data.DTOs
    {
        public class StudentDisplayDto
        {
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string FullName { get; set; } = null!;

            public string ClassName { get; set; } = null!;
            public double AverageGrade { get; set; }
        }
    }

    namespace Shkolo.Data.DTOs
    {
        public class StudentReportDto
        {
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;

            // Changed to allow assignment from your LINQ queries
            public string FullName { get; set; } = null!;

            public string ClassName { get; set; } = null!;
            public double AverageGrade { get; set; }
        }
    }
}
