using System;
using System.Collections.Generic;
using System.Text;

namespace Shkolo.Data.DTOs
{
    public class RecentGradeDto
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; } = null!;
        public string Student { get; set; } = null!;
        public double Value { get; set; }
    }
}