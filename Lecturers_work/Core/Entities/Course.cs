namespace VxCourses.Core.Entities
{
    using VxCourses.Core.Interfaces;

    public class Course : IEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int TeacherId { get; set; }
    }
}