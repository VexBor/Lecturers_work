using Lecturers_work.Core.Interfaces;

namespace Lecturers_work.Core.Entities
{
    public enum UserRole { Admin, Teacher, Student }

    public class User : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }
    }
}