using System;
using System.Text;
using Lecturers_work.Application.Services;
using Lecturers_work.Core.Entities;
using Lecturers_work.Infrastructure.Data;

class Program
{
    static void Main(string[] args)
    {
<<<<<<< Updated upstream
        
        public static void Main(string[] args)
        {
            string name = @"
 __      __          _____ 
 \ \    / /         / ____|                                          
  \ \  / /  __  __ | |        ___    _   _   _ __   ___    ___   ___ 
   \ \/ /   \ \/ / | |       / _ \  | | | | | '__| / __|  / _ \ / __|
    \  /     >  <  | |____  | (_) | | |_| | | |    \__ \ |  __/ \__ \
     \/     /_/\_\  \_____|  \___/   \__,_| |_|    |___/  \___| |___/";

            int totalPrice;
            double finishPrice;
            int mathPrice = 849;
            int ukrLanguagesPrice = 549;
            int literaturePrice = 649;
            int historyPrice = 1049;
            int phisicsPrice = 1495;
            int geographyPrice = 789;
            
            double randomDiscount = new Random().NextDouble() * 10;
            double totalDiscount = Math.Round(randomDiscount, 2);
            double discountCount;
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(name +"\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Список курсів: \n");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"° Математика (Мельник Олександр Іванович) - {mathPrice} грн/міс");
            Console.WriteLine($"° Українська мова (Шевченко Тетяна Петрівна) - {ukrLanguagesPrice} грн/міс");
            Console.WriteLine($"° Українська література (Коваленко Андрій Васильович) - {literaturePrice} грн/міс");
            Console.WriteLine($"° Історія України (Бондаренко Наталія Миколаївна) - {historyPrice} грн/міс");
            Console.WriteLine($"° Фізика (Ткаченко Сергій Олександрович) - {phisicsPrice} грн/міс");
            Console.WriteLine($"° Географія (Ковальчук Ірина Дмитрівна) - {geographyPrice} грн/міс \n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Для продовження натисніть будь-яку клавішу. ");
            Console.ReadKey();
            Console.WriteLine("\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Максимальна тривалість навчання 12 місяців.");
            Console.WriteLine("Оберіть бажану тривалість навчання (місяців): \n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Математика: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            byte mounthsMath = Convert.ToByte(Console.ReadLine());
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Українська мова: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            byte monthsUrkLanguages = Convert.ToByte(Console.ReadLine());
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Українська література: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            byte mounthsLiterature = Convert.ToByte(Console.ReadLine());
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Історія України: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            byte mounthsHistory = Convert.ToByte(Console.ReadLine());
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Фізика: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            byte mounthsPhisics = Convert.ToByte(Console.ReadLine());
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Географія: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            byte mounthsGeography = Convert.ToByte(Console.ReadLine());
            Console.ResetColor();

            totalPrice = mounthsMath * mathPrice + monthsUrkLanguages * ukrLanguagesPrice 
                                                 + mounthsLiterature * literaturePrice + mounthsHistory * historyPrice 
                                                 + mounthsPhisics * phisicsPrice +  mounthsGeography * geographyPrice;
            discountCount = Math.Round(totalPrice * (totalDiscount / 100), 2);
            finishPrice = totalPrice - discountCount;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Загальна сума: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(totalPrice + " грн");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Знижка у відсотках :");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(totalDiscount + " %");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Знижка в гривнях: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(discountCount + " грн");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\nСума зі знижкою: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(finishPrice + " грн");
            Console.ResetColor();
=======
        // 1. Ініціалізація
        var userRepo = new UserRepository("users.csv");
        var courseRepo = new CourseRepository("courses.csv");
        var recordRepo = new RecordRepository("records.csv");

        var authService = new AuthService(userRepo);
        var studyService = new StudyService(courseRepo, recordRepo);

        // 2. Запуск меню
        while (true)
        {
            if (authService.CurrentUser == null)
                ShowLoginMenu(authService);
            else
                ShowMainMenu(authService, studyService);

        }
    }

    static void ShowLoginMenu(AuthService auth)
    {
        Console.Clear();
        Console.WriteLine("=== Вхід ===");
        Console.WriteLine("1. Увійти");
        Console.WriteLine("2. Зареєструватися");
        Console.WriteLine("3. Вихід");
        Console.Write("> ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("Email: "); var e = Console.ReadLine();
                Console.Write("Пароль: "); var p = Console.ReadLine();
                if (!auth.Login(e, p))
                {
                    Console.WriteLine("Помилка! Натисніть Enter.");
                    Console.ReadLine();
                }
                break;
            case "2":
                Console.Write("Ім'я: "); string n = Console.ReadLine();
                Console.Write("Email: "); string em = Console.ReadLine();
                Console.Write("Пароль: "); string ps = Console.ReadLine();
                UserRole r = UserRole.Student;
                auth.Register(n, em, ps, r);
                Console.WriteLine("Користувача додано! Натисніть Enter.");
                Console.ReadLine();
                auth.Login(em, ps);
                break;
            case "3":
                Environment.Exit(0);
                break;
        }
    }

    static void ShowMainMenu(AuthService auth, StudyService study)
    {
        Console.Clear();
        Console.WriteLine($"Вітаємо, {auth.CurrentUser.Name} ({auth.CurrentUser.Role})");

        if (auth.CurrentUser.Role == UserRole.Admin)
        {
            Console.WriteLine("1. Створити курс");
            Console.WriteLine("2. Зареєструвати нового користувача");
        }
        else if (auth.CurrentUser.Role == UserRole.Teacher)
        {
            Console.WriteLine("1. Мої курси");
            Console.WriteLine("2. Поставити оцінку");
        }
        else if (auth.CurrentUser.Role == UserRole.Student)
        {
            Console.WriteLine("1. Список курсів");
            Console.WriteLine("2. Моя успішність");
            Console.WriteLine("3. Інформація про сервіс");
        }

        Console.WriteLine("0. Вихід з акаунту");
        Console.Write("> ");
        string choice = Console.ReadLine();

        try
        {
            if (choice == "0") auth.Logout();

            // Адмін
            else if (auth.CurrentUser.Role == UserRole.Admin && choice == "1")
            {
                Console.Write("Назва: "); string t = Console.ReadLine();
                Console.Write("Опис: "); string d = Console.ReadLine();
                Console.Write("ID викладача: "); int tid = int.Parse(Console.ReadLine());
                Console.Write("Ціна на курс: "); int price = int.Parse(Console.ReadLine());
                study.CreateCourse(t, d, tid, price);
                Console.WriteLine("Створено!");
            }
            else if (auth.CurrentUser.Role == UserRole.Admin && choice == "2")
            {
                Console.Write("Ім'я: "); string n = Console.ReadLine();
                Console.Write("Email: "); string em = Console.ReadLine();
                Console.Write("Пароль: "); string ps = Console.ReadLine();
                Console.Write("Роль (1-Викладач, 2-Студент): ");
                UserRole r = (UserRole)int.Parse(Console.ReadLine());
                auth.Register(n, em, ps, r);
                Console.WriteLine("Користувача додано!");
            }

            // Викладач
            else if (auth.CurrentUser.Role == UserRole.Teacher && choice == "1")
            {
                var courses = study.GetCoursesByTeacher(auth.CurrentUser.Id);
                foreach (var c in courses) Console.WriteLine($"ID: {c.Id} | {c.Title}");
                Console.ReadLine();
            }
            else if (auth.CurrentUser.Role == UserRole.Teacher && choice == "2")
            {
                Console.Write("ID курсу: "); int cid = int.Parse(Console.ReadLine());
                Console.Write("ID студента: "); int sid = int.Parse(Console.ReadLine());
                Console.Write("Оцінка: "); int gr = int.Parse(Console.ReadLine());
                Console.Write("Був присутній (true/false): "); bool pr = bool.Parse(Console.ReadLine());
                study.GradeStudent(cid, sid, gr, pr);
                Console.WriteLine("Оцінку збережено!");
            }

            // Студент
            else if (auth.CurrentUser.Role == UserRole.Student && choice == "1")
            {
                var all = study.GetAllCourses();
                foreach (var c in all) Console.WriteLine($"{c.Id}. {c.Title} - {c.Description}");
                Console.ReadLine();
            }
            else if (auth.CurrentUser.Role == UserRole.Student && choice == "2")
            {
                double avg = study.GetStudentAverageGrade(auth.CurrentUser.Id);
                Console.WriteLine($"Ваш середній бал: {avg:F2}");
                Console.ReadLine();
            }
            else if (auth.CurrentUser.Role == UserRole.Student && choice == "3")
            {
                Console.WriteLine("Оформлюючи одну підписку, ви автоматично отримуєте ключі від" +
                    "\n усіх наших курсів — як старих, так і тих, \nщо вийдуть у майбутньому (поки діє підписка).");

                Console.WriteLine("Натисніть Enter...");
                Console.ReadLine();
            }
>>>>>>> Stashed changes
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            Console.ReadLine();
        }
    }
}