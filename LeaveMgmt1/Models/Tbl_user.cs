using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace LeaveManagement.Models
{
    public class Tbl_user
    {
        public int Id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string contact { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }        

    }
}
