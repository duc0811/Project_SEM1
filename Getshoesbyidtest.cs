using DAL;
using Model;
using Xunit;

namespace Test
{
    public class Getshoesbyidtest   
    {
        ProductsDAL proDAL = new ProductsDAL();

        [Fact]
        public void GetByIdTest1()
        {
            string keySearch = new string("1");
            Product product = new Product();
            Product result = proDAL.GetProductById(keySearch,product);
            Assert.True(result != null);
        }

        [Fact]
        public void GetByIdTest2()
        {
            string keySearch = new string("2");
            Product product = new Product();
            Product result = proDAL.GetProductById(keySearch,product);
            Assert.True(result != null);
        }

        [Theory]
        [InlineData($"{"0"}")]
        [InlineData($"{"-1"}")]
        [InlineData($"{"-2.5"}")]
        public void GetByIdTest4(string keySearch)
        {
            Product product = new Product();
            Product result = proDAL.GetProductById(keySearch,product);
            Assert.True(result == null);
        }
    }
}