
namespace Model
{
    
    public class Orders
    {
        public int orderId { get; set; }
        // public int UserId { get; set; }

        public Customer? orderCustomer { get; set; }
        // public Product? orderC { get; set; }
        public Staff? orderStaff { get; set; }
        public DateTime orderDate { get; set; }
        // public int orderStatus { get; set; }
        public List<Product> productsList { get; set; }
        public decimal total { get; set; }
        public Orders()
        {
            this.productsList = new List<Product>();
        }
    }
    public class Payment
    {
        public decimal paymentAmount { get; set; }
        public decimal refund { get; set; }
    }
}