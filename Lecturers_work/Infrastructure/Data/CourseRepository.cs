namespace Lecturers_work.Infrastructure.Data
{
    using Lecturers_work.Core.Entities;

    public class CourseRepository : CsvRepositoryBase<Course>
    {
        public CourseRepository(string path)
            : base(path) { }

        protected override string GetHeader() => "Id,Title,Description,TeacherId";

        protected override string ToCsv(Course c) =>
            $"{c.Id},{c.Title},{c.Description},{c.TeacherId}";

        protected override Course FromCsv(string line)
        {
            var p = line.Split(',');
            return new Course
            {
                Id = int.Parse(p[0]),
                Title = p[1],
                Description = p[2],
                TeacherId = int.Parse(p[3]),
            };
        }
    }
}