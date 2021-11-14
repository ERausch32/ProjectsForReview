using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public Member Sender { get; set; }
        public Member Recipient { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}
