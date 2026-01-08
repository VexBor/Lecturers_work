using System;
using System.Text;
using Lecturers_work.Application.Services;
using Lecturers_work.Core.Entities;
using Lecturers_work.Infrastructure.Data;
using Spectre.Console;

class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // 1. Ініціалізація
        var userRepo = new UserRepository("users.csv");
        var courseRepo = new CourseRepository("courses.csv");
        var recordRepo = new RecordRepository("records.csv");

        var authService = new AuthService(userRepo);
        var studyService = new StudyService(courseRepo, recordRepo);

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

    static void ShowLoginMenu(AuthService auth)
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

    static void ShowMainMenu(AuthService auth, StudyService study)
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
                var role = AnsiConsole.Prompt(new SelectionPrompt<UserRole>().AddChoices(UserRole.Teacher, UserRole.Student));

                auth.Register(name, email, pass, role);
                Success($"Користувача {name} додано");
            }

            // Викладач
            else if (selection == "Мої курси")
            {
                var courses = study.GetCoursesByTeacher(auth.CurrentUser.Id);
                PrintTable(courses, "Мої курси");
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
                var cid = AnsiConsole.Ask<int>("ID Курсу:");
                var sid = AnsiConsole.Ask<int>("ID Студента:");
                var grade = AnsiConsole.Ask<int>("Оцінка:");
                var present = AnsiConsole.Confirm("Був присутній?");

                study.GradeStudent(cid, sid, grade, present);
                Success("Журнал оновлено");
            }

            // Студент
            else if (selection == "Всі курси")
            {
                var courses = study.GetAllCourses();
                PrintTable(courses, "Список курсів");
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

    static void PrintTable(IEnumerable<dynamic> items, string title)
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
        Pause();
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