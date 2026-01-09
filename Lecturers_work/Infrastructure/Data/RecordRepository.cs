namespace Lecturers_work.Infrastructure.Data
{
    using Lecturers_work.Core.Entities;

    public class RecordRepository : CsvRepositoryBase<Record>
    {
        public RecordRepository(string path)
            : base(path) { }

        protected override string GetHeader() => "Id,CourseId,StudentId,Grade,IsPresent";

        protected override string ToCsv(Record r) =>
            $"{r.Id},{r.CourseId},{r.StudentId},{r.Grade},{r.IsPresent}";

        protected override Record FromCsv(string line)
        {
            var p = line.Split(',');
            return new Record
            {
                Id = int.Parse(p[0]),
                CourseId = int.Parse(p[1]),
                StudentId = int.Parse(p[2]),
                Grade = int.Parse(p[3]),
                IsPresent = bool.Parse(p[4])
            };
        }
    }
}