using Model;

namespace DAL
{
    public interface IStaff
    {
         public Staff Login(Staff staff);
         public int RegUser(Staff staff);
    }
}