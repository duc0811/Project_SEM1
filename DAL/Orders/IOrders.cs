using Model;

namespace DAL
{
    public interface IOrders
    {
        public bool CreateOrder(Orders order);
        public List<Orders> GetAllPaidOrdersInDay();
         
    }
}