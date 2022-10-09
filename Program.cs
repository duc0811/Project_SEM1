using System.Text;
using System.Text.RegularExpressions;
using BL;
using ConsolePL;
using DAL;
using Model;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;
        StaffBL staffBl = new StaffBL();
        OrderBL orderBl = new OrderBL();
        ProductsBL productsBl = new ProductsBL();
        AdminBL adminBl = new AdminBL();
        OrderDAL orderDal = new OrderDAL();
        Helper var = new Helper();
        MainMenu();

// Menu main

        void MainMenu()
        {   
            string[] menu = { "Login", "Admin",  "Register", "Exit" };
            string name = 
@"
 ________  ___  ___  ________  _______   ________           ________  _________  ________  ________  _______      
|\   ____\|\  \|\  \|\   __  \|\  ___ \ |\   ____\         |\   ____\|\___   ___\\   __  \|\   __  \|\  ___ \     
\ \  \___|\ \  \\\  \ \  \|\  \ \   __/|\ \  \___|_        \ \  \___|\|___ \  \_\ \  \|\  \ \  \|\  \ \   __/|    
 \ \_____  \ \   __  \ \  \\\  \ \  \_|/_\ \_____  \        \ \_____  \   \ \  \ \ \  \\\  \ \   _  _\ \  \_|/__  
  \|____|\  \ \  \ \  \ \  \\\  \ \  \_|\ \|____|\  \        \|____|\  \   \ \  \ \ \  \\\  \ \  \\  \\ \  \_|\ \ 
    ____\_\  \ \__\ \__\ \_______\ \_______\____\_\  \         ____\_\  \   \ \__\ \ \_______\ \__\\ _\\ \_______\
   |\_________\|__|\|__|\|_______|\|_______|\_________\       |\_________\   \|__|  \|_______|\|__|\|__|\|_______|
   \|_________|                            \|_________|       \|_________|                           
   ";
            int choice;
            Staff staff = new Staff();
            AdminS admin = new AdminS();
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Helper.Line(114);
                Console.WriteLine(name);
                Helper.Line(114);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"           1. {menu[0],-10}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"                                 2. {menu[1]}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"                                                       3. {menu[2],-10} ");
                Console.ResetColor();
                Console.WriteLine($"                                                                                4. {menu[3]}");
                Helper.Line(114);
                Console.Write("Choose: ");
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        staffBl.LoginUser();
                        MenuStore(staff);
                        break;
                    case 2:
                        Console.Clear();                        
                        adminBl.LoginAdmin(admin);
                        AdminS();
                        break;
                    case 3:
                        Console.Clear();
                        staffBl.RegU(staff);
                        MainMenu();
                        break;
                    case 4:
                        if (Helper.IsContinue("Do you want to exit the program? (Y/N): "))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("App exited!");
                            Console.ResetColor();
                            Helper.WaitForButton("Press any key to continue...");
                            Console.Clear();
                            Helper.Line(114);
                            Console.WriteLine($"{"Thanks for watching",67}");
                            Helper.Line(114);
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }

 // Menu cho Admin

        void MenuAdmin(AdminS admin)
        {   
            Console.Clear();
            string[] menuA = { "Other function", "Revenue Check", "Log out" };
            string nameM = 
@" ________  ________  _____ ______   ___  ________      
                 |\   __  \|\   ___ \|\   _ \  _   \|\  \|\   ___  \    
                 \ \  \|\  \ \  \_|\ \ \  \\\__\ \  \ \  \ \  \\ \  \   
                  \ \   __  \ \  \ \\ \ \  \\|__| \  \ \  \ \  \\ \  \  
                   \ \  \ \  \ \  \_\\ \ \  \    \ \  \ \  \ \  \\ \  \ 
                    \ \__\ \__\ \_______\ \__\    \ \__\ \__\ \__\\ \__\
                     \|__|\|__|\|_______|\|__|     \|__|\|__|\|__| \|__| 
    ";
            int ch;
            do
            {
                ch = Helper.Menu(menuA, nameM);
                switch (ch)
                {
                    case 1:
                        Console.Clear();
                        MenuUp(admin);
                        break;
                    case 2:
                        Console.Clear();
                        orderBl.ShowAllB();
                        break;
                    case 3:
                        if (Helper.IsContinue("Do you want to log out? (Y/N): "))
                        {
                            Console.WriteLine("Signed out!");
                            Helper.WaitForButton("Press any key to continue...");
                        }
                        else
                        {
                            MenuAdmin(admin);
                        }
                        break;
                }
            } while (ch != menuA.Length);
        }
