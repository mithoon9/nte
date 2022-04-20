using LeaveManagement.Models;
//using LeaveManagement.Repository;
using LeaveMgmt1.Interface;
using LeaveMgmt1.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;

namespace LeaveMgmt1.Repository
{
    public class Tbl_user_repository : ITbl_user_repository
    {
        //private readonly ApiDbContext _context;
        //private readonly IConfiguration _config;

        //public Tbl_user_repository(IConfiguration config) { _config = config; }

        //private string connectionString = _config.GetSection("ConnectionStrings:MyConnection").Value;
        public SqlConnection con = new SqlConnection("Data Source = DESKTOP-26BP68T\\MSSQLSERVER3; Initial Catalog = LeaveManagement; Integrated Security=True");

        public bool register(Tbl_user user)
        {
            //_context.Add(user);
            //_context.SaveChanges();
            int row = 0;
            using (con)
            {
                
                string query = @"Insert into dbo.tbl_user( first_name, last_name, email, password, contact, created_date, 
                        updated_date) values( @first_name, @last_name, @email, @password, @contact, @created_date, 
                        @updated_date)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@first_name", user.first_name);
                cmd.Parameters.AddWithValue("@last_name", user.last_name);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@contact", user.contact);
                cmd.Parameters.AddWithValue("@created_date", user.created_date.Date.ToShortDateString());
                cmd.Parameters.AddWithValue("@updated_date", user.updated_date.Date.ToShortDateString());
                
                con.Open();
                row = cmd.ExecuteNonQuery();
                con.Close();

            }
            
            if (row > 0)
                return true;
            return false;
        }

        public Tbl_user GetUser(LoginModel user)
        {
            //Tbl_user tbl_user = _context.tbl_user.FirstOrDefault(o => o.email.Equals(user.email) 
            //&& o.password.Equals(user.password));
            //return tbl_user;
            string query = @"Select * from tbl_user where email=@email and password=@password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", user.email);
            cmd.Parameters.AddWithValue("@password", user.password);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                Tbl_user nuser = new Tbl_user();
                if(!dr.HasRows)
                    return null;
                
                    while (dr.Read())
                    {


                        nuser.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                        nuser.first_name = dr.GetString(dr.GetOrdinal("first_name"));
                        nuser.last_name = dr.GetString(dr.GetOrdinal("last_name"));
                        nuser.email = dr.GetString(dr.GetOrdinal("email"));
                        nuser.password = dr.GetString(dr.GetOrdinal("password"));
                        nuser.contact = dr.GetString(dr.GetOrdinal("contact"));
                        nuser.created_date = dr.GetDateTime(dr.GetOrdinal("created_date"));
                        nuser.updated_date = dr.GetDateTime(dr.GetOrdinal("updated_date"));


                    };
                
                dr.Close();
                con.Close();
                return nuser;
            }      
               
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool IsExisting(string email)
        {
            //var ispresent = _context.tbl_user.FirstOrDefault(o => o.email.Equals(email));
            string query = @"Select * from tbl_user where email=@email";
            bool ispresent = false;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            try
            {
                
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    ispresent = true;
                
            }
          
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                con.Close();
                
            }
            if (ispresent)
                return ispresent;
            return ispresent;
        }
    }
}
