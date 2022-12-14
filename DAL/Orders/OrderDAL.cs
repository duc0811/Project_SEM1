using Model;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class OrderDAL : IOrders
    {
        private MySqlDataReader? reader;
        private MySqlConnection connection = DbConfig.GetConnection();


        public bool CreateOrder(Orders order)
        {
            if (order == null || order.productsList == null || order.productsList.Count == 0)
            {
                return false;
            }
            bool result = false;
            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"lock tables staff write, product write, category write, customer write, order_details write, orders write";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                bool check = false;
                if (order.orderCustomer == null || order.orderCustomer.customer_Name == null || order.orderCustomer.customer_Name == "")
                {
                    order.orderCustomer = new Customer() { customer_Id = 1 };
                }
                try
                {
                    if (!check)
                    {

                        Console.Write("Enter customer name: ");
                        string cusName = Console.ReadLine() ?? "";
                        order.orderCustomer = new Customer { customer_Name = cusName };
                        cmd.CommandText = $"insert into customer(customer_name, customer_phone) values ('{order.orderCustomer.customer_Name}', '{order.orderCustomer.customer_Phone}');";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "select LAST_INSERT_ID() as customer_id;";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            order.orderCustomer.customer_Id = reader.GetInt32("customer_id");
                        }
                        reader.Close();
                    }
                    /*'{order.orderCustomer!.customer_Id}'*/
                    cmd.CommandText = $"SET  FOREIGN_KEY_CHECKS=0 ; insert into orders(customer_id, staff_id, order_status) values ('{order.orderCustomer!.customer_Id}','{order.orderStaff!.staffId}', {OrderSTT.UNPAID});";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "select LAST_INSERT_ID() as order_id;";
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        order.orderId = reader.GetInt32("order_id");
                    }
                    reader.Close();

                    cmd.CommandText = "SELECT order_date FROM orders ORDER BY order_id DESC LIMIT 1;";
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        order.orderDate = reader.GetDateTime("order_date");
                    }
                    reader.Close();

                    foreach (Product product in order.productsList)
                    {
                        cmd.CommandText = $"select product_price from product where product_id={product.productId};";
                        reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            throw new Exception("Not exists!!");
                        }
                        product.productPrice = reader.GetDecimal("product_price");
                        reader.Close();

                        cmd.CommandText = $"insert into order_details(order_id, product_id, unit_price, quantity) values ({order.orderId}, {product.productId}, {product.productPrice * product.productQuantity}, {product.productQuantity});";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"update product set product_quantity = product_quantity-{product.productQuantity} where product_id={product.productId}; SET  FOREIGN_KEY_CHECKS=1;";
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    result = true;
                }
                catch
                {
                    Console.WriteLine("Disconnected Database!!");
                    try
                    {
                        trans.Rollback();
                    }
                    catch
                    {
                        Console.WriteLine("Disconnected database!!");
                    }
                }
                finally
                {
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                Console.WriteLine("Disconnected database!!");
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public List<Orders> GetAllPaidOrdersInDay()
        {
            List<Orders> list = new List<Orders>();
            Orders order = null!;
            MySqlCommand cmd = new MySqlCommand("sp_getPaidOrdersInDay", connection);
            try
            {
                connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        order = new Orders();
                        order = GetOrder(reader);
                        list.Add(order);
                    }
                    if (list == null || list.Count == 0)
                    {
                        reader.Close();
                        return null!;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Disconnected database!!");
            }
            finally
            {
                connection.Close();
            }
            return list;
        }



        private Orders GetOrder(MySqlDataReader reader)
        {
            Orders order = new Orders();
            order.orderCustomer = new Customer();
            order.orderId = reader.GetInt32("order_id");
            order.orderDate = reader.GetDateTime("order_date");
            order.total = reader.GetDecimal("unit_price");
            order.orderCustomer.customer_Phone = reader.GetString("customer_phone");
            order.orderCustomer.customer_Name = reader.GetString("customer_name");
            return order;
        }
    }
}

