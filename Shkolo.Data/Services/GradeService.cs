using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shkolo.Data.Entities;

namespace Shkolo.Data.Services
{
    internal class GradeService
    {
        private readonly ShkoloContext? _context;
        public void AddGrade(int studentId, int subjectId, int teacherId, int value)
        {
            var newGrade = new Grade
            {
                StudentId = studentId,
                SubjectId = subjectId,
                TeacherId = teacherId,
                Value = value,
                DateGiven = DateTime.Now
            };

            _context.Grades.Add(newGrade);
            _context.SaveChanges();
        }

        // 2. Change an existing mark
        public void UpdateGrade(int gradeId, int newValue)
        {
            var grade = _context.Grades.Find(gradeId);
            if (grade != null)
            {
                grade.Value = newValue;
                _context.SaveChanges();
            }
        }
    }
}
