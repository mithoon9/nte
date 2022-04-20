using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Models
{
    public class Tbl_leave
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        [Required]
        public string leave_title { get; set; }
        [Required]
        public string leave_description { get; set; }
        public int leave_type { get; set; }
        [Required]
        public DateTime from_date { get; set; }
        [Required]
        public DateTime to_date { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
