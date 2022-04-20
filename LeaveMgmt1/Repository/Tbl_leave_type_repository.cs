using LeaveManagement.Models;
//using LeaveManagement.Repository;
using LeaveMgmt1.Interface;
using System.Data.SqlClient;
using System.Linq;

namespace LeaveMgmt1.Repository
{
    public class Tbl_leave_type_repository : ITbl_leave_type_repository
    {
        //private readonly ApiDbContext _context;
        SqlConnection con = new SqlConnection("Data Source = DESKTOP-26BP68T\\MSSQLSERVER3; Initial Catalog = LeaveManagement; Integrated Security=True");

        
        public void AddLeaveType(Tbl_leavetype leave_Type)
        {
            //_context.tbl_leave_type.Add(leave_Type);
            //_context.SaveChanges(); 
            string query = @"Insert into dbo.tbl_leave_type (user_id, name, created_date, updated_date)values ( @user_id, @name,
                     @created_date, @updated_date)";
            SqlCommand cmd = new SqlCommand(query, con);
            
            cmd.Parameters.AddWithValue("@user_id", leave_Type.user_id);
            cmd.Parameters.AddWithValue("@name", leave_Type.name);
            cmd.Parameters.AddWithValue("@created_date", leave_Type.created_date.ToShortDateString());
            cmd.Parameters.AddWithValue("@updated_date", leave_Type.updated_date.ToShortDateString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public int CountOfLeave()
        {
            //int count = _context.tbl_leave_type.Count();
            string query = @"select count(*) from tbl_leave_type ";
            SqlCommand cmd = new SqlCommand(query, con);
            

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();   
            return count;
        }

        public int CountOfLeaveWithUser(int user_id)
        {

            //int count = _context.tbl_leave_type.Count(c => c.user_id.Equals(user_id));
            string query = @"select count(*) from tbl_leave_type where user_id = @user_id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", user_id);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();
            return count;
        }

        public Tbl_leavetype FindLeaveTypeByName(string name)
        {
            //int id = _context.tbl_leave_type.FirstOrDefault(c => c.name.Equals(name)).Id;
            string query = @"Select Id from tbl_leave_type where name = @name";
            Tbl_leavetype leave = new Tbl_leavetype();
            SqlCommand cmd = new SqlCommand(query,con);

            cmd.Parameters.AddWithValue("@name", name);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
                leave.Id = dr.GetInt32(dr.GetOrdinal("Id"));
            dr.Close();
            con.Close();
            return leave;
        }
    }
}
