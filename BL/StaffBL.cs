using DAL;
using Model;
using System.Text;
using ConsolePL;
using System.Text.RegularExpressions;

namespace BL
{
    public class StaffBL
    {
        private StaffDAL staffDal = new StaffDAL();

        // đăng nhập user

        public Staff LoginUser()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Helper.Line(50);
            Console.WriteLine(
                @" ___       ________  ________  ___  ________      
|\  \     |\   __  \|\   ____\|\  \|\   ___  \    
\ \  \    \ \  \|\  \ \  \___|\ \  \ \  \\ \  \   
 \ \  \    \ \  \\\  \ \  \  __\ \  \ \  \\ \  \  
  \ \  \____\ \  \\\  \ \  \|\  \ \  \ \  \\ \  \ 
   \ \_______\ \_______\ \_______\ \__\ \__\\ \__\
    \|_______|\|_______|\|_______|\|__|\|__| \|__|
                                                     ");
            Helper.Line(50);
            Console.ResetColor();
            Console.Write("Username: ");
            string userName = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string password = GetPassword();
            Staff staff = new Staff() { userName = userName, password = password };
            staff = staffDal.Login(staff);
            if (staff.userName == null)
            {
                //Console.WriteLine("Welcome!!!");
                //WaitForButton("Press any key to continue...");
            }
            else
            {
                Console.WriteLine("Account does not exist!!!");
                if (IsContinue("Would you like to create a new account?? (Y/N): "))
                {
                    RegU(staff);
                }
                else
                {
                    return LoginUser();
                }
            }
            return staffDal.Login(staff);
        }
        static bool IsContinue(string text)
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


        // đăng kí user
        string GetPassword()
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
        void WaitForButton(string msg)
        {
            Console.Write(msg);
            Console.ReadKey();
        }

        public int RegU(Staff staff)
        {
            Console.Clear();
            int rs;
            string re_password;
            bool check;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Helper.Line(60);
            Console.WriteLine(@"
 ________  _______   ________  ___  ________  _________  _______   ________     
|\   __  \|\  ___ \ |\   ____\|\  \|\   ____\|\___   ___\\  ___ \ |\   __  \    
\ \  \|\  \ \   __/|\ \  \___|\ \  \ \  \___|\|___ \  \_\ \   __/|\ \  \|\  \   
 \ \   _  _\ \  \_|/_\ \  \  __\ \  \ \_____  \   \ \  \ \ \  \_|/_\ \   _  _\  
  \ \  \\  \\ \  \_|\ \ \  \|\  \ \  \|____|\  \   \ \  \ \ \  \_|\ \ \  \\  \| 
   \ \__\\ _\\ \_______\ \_______\ \__\____\_\  \   \ \__\ \ \_______\ \__\\ _\ 
    \|__|\|__|\|_______|\|_______|\|__|\_________\   \|__|  \|_______|\|__|\|__|
                                      \|_________|
                                      ");
            Helper.Line(60);
            Console.ResetColor();
            Console.Write("Your name        : ");
            staff.staffName = Console.ReadLine();
            staff.staffPhone = Helper.GetPhoneNumber();
            Console.Write("City             : ");
            staff.staffAddress = Console.ReadLine();
            staff.userName = Helper.GetNameAccount();
            Console.Write("Password         : ");
            staff.password = Helper.GetPass();
            Console.Write("Confirm Password : ");
            re_password = Helper.GetRe_Pass();
            check = Helper.isValidate(staff, re_password);
            rs = staffDal.RegUser(staff);
            if (check == true)
            {
                if (rs == 1)
                {
                    Console.WriteLine("Successful the new create!!");
                    WaitForButton("Press any key to continue...");

                }
                else
                {
                    Console.WriteLine("New creation failed!!! Phone number or Username already exists");
                    WaitForButton("Press any key to continue...");
                }
            }
            return rs;
        }
    }
}
