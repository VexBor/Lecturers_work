namespace VxCourses.Application.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using VxCourses.Core.Entities;
    using VxCourses.Core.Interfaces;

    /// <summary>
    /// Сервіс, що реалізує бізнес-логіку навчального процесу (курси, оцінки, студенти).
    /// </summary>
    public class StudyService
    {
        private readonly IRepository<Course> _courseRepo;
        private readonly IRepository<Record> _recordRepo;
        private readonly IRepository<User> _userRepo;

        public StudyService(IRepository<Course> courseRepo, IRepository<Record> recordRepo, IRepository<User> userRepo)
        {
            _courseRepo = courseRepo;
            _recordRepo = recordRepo;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Створює новий навчальний курс.
        /// </summary>
        /// <param name="title">Назва курсу.</param>
        /// <param name="description">Опис курсу.</param>
        /// <param name="teacherId">ID викладача, який веде курс.</param>
        public void CreateCourse(string title, string description, int teacherId)
        {
            _courseRepo.Add(new Course
            {
                Title = title,
                Description = description,
                TeacherId = teacherId,
            });
        }

        /// <summary>
        /// Отримує повний список всіх курсів у системі.
        /// </summary>
        /// <returns>Список усіх курсів.</returns>
        public List<Course> GetAllCourses()
        {
            return _courseRepo.GetAll();
        }

        /// <summary>
        /// Отримує список курсів, закріплених за конкретним викладачем.
        /// </summary>
        /// <param name="teacherId">ID викладача.</param>
        /// <returns>Список курсів.</returns>
        public List<Course> GetCoursesByTeacher(int teacherId)
        {
            return _courseRepo.GetAll().Where(c => c.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Виставляє оцінку студенту за курс або оновлює існуючу.
        /// </summary>
        /// <param name="courseId">ID курсу.</param>
        /// <param name="studentId">ID студента.</param>
        /// <param name="grade">Оцінка (0-12).</param>
        /// <param name="isPresent">Чи був присутній.</param>
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

        /// <summary>
        /// Обчислює середній бал студента по всіх курсах, де є записи.
        /// </summary>
        /// <param name="studentId">ID студента.</param>
        /// <returns>Середнє арифметичне оцінок або 0, якщо оцінок немає.</returns>
        public double GetStudentAverageGrade(int studentId)
        {
            var records = _recordRepo.GetAll().Where(r => r.StudentId == studentId).ToList();
            return records.Any() ? records.Average(r => r.Grade) : 0;
        }

        /// <summary>
        /// Отримує повний список всіх користувачів у системі.
        /// </summary>
        /// <returns>Список усіх користувачів.</returns>
        public List<User> GetAllUsers()
        {
            return _userRepo.GetAll();
        }

        /// <summary>
        /// Отримує список користувачів певної ролі (наприклад, тільки студентів).
        /// </summary>
        /// <param name="role">Роль для фільтрації.</param>
        /// <returns>Список користувачів.</returns>
        public List<User> GetUsersByRole(UserRole role)
        {
            var users = _userRepo.GetAll().Where(r => r.Role == role);
            return users.ToList();
        }
    }
}