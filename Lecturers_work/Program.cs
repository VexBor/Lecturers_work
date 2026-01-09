using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using DotNetEnv;
using Lecturers_work.Application.Services;
using Lecturers_work.Core.Entities;
using Lecturers_work.Infrastructure.Data;
using Spectre.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Env.Load();

        string adminPass = Environment.GetEnvironmentVariable("ADMIN_PASS") ?? "admin";
        string adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin";

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
                var title = AnsiConsole.Ask<string>("Назва:");
                var desc = AnsiConsole.Ask<string>("Опис:");
                var tid = AnsiConsole.Ask<int>("ID Викладача:");
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
                PrintTable(courses, "Мої курси");

                var students = study.GetUsersByRole(UserRole.Student);

                var table = new Table().Border(TableBorder.Minimal).Title($"[grey]Студенти[/]");
                table.AddColumn("Id");
                table.AddColumn("Ім\'я");
                foreach (var student in students)
                {
                    table.AddRow(student.Id.ToString(), student.Name.ToString());
                }

                AnsiConsole.Write(table);

                var cid = AnsiConsole.Ask<int>("ID Курсу:");
                var sid = AnsiConsole.Ask<int>("ID Студента:");
                var grade = AnsiConsole.Ask<int>("Оцінка:");
                var present = AnsiConsole.Confirm("Був присутній?");

                if (courses.Any(c => c.Id == cid) && students.Any(s => s.Id == sid))
                {
                    study.GradeStudent(cid, sid, grade, present);
                    Success("Журнал оновлено");
                    return;
                }

                AnsiConsole.MarkupLine($"[red]Помилка: введеного Id не існує[/]");
                Pause();
            }

            // Студент
            else if (selection == "Всі курси")
            {
                var courses = study.GetAllCourses();
                PrintTable(courses, "Список курсів");
                Pause();
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

    private static void Success(string msg)
    {
        AnsiConsole.MarkupLine($"[green]✓ {msg}[/]");
        Pause();
    }

    private static void Pause()
    {
        AnsiConsole.Markup("[grey]Enter щоб продовжити...[/]");
        Console.ReadLine();
        Title();
    }

    private static void Title()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[teal]VxCourses[/]").RuleStyle("grey"));
    }
}