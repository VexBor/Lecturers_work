using System.Collections.Generic;
using System.Linq;
using Lecturers_work.Core.Entities;
using Lecturers_work.Core.Interfaces;

namespace Lecturers_work.Application.Services
{
    public class StudyService
    {
        private readonly IRepository<Course> _courseRepo;
        private readonly IRepository<Record> _recordRepo;

        public StudyService(IRepository<Course> courseRepo, IRepository<Record> recordRepo)
        {
            _courseRepo = courseRepo;
            _recordRepo = recordRepo;
        }

        public void CreateCourse(string title, string description, int teacherId)
        {
            _courseRepo.Add(new Course
            {
                Title = title,
                Description = description,
                TeacherId = teacherId,
            });
        }

        public List<Course> GetAllCourses()
        {
            return _courseRepo.GetAll();
        }

        public List<Course> GetCoursesByTeacher(int teacherId)
        {
            return _courseRepo.GetAll().Where(c => c.TeacherId == teacherId).ToList();
        }

        public void GradeStudent(int courseId, int studentId, int grade, bool isPresent)
        {
            var existingRecord = _recordRepo.GetAll()
                .FirstOrDefault(r => r.CourseId == courseId && r.StudentId == studentId);

            if (existingRecord != null)
            {
                existingRecord.Grade = grade;
                existingRecord.IsPresent = isPresent;
                _recordRepo.Update(existingRecord);
            }
            else
            {
                _recordRepo.Add(new Record
                {
                    CourseId = courseId,
                    StudentId = studentId,
                    Grade = grade,
                    IsPresent = isPresent
                });
            }
        }

        public double GetStudentAverageGrade(int studentId)
        {
            var records = _recordRepo.GetAll().Where(r => r.StudentId == studentId).ToList();
            return records.Any() ? records.Average(r => r.Grade) : 0;
        }
    }
}