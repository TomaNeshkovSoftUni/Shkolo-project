using System;
using System.Collections.Generic;
using System.Text;

namespace Shkolo.Data.DTOs
{
    public class SubjectStatsDto
    {
        public string Name { get; set; } = null!;
        // Changed from 'Count' to 'StudentCount'
        public int StudentCount { get; set; }
    }
}