using System.Drawing;
using System.Globalization;

namespace Lecturers_work;
    public class Program
    {
        
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
        }
    }