// Menu cập nhật cho admin

        void MenuUp(AdminS admin)
        {   
            Console.Clear();
            string[] menu = { "Add Product", "Show Product", "Delete Product", "Update Product", "Return" };
            Helper.Line(200);
            string name = 
@"      _______   ________  ___  _________   
                      |\  ___ \ |\   ___ \|\  \|\___   ___\ 
                      \ \   __/|\ \  \_|\ \ \  \|___ \  \_| 
                       \ \  \_|/_\ \  \ \\ \ \  \   \ \  \  
                        \ \  \_|\ \ \  \_\\ \ \  \   \ \  \ 
                         \ \_______\ \_______\ \__\   \ \__\
                          \|_______|\|_______|\|__|    \|__|
                                      ";
            Helper.Line(200);
            int ch;
            Product product = new Product();
            do
            {
                ch = Helper.Menu(menu, name);
                switch (ch)
                {
                    case 1:
                        Console.Clear();
                        productsBl.InPro(product);
                        break;
                    case 2:
                        Console.Clear();
                        string commandTextGetAllProduct = "select product.product_id, product.product_name, product.product_price, product.product_description, product.product_quantity, category.category_name from product inner join category on product.category_id = category.category_id;";
                        productsBl.GetAllProduct(commandTextGetAllProduct);
                        break;
                    case 3:
                        productsBl.deletePro(product);
                        break;
                    case 4:
                        Console.Clear();
                        productsBl.UpdatePro(product);
                        break;
                    case 5:
                        Console.Clear();
                        MenuAdmin(admin);
                        break;
                    default:
                        break;
                }

            } while (ch != menu.Length);
        }

// Menu đăng nhập cho admin

        void AdminS()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("===============================================================");
            Console.WriteLine(@"     ________  ________  _____ ______   ___  ________      
    |\   __  \|\   ___ \|\   _ \  _   \|\  \|\   ___  \    
    \ \  \|\  \ \  \_|\ \ \  \\\__\ \  \ \  \ \  \\ \  \   
     \ \   __  \ \  \ \\ \ \  \\|__| \  \ \  \ \  \\ \  \  
      \ \  \ \  \ \  \_\\ \ \  \    \ \  \ \  \ \  \\ \  \ 
       \ \__\ \__\ \_______\ \__\    \ \__\ \__\ \__\\ \__\
        \|__|\|__|\|_______|\|__|     \|__|\|__|\|__| \|__|
                                       ");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            Console.Write("Username: ");
            string user = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string passA = Helper.GetPassword();
            AdminS admin = new AdminS() { admin_User = user, admin_pass = passA };
            admin = adminBl.LoginAdmin(admin);
            if (admin.adminId == 1 || admin.adminId == 2)
            {
                MenuAdmin(admin);
            }
            else
            {
                Console.WriteLine("Login failed, please try again!");
                Helper.WaitForButton("Press any key to continue...");
            }
        }

