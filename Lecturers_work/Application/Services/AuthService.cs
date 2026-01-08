namespace Lecturers_work.Application.Services
{
    using System.Linq;
    using Lecturers_work.Core.Entities;
    using Lecturers_work.Core.Interfaces;

    public class AuthService
    {
        private readonly IRepository<User> _userRepo;
        public User CurrentUser { get; private set; }

        public AuthService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public bool Login(string email, string password)
        {
            var user = _userRepo.GetAll()
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public void Register(string name, string email, string password, UserRole role)
        {
            _userRepo.Add(new User
            {
                Name = name,
                Email = email,
                Password = password,
                Role = role
            });
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}