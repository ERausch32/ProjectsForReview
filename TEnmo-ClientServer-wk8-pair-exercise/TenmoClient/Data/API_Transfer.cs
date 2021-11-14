using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Transfer
    {
        public int TransferId { get; set; }
        public API_Member Sender { get; set; }
        public API_Member Recipient { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Gist()
        {
            bool amSender = (Sender.UserId == UserService.UserId);
            API_Member otherParty = amSender ? Recipient : Sender;
            string transferId = TransferId.ToString().PadRight(5);
            string direction = amSender ? "To:  " : "From:";
            string partyName = otherParty.Username.PadRight(15);
            string amount = Amount.ToString("C").PadRight(12);
            string status = "";
            if(Status != "Approved")
            {
                status = Status;
            }
            return $"{transferId}      {direction} {partyName}  {amount} {status}";
        }
        public override string ToString()
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Transfer Details");
            Console.WriteLine(new string('-', 40));
            string amountFormatted = Amount.ToString("C");
            return $"Id:     {TransferId}\n" +
                   $"From:   {Sender.Username}\n" +
                   $"To:     {Recipient.Username}\n" +
                   $"Type:   {Type}\n" +
                   $"Status: {Status}\n" +
                   $"Amount: {amountFormatted}\n" +
                   $"{(new string('-', 10))}\n" +
                    "Enter to continue";
        }
    }
}
