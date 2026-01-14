namespace VxCourses.Infrastructure.Data
{
    using VxCourses.Core.Entities;

    public class UserRepository : CsvRepositoryBase<User>
    {
        public UserRepository(string path)
            : base(path) { }

        protected override string GetHeader() => "Id,Name,Email,Password,Role";

        protected override string ToCsv(User u) =>
            $"{u.Id},{u.Name},{u.Email},{u.Password},{(int)u.Role}";

        protected override User FromCsv(string line)
        {
            var p = line.Split(',');
            return new User
            {
                Id = int.Parse(p[0]),
                Name = p[1],
                Email = p[2],
                Password = p[3],
                Role = (UserRole)int.Parse(p[4])
            };
        }
    }
}