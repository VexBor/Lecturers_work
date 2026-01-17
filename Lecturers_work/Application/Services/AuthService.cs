namespace VxCourses.Application.Services
{
    using System.Linq;
    using VxCourses.Core.Entities;
    using VxCourses.Core.Interfaces;
    using VxCourses.Infrastructure.Utils;

    /// <summary>
    /// Сервіс, що відповідає за процеси автентифікації, реєстрації та керування сесією користувача.
    /// </summary>
    public class AuthService(IRepository<User> userRepo)
    {
        private readonly IRepository<User> _userRepo = userRepo;

        /// <summary>
        /// Gets отримує поточного авторизованого користувача.
        /// </summary>
        /// <value>Об'єкт User або null, якщо користувач не увійшов у систему.</value>
        public User? CurrentUser { get; private set; }

        /// <summary>
        /// Виконує вхід користувача в систему, перевіряючи відповідність Email та хешу пароля.
        /// </summary>
        /// <param name="email">Електронна пошта користувача.</param>
        /// <param name="password">Пароль (відкритий текст).</param>
        /// <returns>True, якщо вхід успішний; інакше — False.</returns>
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

        /// <summary>
        /// Реєструє нового користувача в системі.
        /// </summary>
        /// <param name="name">Ім'я користувача.</param>
        /// <param name="email">Електронна пошта (повинна бути унікальною).</param>
        /// <param name="password">Пароль (буде захешовано).</param>
        /// <param name="role">Роль користувача (Студент, Викладач, Адмін).</param>
        public bool Register(string name, string email, string password, UserRole role)
        {
            string passwordHash = SecurityUtils.HashPassword(password);
            var user = _userRepo.GetAll()
                .FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return false;
            }

            _userRepo.Add(new User
            {
                Name = name,
                Email = email,
                Password = passwordHash,
                Role = role
            });
            return true;
        }

        /// <summary>
        /// Перевіряє наявність адміністратора в базі даних. Якщо його немає — створює дефолтного.
        /// </summary>
        /// <param name="defaultEmail">Email адміністратора за замовчуванням.</param>
        /// <param name="defaultPassword">Пароль адміністратора за замовчуванням.</param
        public void EnsureAdminCreated(string defaultEmail, string defaultPassword)
        {
            bool adminExists = _userRepo.GetAll().Any(u => u.Role == UserRole.Admin);

            if (!adminExists)
            {
                Register("System Admin", defaultEmail, defaultPassword, UserRole.Admin);

                Console.WriteLine($"[SYSTEM] Створено першого адміністратора: {defaultEmail}");
            }
        }

        /// <summary>
        /// Завершує сесію поточного користувача.
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
        }
    }
}