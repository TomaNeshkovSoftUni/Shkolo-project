using System;
using System.Collections.Generic;
using System.Linq;
using Shkolo.Data.Entities;
using Shkolo.Data.ShkoloDbContext;

namespace Shkolo.Data.Services
{
    public class GradeService
    {
        private readonly ShkoloContext _context;

        public GradeService(ShkoloContext context)
        {
            _context = context;
        }

        public void PrintSpecific(string choice)
        {
            switch (choice)
            {
                case "1": PrintGpaReport(); break;
                case "2": PrintHistoryClassReport(); break;
                case "3": PrintShortNamesReport(); break;
                case "4": PrintPrimeIdReport(); break;
                default: Console.WriteLine("Invalid selection."); break;
            }
        }

        public void AddGrade(int studentId, int subjectId, int teacherId, int value)
        {
            _context.Grades.Add(new Grade
            {
                StudentId = studentId,
                SubjectId = subjectId,
                TeacherId = teacherId,
                Value = value,
                DateGiven = DateTime.Now
            });
            _context.SaveChanges();
        }

        private void PrintGpaReport()
        {
            var byGpa = _context.Students
                .Select(s => new { Name = $"{s.FirstName} {s.LastName}", GPA = s.Grades.Any() ? s.Grades.Sum(g => g.Value) / s.Grades.Count() : 0 })
                .OrderByDescending(x => x.GPA).ToList();
            Console.WriteLine("\n--- GPA Report ---");
            foreach (var x in byGpa) Console.WriteLine($" > {x.Name} - {x.GPA}");
        }

        private void PrintHistoryClassReport()
        {
            var history = _context.Students.Where(s => s.Grades.Any(g => g.Subject.Name == "History")).ToList();
            Console.WriteLine("\n--- History Students ---");
            foreach (var s in history) Console.WriteLine($" > {s.FirstName} {s.LastName}");
        }

        private void PrintShortNamesReport()
        {
            var shorts = _context.Students.Where(s => s.FirstName.Length < 5).ToList();
            Console.WriteLine("\n--- Short Names ---");
            foreach (var s in shorts) Console.WriteLine($" > {s.FirstName}");
        }

        private void PrintPrimeIdReport()
        {
            var primes = _context.Students.AsEnumerable().Where(s => IsPrimeRecursive(s.Id)).ToList();
            Console.WriteLine("\n--- Prime IDs ---");
            foreach (var s in primes) Console.WriteLine($" > ID: {s.Id} - {s.FirstName}");
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