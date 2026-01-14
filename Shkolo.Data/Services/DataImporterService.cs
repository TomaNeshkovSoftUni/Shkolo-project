using Newtonsoft.Json;
using Shkolo.Data.DTOs;
using Shkolo.Data.ShkoloDbContext;
using Shkolo.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shkolo.Data.Services
{
    public class DataImporterService
    {
        private readonly ShkoloContext _context;

        public DataImporterService(ShkoloContext context)
        {
            // Always use the injected context
            _context = context;
        }

        public string ImportFromJson(string filePath)
        {
            // 1. Check if the file actually exists
            if (!File.Exists(filePath))
                return $"Error: File not found at {Path.GetFullPath(filePath)}";

            // 2. Read the text and turn it into C# objects (DTOs)
            var json = File.ReadAllText(filePath);
            var studentDtos = JsonConvert.DeserializeObject<List<StudentImportDto>>(json);

            if (studentDtos == null || !studentDtos.Any())
                return "Error: JSON file is empty or corrupted.";

            // 3. Prepare a "System" Teacher for the grades
            var teacher = _context.Teachers.FirstOrDefault();
            if (teacher == null)
            {
                teacher = new Teacher { FirstName = "System", LastName = "Importer" };
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
            }

            // 4. Loop through each student in the JSON
            foreach (var sDto in studentDtos)
            {
                // Safety Check: If the JSON key didn't match 'Class', skip or handle error
                if (string.IsNullOrEmpty(sDto.Class))
                {
                    continue; // Or throw an exception to debug
                }

                // Find or Create the Class (e.g., "10A")
                var schoolClass = _context.SchoolClasses
                    .FirstOrDefault(c => c.Name == sDto.Class);

                if (schoolClass == null)
                {
                    schoolClass = new SchoolClass { Name = sDto.Class };
                    _context.SchoolClasses.Add(schoolClass);
                    _context.SaveChanges();
                }

                // Create the Student and link to Class
                var student = new Student
                {
                    FirstName = sDto.FirstName,
                    LastName = sDto.LastName,
                    SchoolClassId = schoolClass.Id
                };

                _context.Students.Add(student);
                _context.SaveChanges(); // Save to get the student.Id for the grades

                // 5. Loop through the student's grades (matching the 'Grades' property in DTO)
                if (sDto.Grades != null)
                {
                    foreach (var gDto in sDto.Grades)
                    {
                        // Find or Create the Subject (e.g., "Math")
                        var subject = _context.Subjects
                            .FirstOrDefault(sub => sub.Name == gDto.SubjectName);

                        if (subject == null)
                        {
                            subject = new Subject { Name = gDto.SubjectName };
                            _context.Subjects.Add(subject);
                            _context.SaveChanges();
                        }

                        // Create the Grade record
                        var grade = new Grade
                        {
                            Value = gDto.Score,
                            StudentId = student.Id,
                            SubjectId = subject.Id,
                            TeacherId = teacher.Id,
                            DateGiven = DateTime.Now
                        };
                        _context.Grades.Add(grade);
                    }
                }
            }

            // 6. Final save for all the added grades
            _context.SaveChanges();
            return "Successfully imported all students, classes, and grades!";
        }
    }
}