// menu cho user

        void MenuStore(Staff staff)
        {
            string[] menu = { "List Product ", "Create a new order",  "Log out" };
            string name = @"    ___       __   _______   ___       ________  ________  _____ ______   _______      
   |\  \     |\  \|\  ___ \ |\  \     |\   ____\|\   __  \|\   _ \  _   \|\  ___ \     
   \ \  \    \ \  \ \   __/|\ \  \    \ \  \___|\ \  \|\  \ \  \\\__\ \  \ \   __/|    
    \ \  \  __\ \  \ \  \_|/_\ \  \    \ \  \    \ \  \\\  \ \  \\|__| \  \ \  \_|/__  
     \ \  \|\__\_\  \ \  \_|\ \ \  \____\ \  \____\ \  \\\  \ \  \    \ \  \ \  \_|\ \ 
      \ \____________\ \_______\ \_______\ \_______\ \_______\ \__\    \ \__\ \_______\
       \|____________|\|_______|\|_______|\|_______|\|_______|\|__|     \|__|\|_______|
                                                                                    ";
            int choice;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Helper.Line(91);
                Console.WriteLine(name);
                Helper.Line(91);
                Console.ResetColor();
                Console.WriteLine($" 1. {menu[0],-10}");
                Console.WriteLine($" 2. {menu[1]}");
                Console.WriteLine($" 3. {menu[2],-10} ");
                Helper.Line(91);
                Console.Write("Choose: ");
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        MenuSearchProduct();
                        break;

                    case 2:
                        Console.Clear();
                        orderBl.CreateNewOrder(staff);
                        // CreateNewOrder(staff);
                        break;
                    case 3:
                        if (Helper.IsContinue("Do you want to log out ? (Y/N): "))
                        {
                            Console.WriteLine("Signed out!");
                            Helper.WaitForButton("Press any key to continue...");
                        }
                        else
                        {
                            MenuStore(staff);
                        }
                        break;
                }
            } while (choice != menu.Length);
        }
        // tìm sản phẩm cho user

        void MenuSearchProduct()
        {   
            string[] menu = { "Show all product", "Search product by ID", "Return" };
            
            string name = @" ___       ___  ________  _________        ________  ________  ________  ________  ___  ___  ________ _________   
|\  \     |\  \|\   ____\|\___   ___\     |\   __  \|\   __  \|\   __  \|\   ___ \|\  \|\  \|\   ____\\___   ___\ 
\ \  \    \ \  \ \  \___|\|___ \  \_|     \ \  \|\  \ \  \|\  \ \  \|\  \ \  \_|\ \ \  \\\  \ \  \___\|___ \  \_| 
 \ \  \    \ \  \ \_____  \   \ \  \       \ \   ____\ \   _  _\ \  \\\  \ \  \ \\ \ \  \\\  \ \  \       \ \  \  
  \ \  \____\ \  \|____|\  \   \ \  \       \ \  \___|\ \  \\  \\ \  \\\  \ \  \_\\ \ \  \\\  \ \  \____   \ \  \ 
   \ \_______\ \__\____\_\  \   \ \__\       \ \__\    \ \__\\ _\\ \_______\ \_______\ \_______\ \_______\  \ \__\
    \|_______|\|__|\_________\   \|__|        \|__|     \|__|\|__|\|_______|\|_______|\|_______|\|_______|   \|__|
                  \|_________|                                                                                    
                                                                                                                  ";
            int choice;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Helper.Line(115);
                Console.WriteLine(name);
                Helper.Line(115);
                Console.ResetColor();
                Console.WriteLine($" 1. {menu[0],-10}");
                Console.WriteLine($" 2. {menu[1]}");
                Console.WriteLine($" 3. {menu[2],-10} ");
                Helper.Line(115);
                Console.Write("Choose: ");
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        string commandTextGetAllProduct = "select product.product_id, product.product_name, product.product_price, product.product_description, product.product_quantity, category.category_name from product inner join category on product.category_id = category.category_id;";
                        productsBl.GetAllProduct(commandTextGetAllProduct);
                        Helper.WaitForButton("Press any key to continue...");
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Input ID to search: ");
                        string id = Console.ReadLine() ?? "";
                        productsBl.SearchProductByID(id);
                        Helper.WaitForButton("Press any key to continue...");
                        break;
                    default:
                        break;
                }
            } while (choice != menu.Length);
        }

    }
}