using LeaveManagement.Models;

namespace LeaveMgmt1.Interface
{
    public interface ITbl_leave_type_repository
    {
        public void AddLeaveType(Tbl_leavetype leave_Type);
        public int CountOfLeave();
        public int CountOfLeaveWithUser(int user_id);
        public Tbl_leavetype FindLeaveTypeByName(string name);
    }
}
