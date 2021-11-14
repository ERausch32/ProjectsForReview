using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Member
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
