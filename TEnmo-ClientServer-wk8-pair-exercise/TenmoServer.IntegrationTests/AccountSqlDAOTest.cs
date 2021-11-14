using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.IntegrationTests
{
    [TestClass]
    public class AccountSqlDAOTest : IntegrationTestBase
    {
        [TestMethod]
        public void GetAccountShouldReturnCorrectAccount()
        {
            // Arrange
            int userId = 3000;
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);

            // Act
            Account account = dao.GetAccount(userId);

            // Assert
            Assert.AreEqual(4000, account.AccountId);
            Assert.AreEqual(1000.00M, account.Balance);
        }
        [TestMethod]
        public void GetMembersReturnsAllMembers()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);

            // Act
            List<Member> result = dao.GetMembers();
            Member firstMember = result.First();

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(4000, firstMember.AccountId);
        }
        [TestMethod]
        public void CompleteTransferMovesMoneyBetweenAccounts()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);
            List<Member> members = dao.GetMembers();
            Transfer transfer = new Transfer()
            {
                Sender = members[0],
                Recipient = members[1],
                Amount = 100.00M,
            };

            // Act
            dao.CompleteTransfer(transfer);

            // Assert
            Account sendingAccount = dao.GetAccount(transfer.Sender.UserId);
            Account receivingAccount = dao.GetAccount(transfer.Recipient.UserId);
            Assert.AreEqual(900.00M, sendingAccount.Balance);
            Assert.AreEqual(1100.00M, receivingAccount.Balance);
        }
        [TestMethod]
        public void GetTransfersListsTransfersForAccount()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);
            UserSqlDAO userDao = new UserSqlDAO(ConnectionString);
            Account account = dao.GetAccount(3000);

            // Act
            List<Transfer> transfers = dao.GetAccountTransfers(account);

            // Assert
            Assert.AreEqual(3, transfers.Count());
        }
        [TestMethod]
        public void GetSpecificTransferGetsTransferDetails()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);
            int transferId = 1;

            // Act
            Transfer transfer = dao.GetSpecificTransfer(transferId);

            // Assert
            Assert.AreEqual("Alice", transfer.Sender.Username);
            Assert.AreEqual("Bob", transfer.Recipient.Username);
            Assert.AreEqual(100.00M, transfer.Amount);
            Assert.AreEqual("Approved", transfer.Status);
            Assert.AreEqual("Send", transfer.Type);
        }
        [TestMethod]
        public void ApproveRequestMovesMoneyBetweenAccountsAndChangesStatus()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);
            int transferId = 2;
            Transfer transfer = dao.GetSpecificTransfer(transferId);
            List<Member> members = dao.GetMembers();
            Member sendingMember = members[0];
            Member receivingMember = members[1];

            // Act
            dao.ApproveRequest(transfer);

            // Assert
            Account sendingAccount = dao.GetAccount(sendingMember.UserId);
            Account receivingAccount = dao.GetAccount(receivingMember.UserId);
            Transfer updatedTransfer = dao.GetSpecificTransfer(transferId);
            Assert.AreEqual(900.00M, sendingAccount.Balance);
            Assert.AreEqual(1100.00M, receivingAccount.Balance);
            Assert.AreEqual("Approved", updatedTransfer.Status);
            Assert.AreEqual("Request", updatedTransfer.Type);
        }
        [TestMethod]
        public void DenyRequestChangesStatus()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);
            int transferId = 2;
            Transfer transfer = dao.GetSpecificTransfer(transferId);
            List<Member> members = dao.GetMembers();
            Member sendingMember = members[0];
            Member receivingMember = members[1];

            // Act
            dao.DenyRequest(transfer);

            // Assert
            Account sendingAccount = dao.GetAccount(sendingMember.UserId);
            Account receivingAccount = dao.GetAccount(receivingMember.UserId);
            Transfer updatedTransfer = dao.GetSpecificTransfer(transferId);
            Assert.AreEqual(1000.00M, sendingAccount.Balance);
            Assert.AreEqual(1000.00M, receivingAccount.Balance);
            Assert.AreEqual("Rejected", updatedTransfer.Status);
            Assert.AreEqual("Request", updatedTransfer.Type);
        }
        [TestMethod]
        public void InitializeRequestCreatesAPendingTrasnfer()
        {
            // Arrange
            AccountSqlDAO dao = new AccountSqlDAO(ConnectionString);
            Account account = dao.GetAccount(3000);
            List<Member> members = dao.GetMembers();
            int numberOfTransfers = dao.GetAccountTransfers(account).Count();
            Transfer transfer = new Transfer
            {
                Sender = members[0],
                Recipient = members[1],
                Amount = 100.00M
            };

            // Act
            transfer = dao.InitializeRequest(transfer);

            // Assert
            Transfer submittedTransfer = dao.GetSpecificTransfer(transfer.TransferId);
            Assert.AreEqual(numberOfTransfers + 1, dao.GetAccountTransfers(account).Count());
            Assert.AreEqual("Pending", submittedTransfer.Status);
        }
    }
}
