using System.Drawing;
using System.Globalization;

namespace Lecturers_work;
    public class Program
    {
        private static string name = @"
 __      __          _____ 
 \ \    / /         / ____|                                          
  \ \  / /  __  __ | |        ___    _   _   _ __   ___    ___   ___ 
   \ \/ /   \ \/ / | |       / _ \  | | | | | '__| / __|  / _ \ / __|
    \  /     >  <  | |____  | (_) | | |_| | | |    \__ \ |  __/ \__ \
     \/     /_/\_\  \_____|  \___/   \__,_| |_|    |___/  \___| |___/";
        
        private static string[] courses { get; } = new string[6] { "Математика (Мельник Олександр Іванович)", "Українська мова (Шевченко Тетяна Петрівна)",
            "Українська література (Коваленко Андрій Васильович)", "Історія України (Бондаренко Наталія Миколаївна)",
            "Фізика (Ткаченко Сергій Олександрович)", "Географія (Ковальчук Ірина Дмитрівна)" };

        private static int[] prises { get; } = new int[6] {849, 549, 649, 1049, 1495, 789};

        private static double discount;
        
        public static void Main(string[] args)
        {
            ShopName();
            ShowMenu();
        }

        private static void ShopName()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(name +"\n");
            Console.ResetColor();
        }

        private static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Меню:\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. Переглянути список курсів");
            Console.WriteLine("2. Розрахувати вартість навчання");
            Console.WriteLine("3. Інформація про навчальний центр");
            Console.WriteLine("4. Налаштування");
            Console.WriteLine("0. Вихід\n");
            Console.ResetColor();

            int choice = Choice();

            switch (choice)
            {
                case 1:
                    ShowCursesInfo();
                    break;
                case 2:
                    BuyCourses();
                    break;
                case 3:
                    Info();
                    break;
                case 4:
                    Settings();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nТакого пункту не існує.\n");
                    Console.ResetColor();
                    ShowMenu();
                    break;
            }
            
        }
        private static void ShowCursesInfo()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Список курсів: \n");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < courses.Length; i++) 
            {
                Console.WriteLine($"° {courses[i]} - {prises[i]}");
            }
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Для запуску меню натисніть на будь-яку клавішу.");
            Console.ResetColor();
            Console.ReadKey();
            Console.WriteLine();
            ShowMenu();
        }
        
        private static void BuyCourses()
        {
            List<int> coursesBuy = new List<int>();
            List<int> mounth = new List<int>();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Максимальна тривалість навчання 12 місяців.");
            Console.WriteLine("Оберіть бажану тривалість навчання (місяців): \n");
            Console.ResetColor();
            
            for (int i = 0 ; i < courses.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{courses[i]} - {prises[i]}: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                int m = 0;

                try
                {
                    m = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nНеправельний формат вводу!\n");
                    BuyCourses();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                Console.ResetColor();
                if (m > 12) m = 12;
                if (m < 1) continue;
                
                coursesBuy.Add(i);
                mounth.Add(m);
            }
            
            int allMounth = mounth.Sum();

            switch (allMounth)
            {
                case > 70:
                    discount = 25;
                    break;
                case > 50:
                    discount = 20;
                    break;
                case > 30:
                    discount = 15;
                    break;
                case > 15:
                    discount = 10;
                    break;
                case > 10:
                    discount = 7;
                    break;
                case > 5:
                    discount = 5;
                    break;
                default:
                    discount = 0;
                    break;
            }
            

            int result = 0;
            
            for (int i = 0; i < coursesBuy.Count; i++) result += prises[coursesBuy[i]] * mounth[i];
            
            if (result == 0)
            {
                Console.WriteLine("Ви не вибрали жодного курсу!");
                Console.WriteLine("1. Спробувати знову.");
                Console.WriteLine("2. Вихід в меню.");
                
                int choice = Choice();

                switch (choice)
                {
                    case 1:
                        BuyCourses();
                        break;
                    case 2:
                        ShowMenu();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Такого пункту не існує. Запуск меню.");
                        Console.ResetColor();
                        ShowMenu();
                        break;
                }
            }
            else
            {
                double totalDiscount = Math.Round((discount / 100) * result, 2);
                double finishPrice = result - totalDiscount;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Загальна сума без знижки: {result}");
                Console.WriteLine($"Знижка у відсотках: {discount}");
                Console.WriteLine($"Знижка у грн: {totalDiscount}\n");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"Сума зі знижкою: {finishPrice}");

                StartMenu();
            }
        }

        private static void Info()
        {
            ShopName();
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("VxCourses — це сучасна онлайн-платформа навчання, створена для тих,\nхто хоче швидко опанувати затребувані ІТ-професії та технічні навички." +
                              "\nНаш центр поєднує практичний підхід, досвід викладачів-практиків і зручний формат навчання.\n");
            Console.ResetColor();
            
            StartMenu();
        }

        private static int Choice()
        {
            int choice = 0;
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Виберіть пункт з меню: ");
            Console.ResetColor();
            
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
            }
            catch (FormatException)
            { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nПомилка: введено некоректне число\n");
                Console.ResetColor();
                ShowMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return choice;
        }
        
        private static void Settings()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("В розробці.");
            Console.ResetColor();
            
            StartMenu();
        }

        private static void StartMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Для запуску меню натисніть на будь-яку клавішу.");
            Console.ResetColor();
            Console.ReadKey();
            Console.WriteLine();
            ShowMenu();
        }
    }