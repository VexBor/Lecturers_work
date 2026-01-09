namespace Lecturers_work.Core.Entities
{
    using Lecturers_work.Core.Interfaces;

    public class Course : IEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int TeacherId { get; set; }
    }
}