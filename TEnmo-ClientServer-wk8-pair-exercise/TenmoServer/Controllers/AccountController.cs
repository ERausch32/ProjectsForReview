using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        public AccountController(IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
        }

        /// <summary>
        /// Return the Account attached to the User
        /// </summary>
        [HttpGet()]
        [Authorize]
        public IActionResult FetchAccount()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            return Ok(accountDAO.GetAccount(userId));
        }

        /// <summary>
        /// Return a list of all Members
        /// </summary>
        [HttpGet("member")]
        [Authorize]
        public IActionResult FetchMembers()
        {
            return Ok(accountDAO.GetMembers());
        }

        /// <summary>
        /// Given a Transfer (sender, recipient, amount) validate the request & execute if appropriate.
        /// </summary>
        [HttpPost("transfer")]
        [Authorize]
        public IActionResult SubmitTransfer(Transfer transfer)
        {
            // Does this request contain valid data.
            if (transfer.Amount <= 0 ||
                transfer.Sender.AccountId <= 0 || transfer.Sender.UserId <= 0 ||
                transfer.Recipient.AccountId <= 0 || transfer.Recipient.UserId <= 0)
                return BadRequest(new { message = "Transfer data is not valid" });


            // Is the current user the sender in the transfer.
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            // User is sender, this is a send.
            if (transfer.Sender.UserId == userId && (transfer.Type != "Send" || transfer.Status != "Approved"))
                return BadRequest(new { message = "Not a valid submission for Send Funds" });
            // User is recipient.  This is a request.
            if (transfer.Recipient.UserId == userId && (transfer.Type != "Request" || transfer.Status != "Pending"))
                return BadRequest(new { message = "Not a valid submission for Request Funds." });

            //Do not allow transfers to the same account.
            if (transfer.Sender.AccountId == transfer.Recipient.AccountId)
                return BadRequest(new { message = "Sending and receiving accounts must be different" });

            Account senderAccount = accountDAO.GetAccount(transfer.Sender.UserId);
            Account recipientAccount = accountDAO.GetAccount(transfer.Recipient.UserId);
            // did we get valid accounts.
            if (senderAccount == null || recipientAccount == null)
                return BadRequest(new { message = "Both accounts must exist" });
            // Is the account associated with this user the actual account listed in the transfer as sender.
            // No IntegerMan, you can't use postman to steal!!!
            if (senderAccount.AccountId != transfer.Sender.AccountId
                || recipientAccount.AccountId != transfer.Recipient.AccountId)
                return Forbid();
            // Is there enough funds in the senders account to accommodate the transfer.
            if (senderAccount.Balance < transfer.Amount)
                return BadRequest(new { message = "Insufficient funds to submit transfer" });

            // All validation passed.  Execute the transfer.
            if (transfer.Type == "Send")
            {
                transfer = accountDAO.CompleteTransfer(transfer);
                return Ok(transfer);
            }
            if (transfer.Type == "Request")
            {
                transfer = accountDAO.InitializeRequest(transfer);
                return Ok(transfer);
            }
            return BadRequest(new { message = "Could not complete a transfer without a valid type." });
        }

        /// <summary>
        /// Return a list of all transfers associated with this user.
        /// </summary>
        [HttpGet("transfer")]
        [Authorize]
        public IActionResult FetchTransfers()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            Account account = accountDAO.GetAccount(userId);
            List<Transfer> transfers = accountDAO.GetAccountTransfers(account);
            return Ok(transfers);
        }

        /// <summary>
        /// Given a specific transferId, validate the request & return details on that transfer
        /// </summary>
        [HttpGet("transfer/{transferId}")]
        [Authorize]
        public IActionResult FetchSpecificTransfer(int transferId)
        {
            Transfer transfer = accountDAO.GetSpecificTransfer(transferId);
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            
            // Does the transfer exist and should THIS user have access to it?
            if (transfer == null)
                return NotFound();
            if (transfer.Sender.UserId != userId && transfer.Recipient.UserId != userId)
                return Forbid();

            return Ok(transfer);
        }
        [HttpPut("transfer")]
        [Authorize]
        public IActionResult FinalizePendingTransfer(Transfer requestedTransfer)
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            Transfer existingTransfer = accountDAO.GetSpecificTransfer(requestedTransfer.TransferId);

            if (requestedTransfer.Amount != existingTransfer.Amount
                || requestedTransfer.Sender.AccountId != existingTransfer.Sender.AccountId
                || requestedTransfer.Sender.UserId != existingTransfer.Sender.UserId
                || requestedTransfer.Recipient.AccountId != existingTransfer.Recipient.AccountId
                || requestedTransfer.Recipient.UserId != existingTransfer.Recipient.UserId)
                return BadRequest(new { message = "Requested transfer does not match existing transfer." });

            Account senderAccount = accountDAO.GetAccount(existingTransfer.Sender.UserId);
            // Is there enough funds in the senders account to accommodate the transfer.
            if (senderAccount.Balance < existingTransfer.Amount)
                return BadRequest(new { message = "Insufficient funds to submit transfer" });

            if (existingTransfer.Type != "Request")
                return BadRequest(new { message = "Cannot finalize a transfer that isn't a request." });
            if (existingTransfer.Status != "Pending")
                return BadRequest(new { message = "Cannot update a transfer that isn't pending." });
            if (existingTransfer.Sender.UserId != userId)
                return Forbid();

            existingTransfer.Status = requestedTransfer.Status;
            if (existingTransfer.Status == "Approved")
            {
                accountDAO.ApproveRequest(existingTransfer);
                return Ok();
            }
            if (existingTransfer.Status == "Rejected")
            {
                accountDAO.DenyRequest(existingTransfer);
                return Ok();
            }
            return BadRequest(new { message = "Cannot finalize a transfer without a completed status." });
        }
    }
}