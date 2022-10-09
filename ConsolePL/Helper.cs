using System.Text.RegularExpressions;
using System.Text;
using Model;
namespace ConsolePL
{
    public class Helper
    {
        public static int Menu(string[] menu, string name)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Helper.Line(90);
            Console.WriteLine($"                 {name,-5}               ");
            Helper.Line(90);
            Console.ResetColor();
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {menu[i]}");
            }
            Helper.Line(90);
            int choice;
            do
            {
                Console.Write("Choose: ");
                int.TryParse(Console.ReadLine(), out choice);
            } while (choice < 1 || choice > menu.Length);
            return choice;
        }
        public static void Line(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine();
        }
        public static void Line1(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine();
        }
        public static void WaitForButton(string msg)
        {
            Console.Write(msg);
            Console.ReadKey();
        }
        public static string FormatCurrency(string currency)
        {
            for (int k = currency.Length - 3; k > 0; k = k - 3)
            {
                currency = currency.Insert(k, ".");
            }
            return currency;
        }


        public static bool IsContinue(string text)
        {
            string Continue;
            bool isMatch;
            Console.Write(text);
            Continue = Console.ReadLine() ?? "";
            isMatch = Regex.IsMatch(Continue, @"^[yYnN]$");
            while (!isMatch)
            {
                Console.Write(" Choose (Y/N)!!!: ");
                Continue = Console.ReadLine() ?? "";
                isMatch = Regex.IsMatch(Continue, @"^[yYnN]$");
            }
            if (Continue == "y" || Continue == "Y") return true;
            return false;
        }

        public static string GetPassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        sb.Length--;
                    }
                    continue;
                }
                Console.Write('*');

                sb.Append(cki.KeyChar);
            }
            return sb.ToString();
        }
        public static string GetNameAccount()
        {
            bool validate = false;
            string output = "";
            string pattern = @"^([^<>\s\W]){3,15}$";
            while (validate == false)
            {
                Console.Write("New Account      : ");
                output = Console.ReadLine() ?? "";
                validate = Regex.IsMatch(output, pattern);

                if (validate == false)
                {
                    Console.WriteLine("There are spaces or not enough characters!!");
                    Console.ReadKey();
                }
                else
                {
                }
            }
            return output;
        }
         public static string GetPass()
        {
            bool validate = false;
            string output = "";
            string pattern = @"^([^<>\s\W]){5,30}$";
            while (validate == false)
            {
                output = GetPassword();
                validate = Regex.IsMatch(output, pattern);

                if (validate == false)
                {
                    Console.WriteLine("There are spaces or not enough characters!!");
                    Console.Write("Password         : ");
                }
                else
                {
                }
            }
            return output;
        }
      public static  string GetRe_Pass()
        {
            bool validate = false;
            string output = "";
            string pattern = @"^([^<>\s\W]){5,30}$";
            while (validate == false)
            {
                output = GetPassword();
                validate = Regex.IsMatch(output, pattern);

                if (validate == false)
                {
                    Console.WriteLine("There are spaces or not enough characters!!");
                    Console.Write("Confirm Password : ");
                }
                else
                {
                }
            }
            return output;
        }
      public static  string GetPhoneNumber()
        {
            bool validate = false;
            string output = "";
            string pattern = @"^([^\s\D]{10})$";
            while (validate == false)
            {
                Console.Write("Phone            : ");
                output = Console.ReadLine() ?? "";
                validate = Regex.IsMatch(output, pattern);

                if (validate == false)
                {
                    Console.WriteLine("There are spaces or not enough 10 numbers!!");
                }
                else
                {
                }
            }
            return output;
        }

        public static string GetMoney()
        {
            bool validate = false;
            string output = "";
            string pattern = @"^([^\s\D]{4,9})$";
            while (validate == false)
            {
                Console.Write(" Amount paid by customer: ");
                output = Console.ReadLine() ?? "";
                validate = Regex.IsMatch(output, pattern);

                if (validate == false)
                {
                    Console.WriteLine("The amount cannot be more 100.000.000 !!!");
                }
                else
                {
                }
            }
            return output;
        }
        public static bool isValidate(Staff user, string re_password)
        {
            if (user.password == re_password)
            {
                return true;
            }
            return false;


        }
    }
}