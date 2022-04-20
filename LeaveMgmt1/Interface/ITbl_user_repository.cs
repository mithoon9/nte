using LeaveManagement.Models;
using LeaveMgmt1.Models;

namespace LeaveMgmt1.Interface
{
    public interface ITbl_user_repository
    {
        public bool register(Tbl_user user);
        public Tbl_user GetUser(LoginModel user);
        public bool IsExisting(string email);
    }
}
