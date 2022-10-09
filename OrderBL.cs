using Model;
using DAL;
using ConsolePL;
using System.Text.RegularExpressions;

namespace BL
{
    public class OrderBL
    {
        private OrderDAL orderDal = new OrderDAL();
        private ProductsBL productsBl = new ProductsBL();

        public void CreateNewOrder(Staff staff)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================");
            Console.WriteLine("|                       Create invoice !                      |");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            Orders order = new Orders();
            order.orderStaff = staff;
            do
            {
                Console.Write("Input product ID to add to the invoice:");
                string id = Console.ReadLine() ?? "";
                Product product = productsBl.SearchProductByID(id);
                if (product.productId == 0)
                {
                    continue;
                }
                else
                {
                    string strQuantity;
                    bool isSuccess;
                    int quantity;
                    Console.Write("Enter the quantity you want to buy: ");
                    strQuantity = Console.ReadLine() ?? "";
                    isSuccess = int.TryParse(strQuantity, out quantity);
                    while (!isSuccess)
                    {
                        Console.Write("Invalid quantity! Re-enter quantity: ");
                        strQuantity = Console.ReadLine() ?? "";
                        isSuccess = int.TryParse(strQuantity, out quantity);
                    }
                    if (quantity <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Add failed !!");
                        Console.ResetColor();
                        Console.WriteLine("This product is out of stock!");
                        continue;
                    }
                    if (quantity <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Add failed !!");
                        Console.ResetColor();
                        Console.WriteLine("Invalid number entered!");
                        continue;
                    }
                    if (quantity > product.productQuantity)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Add failed !!");
                        Console.ResetColor();
                        Console.WriteLine("Purchase quantity exceeds available quantity!");
                        continue;
                    }
                    decimal amount = quantity * product.productPrice;
                    product.productQuantity = quantity;
                    product.productAmount = amount;
                    bool add = true;
                    if (order.productsList == null || order.productsList.Count == 0)
                    {
                        order.productsList!.Add(product);
                    }
                    else
                    {
                        for (int n = 0; n < order.productsList.Count; n++)
                        {
                            if (int.Parse(id) == order.productsList[n].productId)
                            {
                                order.productsList[n].productQuantity += quantity;
                                order.productsList[n].productAmount += amount;
                                add = false;
                            }
                        }
                        if (add) order.productsList.Add(product);
                    }
                }
            } while (Helper.IsContinue("Would you like to add another product to your invoice? (Y/N): "));

            if (order.productsList == null || order.productsList.Count == 0)
                Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("Invoice has no products!");
            Console.ResetColor();

