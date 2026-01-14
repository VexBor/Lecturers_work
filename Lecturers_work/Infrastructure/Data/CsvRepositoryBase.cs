namespace VxCourses.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VxCourses.Core.Interfaces;

    /// <summary>
    /// Базовий абстрактний клас репозиторію для роботи з CSV-файлами.
    /// Реалізує основні CRUD-операції.
    /// </summary>
    /// <typeparam name="T">Тип сутності, що наслідується від IEntity.</typeparam>
    public abstract class CsvRepositoryBase<T> : IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// Шлях до файлу бази даних.
        /// </summary>
        private readonly string _filePath;

        protected CsvRepositoryBase(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, GetHeader() + Environment.NewLine);
            }
        }

        protected abstract string GetHeader();

        protected abstract T FromCsv(string line);

        protected abstract string ToCsv(T entity);

        /// <summary>
        /// Отримує всі записи з файлу.
        /// </summary>
        /// <returns>Список сутностей типу T.</returns>
        public List<T> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            return File.ReadAllLines(_filePath)
                .Skip(1)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(line =>
                {
                    try
                    {
                        return FromCsv(line);
                    }
                    catch
                    {
                        return default;
                    }
                })
                .Where(x => x != null)
                .ToList();
        }

        public T GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Додає нову сутність у файл, автоматично генеруючи новий ID.
        /// </summary>
        /// <param name="entity">Об'єкт для додавання.</param>
        public void Add(T entity)
        {
            var all = GetAll();
            entity.Id = all.Any() ? all.Max(x => x.Id) + 1 : 1;
            File.AppendAllText(_filePath, ToCsv(entity) + Environment.NewLine);
        }

        /// <summary>
        /// Оновлює сутність у файлу.
        /// </summary>
        /// <param name="entity">Об'єкт для оновлення.</param>
        public void Update(T entity)
        {
            var all = GetAll();
            var index = all.FindIndex(x => x.Id == entity.Id);
            if (index != -1)
            {
                all[index] = entity;
                SaveChanges(all);
            }
        }

        /// <summary>
        /// Видаляє сутність з файлу.
        /// </summary>
        /// <param name="id">Об'єкт для видалення.</param>
        public void Delete(int id)
        {
            var all = GetAll();
            all.RemoveAll(x => x.Id == id);
            SaveChanges(all);
        }

        /// <summary>
        /// Збереження змін.
        /// </summary>
        /// <param name="items">Список сутностей типу T.</param>
        private void SaveChanges(List<T> items)
        {
            var lines = new List<string> { GetHeader() };
            lines.AddRange(items.Select(ToCsv));
            File.WriteAllLines(_filePath, lines);
        }
    }
}