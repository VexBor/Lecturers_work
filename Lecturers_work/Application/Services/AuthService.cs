namespace Lecturers_work.Application.Services
{
    using System.Linq;
    using Lecturers_work.Core.Entities;
    using Lecturers_work.Core.Interfaces;
    using Lecturers_work.Infrastructure.Utils;

    public class AuthService(IRepository<User> userRepo)
    {
        private readonly IRepository<User> _userRepo = userRepo;

        public User? CurrentUser { get; private set; }

        public bool Login(string email, string password)
        {
            string inputHash = SecurityUtils.HashPassword(password);

            var user = _userRepo.GetAll()
                .FirstOrDefault(u => u.Email == email && u.Password == inputHash);

            if (user != null)
            {
                CurrentUser = user;
                return true;
            }

            return false;
        }

        public void Register(string name, string email, string password, UserRole role)
        {
            string passwordHash = SecurityUtils.HashPassword(password);

            _userRepo.Add(new User
            {
                Name = name,
                Email = email,
                Password = passwordHash,
                Role = role
            });
        }

        public void EnsureAdminCreated(string defaultEmail, string defaultPassword)
        {
            bool adminExists = _userRepo.GetAll().Any(u => u.Role == UserRole.Admin);

            if (!adminExists)
            {
                Register("System Admin", defaultEmail, defaultPassword, UserRole.Admin);

                Console.WriteLine($"[SYSTEM] Створено першого адміністратора: {defaultEmail}");
            }
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}