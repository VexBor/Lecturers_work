namespace VxCourses.Core.Entities
{
    using VxCourses.Core.Interfaces;

    /// <summary>
    /// Сутність, що представляє користувача системи.
    /// </summary>
    public enum UserRole
    {
        Admin,
        Teacher,
        Student
    }

    public class User : IEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public UserRole Role { get; set; }
    }
}