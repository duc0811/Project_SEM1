using Model;
using System.Text.RegularExpressions;
using DAL;
using ConsolePL;
namespace BL
{
    public class ProductsBL
    {   
        private ProductsDAL productDal = new ProductsDAL();
     
        public Product SearchProductByID(string searchKeyWord)
        {
            Product product = new Product();
            product = productDal.GetProductById(searchKeyWord, product);
            string search = '"' + searchKeyWord + '"';
            if (product.productId <= 0)
            {
                Console.WriteLine($"Not found product with ID  {search}");
            }
            else
            {
                ShowProductDatail(product, searchKeyWord);
            }
            return product;
        }

        // IN TẤT CẢ DANH SÁCH BÁNH CHO USER
        public void GetAllProduct(string commandText)
        {
            List<Product> list = new List<Product>();
            list = productDal.GetProductList(list, commandText);
            if (list.Count == 0)
            {
                Console.WriteLine("Product not available!");
                Helper.WaitForButton("Press any key to continue...");
            }
            else
            {
                int size = 10;
                int page = 1;
                int pages = (int)Math.Ceiling((double)list.Count / size);
                int i, k = 0;
                string choice, price;
                for (; ; )
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("================================================================================================================================");
                    Console.WriteLine(@"|         ________  ___       ___               ________  ________  ________  ________  ___  ___  ________ _________           |");
                    Console.WriteLine(@"|        |\   __  \|\  \     |\  \             |\   __  \|\   __  \|\   __  \|\   ___ \|\  \|\  \|\   ____\\___   ___\         |");
                    Console.WriteLine(@"|        \ \  \|\  \ \  \    \ \  \            \ \  \|\  \ \  \|\  \ \  \|\  \ \  \_|\ \ \  \\\  \ \  \___\|___ \  \_|         |");
                    Console.WriteLine(@"|         \ \   __  \ \  \    \ \  \            \ \   ____\ \   _  _\ \  \\\  \ \  \ \\ \ \  \\\  \ \  \       \ \  \          |");
                    Console.WriteLine(@"|          \ \  \ \  \ \  \____\ \  \____        \ \  \___|\ \  \\  \\ \  \\\  \ \  \_\\ \ \  \\\  \ \  \____   \ \  \         |");
                    Console.WriteLine(@"|           \ \__\ \__\ \_______\ \_______\       \ \__\    \ \__\\ _\\ \_______\ \_______\ \_______\ \_______\  \ \__\        |");
                    Console.WriteLine(@"|            \|__|\|__|\|_______|\|_______|        \|__|     \|__|\|__|\|_______|\|_______|\|_______|\|_______|   \|__|        |");
                    Console.WriteLine("|                                                                                                                              |");
                    Console.WriteLine("================================================================================================================================");
                    Console.WriteLine("| ID      | Name                                                                 | Price         | Category (ID)               |");
                    if (list.Count < size)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            price = Helper.FormatCurrency(list[i].productPrice.ToString());
                            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine($"| {list[i].productId,-7} | {list[i].productName,-68} | {price,-13} | {list[i].productCategory,-27} |");
                        }
                    }
                    else
                    {
                        for (i = ((page - 1)) * size; i < k + 10; i++)
                        {
                            if (i == list.Count) break;
                            price = Helper.FormatCurrency(list[i].productPrice.ToString());
                            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine($"| {list[i].productId,-7} | {list[i].productName,-68} | {price,-13} | {list[i].productCategory,-27} |");
                        }
                    } 
                    Console.WriteLine("================================================================================================================================");
                    Console.WriteLine($"|                                                                                                                     Page {page}/{pages} |");
                    Console.WriteLine("================================================================================================================================");
                    Console.WriteLine("|Enter [P] to previous page and [N] to next page and [number 0] to return!!!                                                   |");
                    Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                    Console.Write("Choose: ");
                    choice = Console.ReadLine() ?? "";
                    while (!(Regex.IsMatch(choice, @"([PpNn]|[1-9]|^0$)")))
                    {
                        Console.Write("Invalid selection! Re-select: ");
                        choice = Console.ReadLine() ?? "";
                    }
                    choice = choice.Trim();
                    choice = choice.ToLower();
                    string number = Regex.Match(choice, @"\d+").Value;
                    string pageNum = "p" + number;
                    if (choice == "n")
                    {
                        if (page == pages)
                        {
                            Helper.WaitForButton("No back page! Press any key to continue...");
                        }
                        else
                        {
                            page = page + 1;
                            k = k + 10;
                        }
                    }
                    else if (choice == "p")
                    {
                        if (page == 1)
                        {
                            Helper.WaitForButton("No previous page! Press any key to continue...");
                        }
                        else
                        {
                            page = page - 1;
                            k = k - 10;
                        }
                    }
                    else if (choice == pageNum)
                    {
                        if (int.Parse(number) < 0 || int.Parse(number) > pages || int.Parse(number) == 0)
                        {
                            Console.WriteLine($"Not exists page {int.Parse(number)}");
                            Helper.WaitForButton("Press any key to continue...");
                        }
                        else
                        {
                            page = int.Parse(number);
                            k = (int.Parse(number) - 1) * 10;
                        }
                    }
                    else if (choice == "0") return;
                    else
                    {
                        bool found = false;
                        string search1 = '"' + choice + '"';
                        for (i = ((page - 1)) * size; i < k + 10; i++)
                        {
                            try
                            {
                                if (int.Parse(choice) == list[i].productId)
                                {   
                                    //ShowProductDatail(product, searchKeyWord)
                                    ShowProductDatail(list[i], search1);
                                    Helper.WaitForButton("Press any key to continue...");
                                    found = true;
                                    break;
                                }
                            }
                            catch (System.FormatException) { }
                            catch (System.ArgumentOutOfRangeException) { }
                        }
                        if (!(found))
                        {
                            Console.WriteLine("Not found ID !");
                            Helper.WaitForButton("Press any key to continue...");
                        }
                    }
                }
            }
        }

        
      
 // IN THÔNG TIN CHI TIẾT
        private void ShowProductDatail(Product product, string search)

        {

            Console.Clear();
            string price = Helper.FormatCurrency(product.productPrice.ToString());
            Console.ForegroundColor = ConsoleColor.Green;
            Helper.Line(75);
            Console.WriteLine($"|                   Product Detail information have ID: {search,-17} |");
            Helper.Line(75);
            Console.WriteLine($"| ID         :       | {product.productId,-50} |");
            Console.WriteLine("|--------------------|----------------------------------------------------|");
            Console.WriteLine($"| Name       :       | {product.productName,-50} |");
            Console.WriteLine("|--------------------|----------------------------------------------------|");
            Console.WriteLine($"| Price      :       | {product.productPrice,-50} |");
            Console.WriteLine("|--------------------|----------------------------------------------------|");
            Console.WriteLine($"| Category   :       | {product.productCategory,-50} |");
            Console.WriteLine("|--------------------|----------------------------------------------------|");
            Console.WriteLine($"| Quantity   :       | {product.productQuantity,-50} |");
            Console.WriteLine("|--------------------|----------------------------------------------------|");
            Console.Write("| Description:       |");
            string str = ' ' + product.productDescription;
            string subStr;
            int i = 65;
            try
            {
                while (str.Length > 0 && i < str.Length)
                {
                    i = 26;
                    while (str[i] != ' ')
                    {
                        i = i + 1;
                        if (i >= str.Length) break;
                    }
                    subStr = str.Substring(1, i);
                    Console.WriteLine($" {subStr,50} |");
                    str = str.Remove(0, i);
                }
            }
            catch (System.ArgumentOutOfRangeException) { }
            finally
            {
                Console.WriteLine($" {str.Remove(0, 1),-50} |");
                Helper.Line(75);
                Console.ResetColor();
            }
        }

        public int UpdatePro(Product product)
        {
            Console.Clear();
            int rs;
            Console.ForegroundColor = ConsoleColor.Green;
            Helper.Line(65);
            Console.WriteLine(
                @" ___  ___  ________  ________  ________  _________  _______      
|\  \|\  \|\   __  \|\   ___ \|\   __  \|\___   ___\\  ___ \     
\ \  \\\  \ \  \|\  \ \  \_|\ \ \  \|\  \|___ \  \_\ \   __/|    
 \ \  \\\  \ \   ____\ \  \ \\ \ \   __  \   \ \  \ \ \  \_|/__  
  \ \  \\\  \ \  \___|\ \  \_\\ \ \  \ \  \   \ \  \ \ \  \_|\ \ 
   \ \_______\ \__\    \ \_______\ \__\ \__\   \ \__\ \ \_______\
    \|_______|\|__|     \|_______|\|__|\|__|    \|__|  \|_______| 
    ");
            Helper.Line(65);
            Console.ResetColor();
            Console.Write("Input ID   : ");
            product.productId = Convert.ToInt32(Console.ReadLine());
            Console.Write("New name   : ");
            product.productName = Console.ReadLine();
            Console.Write("Category   : ");
            product.productCategory = Console.ReadLine();
            Console.Write("Description: ");
            product.productDescription = Console.ReadLine();
            Console.Write("Price      : ");
            product.productPrice = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Quantity   : ");
            product.productAmount = Convert.ToDecimal(Console.ReadLine());
            rs = productDal.Update(product);
            if (rs == 1)
            {

                Console.WriteLine("Update Successful!!");
                Helper.WaitForButton("Press any key to continue...");
            }
            else
            {
                Console.WriteLine("Not found ID !!");
            }
            return rs;
        }

        public int deletePro(Product product)
        {
            Console.Clear();
            int rs;
            Console.ForegroundColor = ConsoleColor.Green;
            Helper.Line(64);
            Console.WriteLine(
@" ________  _______   ___       _______  _________  _______      
|\   ___ \|\  ___ \ |\  \     |\  ___ \|\___   ___\\  ___ \     
\ \  \_|\ \ \   __/|\ \  \    \ \   __/\|___ \  \_\ \   __/|    
 \ \  \ \\ \ \  \_|/_\ \  \    \ \  \_|/__  \ \  \ \ \  \_|/__  
  \ \  \_\\ \ \  \_|\ \ \  \____\ \  \_|\ \  \ \  \ \ \  \_|\ \ 
   \ \_______\ \_______\ \_______\ \_______\  \ \__\ \ \_______\
    \|_______|\|_______|\|_______|\|_______|   \|__|  \|_______|
                                                               ");
            Helper.Line(64);
            Console.ResetColor();
            Console.Write("Enter the product id you want to delete : ");
            product.productId = Convert.ToInt32(Console.ReadLine());
            rs = productDal.Delete(product);
            if (rs == 1)
            {
                Helper.Line(50);
                Console.WriteLine($"Delete successful product have ID: {product.productId}");
                Helper.WaitForButton("Press any key to continue...");
            }
            else
            {
                Helper.Line(50);
                Console.WriteLine($"Not found product with ID: {product.productId} ");
                Helper.WaitForButton("Press any key to continue...");

            }
            return rs;

        }

        public int InPro(Product product)
        {
            int result;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Helper.Line(59);
            Console.WriteLine(
                @" ___  ________   ________  _______   ________  _________   
|\  \|\   ___  \|\   ____\|\  ___ \ |\   __  \|\___   ___\ 
\ \  \ \  \\ \  \ \  \___|\ \   __/|\ \  \|\  \|___ \  \_| 
 \ \  \ \  \\ \  \ \_____  \ \  \_|/_\ \   _  _\   \ \  \  
  \ \  \ \  \\ \  \|____|\  \ \  \_|\ \ \  \\  \|   \ \  \ 
   \ \__\ \__\\ \__\____\_\  \ \_______\ \__\\ _\    \ \__\
    \|__|\|__| \|__|\_________\|_______|\|__|\|__|    \|__|
                   \|_________|                            
                   ");
            Helper.Line(59);
            Console.ResetColor();
            Console.Write("Category Product    : ");
            product.productCategory = Console.ReadLine();
            Console.Write("Name Product        : ");
            product.productName = Console.ReadLine();
            Console.Write("Price               : ");
            product.productPrice = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Product Description : ");
            product.productDescription = Console.ReadLine();
            Console.Write("Quantity            : ");
            product.productAmount = Convert.ToInt32(Console.ReadLine());
            result = productDal.InS(product);
            if (result == 1)
            {
                Console.WriteLine("Add new successful!!!");
                Helper.WaitForButton("Press any key to continue...");

            }
            else
            {
                Console.WriteLine("Add failed!! Product already exists");
                Helper.WaitForButton("Press any key to continue...");
            }
            return result;


        }

        public static void ShowProductDataill(Product product, string search)
        {
            Console.Clear();
            string price = (product.productPrice.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"             Product Detail information have ID: {search,-30} ");
            Helper.Line(64);
            Console.WriteLine($"| ID        :        | {product.productId,-39} |");
            Console.WriteLine("|--------------------|-----------------------------------------|");
            Console.WriteLine($"| Name      :        | {product.productName,-39} |");
            Console.WriteLine("|--------------------|-----------------------------------------|");
            Console.WriteLine($"| Price     :        | {product.productPrice,-39} |");
            Console.WriteLine("|--------------------|-----------------------------------------|");
            Console.WriteLine($"| Category  :        | {product.productCategory,-39} |");
            Console.WriteLine("|--------------------|-----------------------------------------|");
            Console.WriteLine($"| Quantity  :        | {product.productQuantity,-39} |");
            Console.WriteLine("|--------------------|-----------------------------------------|");
            Console.Write("| Description:       |");
            string str = ' ' + product.productDescription;
            string subStr;
            int i = 65;
            try
            {
                while (str.Length > 0 && i < str.Length)
                {
                    i = 65;
                    while (str[i] != ' ')
                    {
                        i = i + 1;
                        if (i >= str.Length)
                        {
                            break;
                        }
                    }
                    subStr = str.Substring(1, i);
                    Console.WriteLine($" {subStr,-39} |");
                    Console.Write("|                    |");
                    str = str.Remove(0, i);
                }
            }
            catch (System.ArgumentOutOfRangeException) { }
            finally
            {
                Console.WriteLine($" {str.Remove(0, 1),-39} |");
                Console.WriteLine("================================================================");
            }

        }
    }
}
