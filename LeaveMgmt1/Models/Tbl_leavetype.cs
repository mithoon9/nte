using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Models
{
    public class Tbl_leavetype
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        [Required]
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
