using LeaveManagement.Models;
//using LeaveManagement.Repository;
using LeaveMgmt1.Interface;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace LeaveMgmt1.Repository
{
    public class Tbl_leave_repository: ITbl_leave_repository
    {
        //private readonly ApiDbContext _context;
        SqlConnection con = new SqlConnection("Data Source = DESKTOP-26BP68T\\MSSQLSERVER3; Initial Catalog = LeaveManagement; Integrated Security=True");

        //public Tbl_leave_repository(ApiDbContext context) { 
        //    //_context = context;
        //    }

        public void AddLeave(Tbl_leave tbl_Leave)
        {
            //_context.tbl_leave.Add(tbl_Leave);
            //_context.SaveChanges();
            string query = @"Insert into dbo.tbl_leave(user_id, leave_title, leave_description, leave_type, from_date, to_date
                    created_date, updated_date) values ( @user_id, @leave_title, 
                    @leave_description, @leave_type, @from_date, @to_date, @created_date, @updated_date)";
            SqlCommand cmd = new SqlCommand(query, con);
            
            cmd.Parameters.AddWithValue("@user_id", tbl_Leave.user_id);
            cmd.Parameters.AddWithValue("@leave_title", tbl_Leave.leave_title);
            cmd.Parameters.AddWithValue("@leave_description", tbl_Leave.leave_description);
            cmd.Parameters.AddWithValue("@leave_type", tbl_Leave.leave_type);
            cmd.Parameters.AddWithValue("@from_date", tbl_Leave.from_date);
            cmd.Parameters.AddWithValue("@to_date", tbl_Leave.to_date);
            cmd.Parameters.AddWithValue("@created_date", tbl_Leave.created_date);
            cmd.Parameters.AddWithValue("@updated_date", tbl_Leave.updated_date);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public int CountOfLeveWithUser(int user_id)
        {
            //int count = _context.tbl_leave.Count(c => c.user_id.Equals(user_id));
            string query = @"select count(*) from tbl_leave where user_id = @user_id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();
            return count;
        }

        public IEnumerable<Tbl_leave> GetAllLeave()
        {
            //return _context.tbl_leave;
            List<Tbl_leave> leaves = new List<Tbl_leave>();

            string query = @"select * from tbl_leave";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Tbl_leave tbl_Leave = new Tbl_leave()
                {
                    Id = dr.GetInt32(dr.GetOrdinal("Id")),
                    user_id = dr.GetInt32(dr.GetOrdinal("user_id")),
                    leave_title = dr.GetString(dr.GetOrdinal("leave_title")),
                    leave_description = dr.GetString(dr.GetOrdinal("leave_description")),
                    leave_type = dr.GetInt32(dr.GetOrdinal("leave_type")),
                    from_date = dr.GetDateTime(dr.GetOrdinal("from_date")),
                    to_date = dr.GetDateTime(dr.GetOrdinal("to_date")),
                    created_date = dr.GetDateTime(dr.GetOrdinal("created_date")),
                    updated_date = dr.GetDateTime(dr.GetOrdinal("updated_date")),
                };
                leaves.Add(tbl_Leave);
            }
            dr.Close();
            con.Close();
            return leaves;
        }
    }
}
