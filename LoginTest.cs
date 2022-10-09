using DAL;
using Model;
using Xunit;

namespace Test
{
    public class LoginTest
    {
        private StaffDAL staffDal = new StaffDAL();
        private Staff staff = new Staff();

        [Fact]
        public void LoginTest1()
        {

            staff.userName = "Anhduc";
            staff.password = "anhduc123";

            Staff result = staffDal.Login(staff);
            Assert.True(result != null);
            Assert.True(result!.staffRole == 1);
        }

        [Fact]
        public void LoginTest2()
        {

            staff.userName = "hung123";
            staff.password = "Hung321";

            Staff result = staffDal.Login(staff);
            Assert.True(result != null);
            Assert.True(result!.staffRole == 1);
        }

        [Theory]
        [InlineData("new01", "abc123")]
        [InlineData("new02", "abc456A")]
        [InlineData("abcxyz", "abcxyz")]


        public void LoginTest3(string userName, string password)
        {
            staff.userName = userName;
            staff.password = password;
            Staff result = staffDal.Login(staff);
            Assert.True(result == null);
        }
    }
}

