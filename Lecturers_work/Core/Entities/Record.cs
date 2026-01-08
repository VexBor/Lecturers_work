using Lecturers_work.Core.Interfaces;

namespace Lecturers_work.Core.Entities
{
    public class Record : IEntity
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public int Grade { get; set; }
        public bool IsPresent { get; set; }
    }
}