using Microsoft.VisualStudio.TestTools.UnitTesting;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.IntegrationTests
{
    [TestClass]
    public class UserSqlDAOTest : IntegrationTestBase
    {
        [TestMethod]
        public void AddUserReturnsAUserWithABalanceOfOneThousand()
        {
            // Arrange
            string username = "Dave";
            string password = "hunter5";
            UserSqlDAO dao = new UserSqlDAO(ConnectionString);
            AccountSqlDAO accountDao = new AccountSqlDAO(ConnectionString);

            // Act
            User user = dao.AddUser(username, password);
            Account account = accountDao.GetAccount(user.UserId);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(1000.00M, account.Balance);
        }
    }
}
