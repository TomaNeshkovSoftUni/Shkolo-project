using Newtonsoft.Json;
using Shkolo.Data.DTOs;
using Shkolo.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Shkolo.Data.Services
{
    public class DataImporterService
    {
        private readonly ShkoloContext _context;

        public DataImporterService(ShkoloContext context)
        {
            _context = context;
        }

        // --- 1. JSON IMPORT ---
        public string ImportFromJson(string filePath)
        {
            if (!File.Exists(filePath)) return "Error: File not found.";

            var json = File.ReadAllText(filePath);
            var dtos = JsonConvert.DeserializeObject<List<StudentImportDto>>(json);

            if (dtos == null) return "Error: Invalid JSON structure.";

            int importedCount = 0;
            foreach (var dto in dtos)
            {
                // Requirement: Validation (Base check)
                if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName)) continue;

                var student = new Student
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

                // Logic to find/create class from JSON
                var schoolClass = _context.SchoolClasses.FirstOrDefault(c => c.Name == dto.Class);
                if (schoolClass == null)
                {
                    schoolClass = new SchoolClass { Name = dto.Class };
                    _context.SchoolClasses.Add(schoolClass);
                }
                student.SchoolClass = schoolClass;

                _context.Students.Add(student);
                importedCount++;
            }

            _context.SaveChanges();
            return $"Successfully imported {importedCount} students!";
        }

        // --- 2. JSON EXPORT ---
        public string ExportTopStudentsToJson(string filePath)
        {
            // Requirement: Export a LINQ query result
            var topStudents = GetTopStudents(10);
            var json = JsonConvert.SerializeObject(topStudents, Formatting.Indented);
            File.WriteAllText(filePath, json);
            return $"Successfully exported data to {filePath}";
        }

        // --- 3. THE 8 MANDATORY LINQ QUERIES ---

        // Query 1: Search by Criterion (FirstName or LastName)
        public List<StudentDisplayDto> SearchStudentsByName(string search) =>
            _context.Students
                .Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search))
                .Select(s => new StudentDisplayDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassName = s.SchoolClass != null ? s.SchoolClass.Name : "No Class"
                }).ToList();

        // Query 2: Statistics (Top Students by Average Grade)
        public List<StudentReportDto> GetTopStudents(int count) =>
            _context.Students
                .Select(s => new StudentReportDto
                {
                    FullName = s.FirstName + " " + s.LastName,
                    AverageGrade = s.Grades.Any() ? s.Grades.Average(g => g.Value) : 0
                })
                .OrderByDescending(s => s.AverageGrade)
                .Take(count)
                .ToList();

        // Query 3: Filtering by Class
        public List<StudentDisplayDto> GetStudentsByClass(string className) =>
            _context.Students
                .Where(s => s.SchoolClass.Name == className)
                .Select(s => new StudentDisplayDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassName = s.SchoolClass.Name
                }).ToList();

        // Query 4: Many-to-Many check (Popularity)
        public dynamic GetPopularSubjects() =>
            _context.Subjects
                .OrderByDescending(sub => sub.Students.Count)
                .Select(sub => new { sub.Name, StudentCount = sub.Students.Count })
                .ToList();

        // Query 5: Failing Students
        public List<StudentDisplayDto> GetFailingStudents() =>
            _context.Students
                .Where(s => s.Grades.Any(g => g.Value < 3.00))
                .Select(s => new StudentDisplayDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassName = s.SchoolClass.Name
                }).ToList();

        // Query 6: Grouping (Grades per Subject)
        public dynamic GetGradeCountsBySubject() =>
            _context.Grades
                .GroupBy(g => g.Subject.Name)
                .Select(group => new { Subject = group.Key, Count = group.Count() })
                .ToList();

        // Query 7: Date-based query (Last 30 days)
        public dynamic GetRecentGrades() =>
            _context.Grades
                .Where(g => g.DateGiven >= DateTime.Now.AddDays(-30))
                .Select(g => new { Student = g.Student.FirstName, g.Value, Subject = g.Subject.Name })
                .ToList();

        // Query 8: Empty/Null Check (Students with no grades)
        public List<StudentDisplayDto> GetStudentsWithNoGrades() =>
            _context.Students
                .Where(s => !s.Grades.Any())
                .Select(s => new StudentDisplayDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassName = s.SchoolClass != null ? s.SchoolClass.Name : "N/A"
                }).ToList();
    }
}