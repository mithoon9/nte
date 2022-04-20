using LeaveManagement.Models;
using System.Collections.Generic;

namespace LeaveMgmt1.Interface
{
    public interface ITbl_leave_repository
    {

        public void AddLeave(Tbl_leave tbl_Leave);
        public IEnumerable<Tbl_leave> GetAllLeave();
        public int CountOfLeveWithUser(int user_id);
    }
}