            if (orderDal.CreateOrder(order))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Create new invoice successfully!");
                Console.ResetColor();
                Helper.WaitForButton("Enter any key to view invoice...");
                Console.Clear();
                string price, amount;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Helper.Line(96);
                Console.WriteLine("|                                       Sales Invoice                                          |");
                Helper.Line(96);
                Console.ResetColor();
                Console.WriteLine($"| Date/time: {order.orderDate,-61}     Order ID: {order.orderId,5} |");
                Console.WriteLine($"|                                                                              Địa chỉ: Hà Nội |");
                Console.WriteLine($"| {"Name Product",-62}|{"Price Each",10}|{"Quantity",8}|  {"Amount"}  |");
                Helper.Line(96);
                foreach (Product product in order.productsList!)
                {
                    price = Helper.FormatCurrency(product.productPrice.ToString());
                    amount = Helper.FormatCurrency(product.productAmount.ToString());
                    Console.WriteLine($"| {product.productName,-62}|{price,10}|{product.productQuantity,8}|{amount,10}|");
                    order.total += product.productAmount;
                }
                string total = Helper.FormatCurrency(order.total.ToString());
                Console.ForegroundColor = ConsoleColor.Green;
                Helper.Line(96);
                Console.WriteLine($"| TOTAL PAYMENT AMOUNT {total,67} VND |");
                Helper.Line(96);
                Console.WriteLine($"| Customer Name: {order.orderCustomer!.customer_Name,-49}                             |");
                Helper.Line(96);
                Console.ResetColor();
                Payment payment = new Payment();
                string paymentAmount;
                string refund;
                while (true)
                {

                    paymentAmount = Helper.GetMoney();
                    if (Convert.ToDecimal(paymentAmount) >= order.total)
                    {
                        payment.paymentAmount = Convert.ToDecimal(paymentAmount);
                        paymentAmount = Helper.FormatCurrency(payment.paymentAmount.ToString());

                        payment.refund = payment.paymentAmount - order.total;
                        refund = Helper.FormatCurrency(payment.refund.ToString());
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The amount you entered is incorrect or not enough ! Please re-enter!");
                    }
                }
                Helper.Line(96);
                Console.WriteLine($"| + Total         : {total,70} VND |");
                Console.WriteLine($"| + Amount Paid   : {paymentAmount,70} VND |");
                Console.WriteLine($"| + Return Amount : {refund,70} VND |");
                Helper.Line(96);
            }
            else
            {
                Console.WriteLine("Invoice creation failed!");
            }
            Helper.WaitForButton("Press any key to continue...");

        }
        public void ShowAllB()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            OrderDAL orderDAL = new OrderDAL();
            List<Orders> ordersList = new List<Orders>();
            ordersList = orderDAL.GetAllPaidOrdersInDay();
            if (ordersList == null || ordersList.Count == 0)
            {
                Console.WriteLine("Order not exists!!");
                return;
            }
            else
            {
                int size = 10;
                int page = 1;
                int pages = (int)Math.Ceiling((double)ordersList.Count / size);
                int k = 0;
                string choose;
                while (true)
                {
                    {
                        Helper.Line(94);
                        Console.Clear();

                        Console.WriteLine(@"     
  ________  ___       ___               ________  ________  ________  _______   ________              
 |\   __  \|\  \     |\  \             |\   __  \|\   __  \|\   ___ \|\  ___ \ |\   __  \    
 \ \  \|\  \ \  \    \ \  \            \ \  \|\  \ \  \|\  \ \  \_|\ \ \   __/|\ \  \|\  \   
  \ \   __  \ \  \    \ \  \            \ \  \\\  \ \   _  _\ \  \ \\ \ \  \_|/_\ \   _  _\  
   \ \  \ \  \ \  \____\ \  \____        \ \  \\\  \ \  \\  \\ \  \_\\ \ \  \_|\ \ \  \\  \| 
    \ \__\ \__\ \_______\ \_______\       \ \_______\ \__\\ _\\ \_______\ \_______\ \__\\ _\ 
     \|__|\|__|\|_______|\|_______|        \|_______|\|__|\|__|\|_______|\|_______|\|__|\|__|");
                        Console.WriteLine();
                        Helper.Line(94);
                        Console.WriteLine("| Order id |     Customer name         |     Date / Time        | Total      | Status        |");
                        Console.WriteLine("| -------- |     -------------         |     -----------        | -----      | ------        |");
                        if (ordersList.Count < size)
                        {
                            for (int i = 0; i < ordersList.Count; i++)
                            {
                                Console.WriteLine($"| {ordersList[i].orderId,-8} | {ordersList[i].orderCustomer!.customer_Name,-25} | {ordersList[i].orderDate,-22} | {ordersList[i].total.ToString(),-10} | {"Đã thanh toán",-13} |");
                            }
                            Helper.Line(94);
                            Console.WriteLine($"|                                                                                   Page {page}/{pages} |");
                        }
                        else
                        {
                            for (int i = ((page - 1)) * size; i < k + 10; i++)
                            {
                                if (i == ordersList.Count) break;
                                Console.WriteLine($"| {ordersList[i].orderId,-8} | {ordersList[i].orderCustomer!.customer_Name,-25} | {ordersList[i].orderDate,-22} | {ordersList[i].total.ToString(),-10} | {"Đã thanh toán",-13} |");
                            }
                            Helper.Line(94);
                            Console.WriteLine($"|                                                                                   Page {page}/{pages} |");
                        }
                    }
                    Helper.Line(94);
                    Console.WriteLine("Enter [P] to previous page and [N] to next page and [number 0] to return!!!");
                    Helper.Line(94);
                    Console.Write("Choose: ");
                    choose = Console.ReadLine() ?? "";
                    while (true)
                    {
                        if (Regex.Match(choose, @"([PpNn]|[1-9]|^0$)").Success)
                        {
                            break;
                        }
                        else
                        {
                            Console.Write("Invalid selection! Re-select: ");
                            choose = Console.ReadLine() ?? "";
                        }
                    }
                    choose = choose.Trim();
                    choose = choose.ToLower();
                    string number = Regex.Match(choose, @"\d+").Value;
                    if (choose == "n")
                    {
                        if (page == pages)
                        {
                            Helper.WaitForButton("No next page! Enter any key to continue...");
                        }
                        else
                        {
                            page = page + 1;
                            k = k + 10;
                        }
                    }
                    else if (choose == "p")
                    {
                        if (page == 1)
                        {
                            Helper.WaitForButton("No previous page! Enter any key to continue...");
                        }
                        else
                        {
                            page = page - 1;
                            k = k - 10;
                        }
                    }
                    else if (choose == "0")
                    {
                        return;

                    }
                }
            }
        }
    }
}