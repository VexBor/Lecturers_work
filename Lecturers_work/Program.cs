using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using DotNetEnv;
using Spectre.Console;
using VxCourses.Application.Services;
using VxCourses.Core.Entities;
using VxCourses.Infrastructure.Data;

/// <summary>
/// Головний клас програми, що відповідає за запуск, ініціалізацію сервісів та відображення інтерфейсу користувача (UI).
/// </summary>
internal class Program
{
    /// <summary>
    /// Точка входу в програму.
    /// </summary>
    /// <param name="args">Не використовуються.</param>
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Env.Load();

        string adminPass = Environment.GetEnvironmentVariable("ADMIN_PASS") ?? "admin1";
        string adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin1";

        // 1. Ініціалізація
        var userRepo = new UserRepository("users.csv");
        var courseRepo = new CourseRepository("courses.csv");
        var recordRepo = new RecordRepository("records.csv");

        var authService = new AuthService(userRepo);
        var studyService = new StudyService(courseRepo, recordRepo, userRepo);

        authService.EnsureAdminCreated(adminEmail, adminPass);

        while (true)
        {
            if (authService.CurrentUser == null)
            {
                ShowLoginMenu(authService);
            }
            else
            {
                ShowMainMenu(authService, studyService);
            }
        }
    }

    /// <summary>
    /// Відображає меню входу та реєстрації для неавторизованих користувачів.
    /// </summary>
    /// <param name="auth">Екземпляр сервісу автентифікації.</param>
    ///
    private static void ShowLoginMenu(AuthService auth)
    {
        Title();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(5)
                .AddChoices("Увійти", "Реєстрація", "Вихід"));

        switch (choice)
        {
            case "Увійти":
                var email = AnsiConsole.Ask<string>(" [grey]Email:[/]");
                var pass = AnsiConsole.Prompt(new TextPrompt<string>(" [grey]Пароль:[/]").Secret());

                if (!auth.Login(email, pass))
                {
                    AnsiConsole.MarkupLine("[red]✕ Невірний логін або пароль[/]");
                }

                Pause();
                break;
            case "Реєстрація":
                AnsiConsole.MarkupLine("[teal]--- Створення нового акаунту ---[/]");

                var name = AnsiConsole.Ask<string>("Ваше ім'я:");
                var newEmail = AnsiConsole.Ask<string>("Email:");
                var newPass = AnsiConsole.Prompt(new TextPrompt<string>("Пароль:").Secret());

                var roleName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Хто ви?")
                        .AddChoices("Студент", "Викладач"));

                var role = roleName == "Викладач" ? UserRole.Teacher : UserRole.Student;

                try
                {
                    auth.Register(name, newEmail, newPass, role);
                    Success($"Акаунт створено! Тепер увійдіть як {roleName}.");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Помилка реєстрації: {ex.Message}[/]");
                    Pause();
                }

                break;
            case "Вихід":
                Environment.Exit(0);
                break;
        }
    }

    /// <summary>
    /// Відображає головне меню системи залежно від ролі авторизованого користувача.
    /// </summary>
    /// <param name="auth">Екземпляр сервісу автентифікації (для доступу до поточного користувача).</param>
    /// <param name="study">Екземпляр навчального сервісу (для виконання бізнес-логіки).</param>
    private static void ShowMainMenu(AuthService auth, StudyService study)
    {
        Title();
        AnsiConsole.MarkupLine($" Користувач: [bold white]{auth.CurrentUser.Name}[/] | Роль: [teal]{auth.CurrentUser.Role}[/]");
        AnsiConsole.WriteLine();

        var menuItems = new List<string>();

        if (auth.CurrentUser.Role == UserRole.Admin)
        {
            menuItems.AddRange(new[] { "Створити курс", "Додати користувача" });
        }
        else if (auth.CurrentUser.Role == UserRole.Teacher)
        {
            menuItems.AddRange(new[] { "Мої курси", "Створити курс", "Поставити оцінку" });
        }
        else if (auth.CurrentUser.Role == UserRole.Student)
        {
            menuItems.AddRange(new[] { "Всі курси", "Моя успішність" });
        }

        menuItems.Add("Вийти");

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(menuItems));

        try
        {
            if (selection == "Вийти")
            {
                auth.Logout();
                return;
            }

            // Адмін
            if (selection == "Створити курс" && auth.CurrentUser.Role == UserRole.Admin)
            {
                var allTeacher = study.GetUsersByRole(UserRole.Teacher);
                var title = AnsiConsole.Ask<string>("Назва:");
                var desc = AnsiConsole.Ask<string>("Опис:");
                var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<User>()
                         .Title("Оберіть викладача:")
                         .AddChoices(allTeacher)
                         .UseConverter(u => $"{u.Id} - {u.Name}"));
                var tid = selectedOption.Id;
                study.CreateCourse(title, desc, tid);
                Success("Курс створено");
            }
            else if (selection == "Додати користувача")
            {
                var name = AnsiConsole.Ask<string>("Ім'я:");
                var email = AnsiConsole.Ask<string>("Email:");
                var pass = AnsiConsole.Prompt(new TextPrompt<string>("Пароль:").Secret());
                var role = AnsiConsole.Prompt(new SelectionPrompt<UserRole>().AddChoices(UserRole.Teacher, UserRole.Student, UserRole.Admin));

                auth.Register(name, email, pass, role);
                Success($"Користувача {name} додано");
            }

            // Викладач
            else if (selection == "Мої курси")
            {
                var courses = study.GetCoursesByTeacher(auth.CurrentUser.Id);
                PrintTable(courses, "Мої курси");
                Pause();
            }
            else if (selection == "Створити курс")
            {
                var title = AnsiConsole.Ask<string>("Назва:");
                var desc = AnsiConsole.Ask<string>("Опис:");
                study.CreateCourse(title, desc, auth.CurrentUser.Id);
                Success("Курс створено");
            }
            else if (selection == "Поставити оцінку")
            {
                var courses = study.GetCoursesByTeacher(auth.CurrentUser.Id);
                var students = study.GetUsersByRole(UserRole.Student);

                var selectedOptionCId = AnsiConsole.Prompt(
                    new SelectionPrompt<Course>()
                     .Title("Оберіть курс:")
                    .AddChoices(courses)
                    .UseConverter(u => $"{u.Title} - {u.Description}"));
                var selectedOptionSId = AnsiConsole.Prompt(
                    new SelectionPrompt<User>()
                     .Title("Оберіть студента:")
                     .AddChoices(students)
                                    .UseConverter(u => $"{u.Id} - {u.Name}"));
                var cid = selectedOptionCId.Id;
                var sid = selectedOptionSId.Id;
                var grade = AnsiConsole.Ask<int>("Оцінка:");
                var present = AnsiConsole.Confirm("Був присутній?");

                while (grade > 12 || grade < 1)
                {
                    AnsiConsole.MarkupLine("[red]Введена оцінка не коректна.[/]");
                    grade = AnsiConsole.Ask<int>("Оцінка:");
                }

                study.GradeStudent(cid, sid, grade, present);
                Success("Журнал успішно оновлено!");
            }

            // Студент
            else if (selection == "Всі курси")
            {
                var courses = study.GetAllCourses();

                var search = AnsiConsole.Ask<string>("Введіть назву для пошуку (або 0 щоб показати всі):").ToLower();

                if (search != "0")
                {
                    courses = courses.Where(c => c.Title.ToLower().Contains(search)).ToList();
                }

                if (courses.Count == 0)
                {
                    AnsiConsole.MarkupLine($"[red]Не знайдено[/]");
                    Pause();
                }
                else
                {
                    PrintTable(courses, "Результати пошуку");
                    Pause();
                }

            }
            else if (selection == "Моя успішність")
            {
                double avg = study.GetStudentAverageGrade(auth.CurrentUser.Id);
                var color = avg >= 60 ? "green" : "red";
                AnsiConsole.MarkupLine($"Середній бал: [{color} bold]{avg:F1}[/]");
                Pause();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Помилка: {ex.Message}[/]");
            Pause();
        }
    }

    /// <summary>
    /// Виводить колекцію даних у вигляді відформатованої таблиці в консоль.
    /// </summary>
    /// <param name="items">Колекція об'єктів для відображення.</param>
    /// <param name="title">Заголовок таблиці.</param>
    private static void PrintTable(IEnumerable<dynamic> items, string title)
    {
        var table = new Table().Border(TableBorder.Minimal).Title($"[grey]{title}[/]");
        table.AddColumn("ID");
        table.AddColumn("Назва");
        table.AddColumn("Опис");

        foreach (var item in items)
        {
            table.AddRow((string)item.Id.ToString(), (string)item.Title.ToString(), (string)item.Description.ToString());
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Виводить повідомлення про успішне виконання операції зеленим кольором.
    /// </summary>
    /// <param name="msg">Текст повідомлення.</param>
    private static void Success(string msg)
    {
        AnsiConsole.MarkupLine($"[green]✓ {msg}[/]");
        Pause();
    }

    /// <summary>
    /// Зупиняє виконання програми до натискання клавіші Enter.
    /// </summary>
    private static void Pause()
    {
        AnsiConsole.Markup("[grey]Enter щоб продовжити...[/]");
        Console.ReadLine();
        Title();
    }

    /// <summary>
    /// Очищує консоль та виводить стилізований заголовок програми.
    /// </summary>
    private static void Title()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[teal]VxCourses[/]").RuleStyle("grey"));
    }
}