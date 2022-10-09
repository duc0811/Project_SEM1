using Model;

namespace DAL
{
    public interface IProducts
    {
         public Product GetProductById(string searchKeyWord, Product product);
         public List<Product> GetProductList(List<Product> list, string commandText);
        public int Update(Product product);
        public int Delete(Product product);
        public int InS(Product product);

    }
}