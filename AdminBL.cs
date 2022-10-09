using Model;
using DAL;

namespace BL
{
    public class AdminBL
    {
        private AdminDAL adminDAL = new AdminDAL();
        public AdminS LoginAdmin(AdminS admin)
        {
            return adminDAL.LoginAdmin(admin);
        }
    }
}