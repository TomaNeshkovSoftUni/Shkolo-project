using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shkolo.Data.DTOs;
using Shkolo.Data.DTOs.Shkolo.Data.DTOs;
using Shkolo.Data.Entities;
using Shkolo.Data.ShkoloDbContext;

namespace Shkolo.Data.Services
{
    /// <summary>
    /// Service layer handling all data logic.
    /// This keeps the DB logic out of the Console App project.
    /// </summary>
    public class DataImporterService
    {
        private readonly ShkoloContext _context;

        public DataImporterService(ShkoloContext context)
        {
            _context = context;
        }

        // --- JSON IMPORT LOGIC ---
        // Reads JSON, maps to DTOs, and saves multiple entities (Students, Grades, Subjects)
        public string ImportFromJson(string filePath)
        {
            if (!File.Exists(filePath)) return "Error: File not found.";

            try
            {
                var json = File.ReadAllText(filePath);
                var dtos = JsonConvert.DeserializeObject<List<StudentImportDto>>(json);

                if (dtos == null) return "Error: Invalid JSON structure.";

                // Check for a default teacher to satisfy Foreign Key constraints
                var systemTeacher = _context.Teachers.FirstOrDefault();
                if (systemTeacher == null)
                {
                    systemTeacher = new Teacher { FirstName = "System", LastName = "User" };
                    _context.Teachers.Add(systemTeacher);
                    _context.SaveChanges();
                }

                foreach (var dto in dtos)
                {
                    // Basic validation for empty fields
                    if (string.IsNullOrWhiteSpace(dto.FirstName)) continue;

                    // Code-First logic: Get existing class or create new one
                    var schoolClass = _context.SchoolClasses.FirstOrDefault(c => c.Name == dto.Class)
                                     ?? new SchoolClass { Name = dto.Class };

                    var student = new Student
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        SchoolClass = schoolClass
                    };

                    if (dto.Grades != null)
                    {
                        foreach (var gDto in dto.Grades)
                        {
                            var subject = _context.Subjects.FirstOrDefault(s => s.Name == gDto.SubjectName)
                                         ?? new Subject { Name = gDto.SubjectName };

                            student.Grades.Add(new Grade
                            {
                                Subject = subject,
                                Value = gDto.Score,
                                DateGiven = DateTime.Now.AddDays(-new Random().Next(1, 10)),
                                Teacher = systemTeacher
                            });
                        }
                    }
                    _context.Students.Add(student);
                }

                try
                {
                    _context.SaveChanges();
                    return $"Successfully imported {dtos.Count} students!";
                }
                catch (DbUpdateException ex)
                {
                    // Detailed error catcher for database conflicts
                    Console.WriteLine("\n--- DB ERROR ---");
                    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                    return "Database error. Check console.";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // --- LINQ QUERIES (Project Requirement: 8 Different Queries) ---

        // 1. Search with criteria
        public List<StudentDisplayDto> SearchStudentsByName(string search) =>
            _context.Students
                .Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search))
                .Select(s => new StudentDisplayDto
                {
                    FullName = s.FirstName + " " + s.LastName,
                    ClassName = s.SchoolClass != null ? s.SchoolClass.Name : "No Class"
                }).ToList();

        // 2. Aggregation and Rounding (GPA)
        public List<StudentDisplayDto> GetTopStudents(int count) =>
            _context.Students
                .Select(s => new StudentDisplayDto
                {
                    FullName = s.FirstName + " " + s.LastName,
                    AverageGrade = s.Grades.Any() ? Math.Round(s.Grades.Average(g => (double)g.Value), 2) : 0.00
                })
                .OrderByDescending(s => s.AverageGrade)
                .Take(count)
                .ToList();

        // 3. Simple filtering by related entity
        public List<StudentDisplayDto> GetStudentsByClass(string className) =>
            _context.Students
                .Where(s => s.SchoolClass.Name == className)
                .Select(s => new StudentDisplayDto { FullName = s.FirstName + " " + s.LastName })
                .ToList();

        // 4. Complex Grouping and Distinct Count
        public List<SubjectStatsDto> GetPopularSubjects()
        {
            return _context.Subjects
                .GroupBy(s => s.Name)
                .Select(group => new SubjectStatsDto
                {
                    Name = group.Key,
                    StudentCount = group.SelectMany(s => s.Grades).Select(g => g.StudentId).Distinct().Count()
                })
                .OrderByDescending(x => x.StudentCount)
                .ToList();
        }

        // 5. Filtering based on value threshold
        public List<StudentDisplayDto> GetFailingStudents() =>
            _context.Students
                .Where(s => s.Grades.Any(g => g.Value <= 2))
                .Select(s => new StudentDisplayDto { FullName = s.FirstName + " " + s.LastName })
                .ToList();

        // 6. Grouping and Aggregate Count
        public List<SubjectStatsDto> GetGradeCountsBySubject()
        {
            return _context.Grades
                .GroupBy(g => g.Subject.Name)
                .Select(group => new SubjectStatsDto
                {
                    Name = group.Key,
                    StudentCount = group.Count()
                })
                .OrderByDescending(x => x.StudentCount)
                .ToList();
        }

        // 7. Filtering by Date range and Joins
        public List<RecentGradeDto> GetRecentGrades()
        {
            return _context.Grades
                .Where(g => g.DateGiven >= DateTime.Now.AddDays(-30))
                .Select(g => new RecentGradeDto
                {
                    Student = g.Student.FirstName + " " + g.Student.LastName,
                    Value = g.Value,
                    Subject = g.Subject.Name,
                    Date = g.DateGiven
                })
                .OrderByDescending(g => g.Date)
                .ToList();
        }

        // 8. Finding entities with missing relationships
        public List<StudentDisplayDto> GetStudentsWithNoGrades() =>
            _context.Students
                .Where(s => !s.Grades.Any())
                .Select(s => new StudentDisplayDto { FullName = s.FirstName + " " + s.LastName })
                .ToList();

        // --- UTILITIES & EXPORT ---

        public List<string> GetPublicClasses() =>
            _context.SchoolClasses.Select(c => c.Name).OrderBy(n => n).ToList();

        // Requirement: JSON Export of a LINQ query result
        public string ExportTopStudentsToJson(string filePath)
        {
            var data = GetTopStudents(5);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
            return "Exported successfully.";
        }
    }
}