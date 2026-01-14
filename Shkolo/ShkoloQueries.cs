using System;
using System.Collections.Generic;
using System.Text;

using Shkolo.Data;
using Microsoft.EntityFrameworkCore;

namespace Shkolo.App.Services
{
    public class StudentReportService
    {
        private readonly ShkoloContext _context;

        public StudentReportService(ShkoloContext context)
        {
            _context = context;
        }

        public void PrintAllReports()
        {
            Console.WriteLine("=== SHKOLO ANALYTICS REPORT ===");

            // 1. GPA Order
            var byGpa = _context.Students
            .Select(s => new { Name = $"{s.FirstName} {s.LastName}", GPA = s.Grades.Any() ? s.Grades.Average(g => g.Value) : 0 })
            .OrderByDescending(x => x.GPA).ToList();
            PrintList("Students by GPA", byGpa.Select(x => $"{x.Name} - {x.GPA}"));

            // 2. Reverse Alpha
            var revAlpha = _context.Students.OrderByDescending(s => s.FirstName).ThenByDescending(s => s.LastName).ToList();
            PrintList("Reverse Alphabetical", revAlpha.Select(s => $"{s.FirstName} {s.LastName}"));

            // 3. Starts with 'A'
            var startsWithA = _context.Students.Where(s => s.FirstName.StartsWith("A")).ToList();
            PrintList("Names Starting with 'A'", startsWithA.Select(s => s.FirstName));

            // 4. History 4s
            var historyFours = _context.Students
                .Where(s => s.Grades.Any(g => g.Subject.Name == "History" && g.Value == 4)).ToList();
            PrintList("Students with a 4 in History", historyFours.Select(s => s.FirstName));

            // 5. Short Names (< 5 chars)
            var shortNames = _context.Students.Where(s => s.FirstName.Length < 5).ToList();
            PrintList("Names shorter than 5 letters", shortNames.Select(s => s.FirstName));

            // 6. Prime IDs (Recursive)
            var primeIds = _context.Students.AsEnumerable().Where(s => IsPrimeRecursive(s.Id)).ToList();
            PrintList("Students with Prime Internal IDs", primeIds.Select(s => $"ID: {s.Id} - {s.FirstName}"));
        }

        private void PrintList(string title, IEnumerable<string> items)
        {
            Console.WriteLine($"\n--- {title} ---");
            foreach (var item in items) Console.WriteLine($" > {item}");
        }

        private bool IsPrimeRecursive(int n, int divisor = 3)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            if (divisor * divisor > n) return true;
            if (n % divisor == 0) return false;
            return IsPrimeRecursive(n, divisor + 2);
        }
    }
}