namespace VxCourses.Infrastructure.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Статичний клас для допоміжних функцій безпеки.
    /// </summary>
    public static class SecurityUtils
    {
        /// <summary>
        /// Хешує вхідний рядок (пароль) за допомогою алгоритму SHA-256.
        /// </summary>
        /// <param name="password">Пароль у вигляді відкритого тексту.</param>
        /// <returns>Рядок, що містить хеш у шістнадцятковому форматі.</returns>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);

                var hash = sha256.ComputeHash(bytes);

                var builder = new StringBuilder();
                foreach (var b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
