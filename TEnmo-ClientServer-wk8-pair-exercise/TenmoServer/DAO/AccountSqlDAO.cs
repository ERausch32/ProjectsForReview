using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        const string getAccountSqlString =
            "SELECT account_id, user_id, balance FROM accounts WHERE user_id = @user_id;";

        const string getMembersSqlString =
            "SELECT account_id, u.user_id, username FROM users u JOIN accounts a ON a.user_id = u.user_id";

        // transferSqlString includes 2 updates & 1 insert enclosed within a transaction as they are interdependant.
        const string transferSqlString =
            "BEGIN TRANSACTION;" +
            "UPDATE accounts SET balance -= @amount WHERE account_id = @sending_account_id;" +
            "UPDATE accounts SET balance += @amount WHERE account_id = @receiving_account_id;" +
            "INSERT INTO transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
            "VALUES(1001, 2001, @sending_account_id, @receiving_account_id, @amount);" +
            "COMMIT TRANSACTION;" +
            "SELECT @@IDENTITY";

        const string approveSqlString =
            "BEGIN TRANSACTION;" +
            "UPDATE accounts SET balance -= @amount WHERE account_id = @sending_account_id;" +
            "UPDATE accounts SET balance += @amount WHERE account_id = @receiving_account_id;" +
            "UPDATE transfers SET transfer_status_id = @transfer_status_id WHERE transfer_id = @transfer_id " +
            "COMMIT TRANSACTION;";

        const string denySqlString =
            "UPDATE transfers SET transfer_status_id = @transfer_status_id WHERE transfer_id = @transfer_id";

        const string initializeSqlString =
            "INSERT INTO transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
            "VALUES(1000, 2000, @sending_account_id, @receiving_account_id, @amount);" +
            "SELECT @@IDENTITY";

        //  base string returns all Transfers in DB. Can be modified by appending Where clauses
        const string baseGetTransferSqlString =
            "SELECT t.transfer_id, t.account_from, t.account_to, t.amount, " +
            "u_f.user_id AS sender_id, u_t.user_id AS recipient_id, " +
            "u_f.username AS sender_name, u_t.username AS recipient_name, " +
            "t_s.transfer_status_desc, t_t.transfer_type_desc " +
            "FROM transfers t " +
            "JOIN accounts a_f ON a_f.account_id = t.account_from " +
            "JOIN users u_f ON u_f.user_id = a_f.user_id " +
            "JOIN accounts a_t ON a_t.account_id = t.account_to " +
            "JOIN users u_t ON u_t.user_id = a_t.user_id " +
            "JOIN transfer_types t_t ON t.transfer_type_id = t_t.transfer_type_id " +
            "JOIN transfer_statuses t_s ON t.transfer_status_id = t_s.transfer_status_id ";
        //  Append to base string to return all transfers associated with an account.
        const string getTransfersSqlString =
            baseGetTransferSqlString +
            "WHERE t.account_from = @account_id OR t.account_to = @account_id;";
        //  Append to base string to return a specific transfer.
        const string getSpecificTransferSqlString =
            baseGetTransferSqlString +
            "WHERE t.transfer_id = @transfer_id;";

        private readonly string connectionString;
        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Given a userID, return an associated account.
        /// </summary>
        public Account GetAccount(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getAccountSqlString, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return GetAccountFromReader(reader);
                }
                return null;
            }
        }

        /// <summary>
        /// Return a list of all Members
        /// </summary>
        public List<Member> GetMembers()
        {
            List<Member> members = new List<Member>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getMembersSqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Member m = GetMemberFromReader(reader);
                        members.Add(m);
                    }
            }
            return members;
        }

        /// <summary>
        /// Given an Account, Return a list of all Transactions
        /// Where the Account is Sender or Recipient
        /// </summary>
        public List<Transfer> GetAccountTransfers(Account account)
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getTransfersSqlString, conn);
                cmd.Parameters.AddWithValue("@account_id", account.AccountId);
                SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer t = GetTransferFromReader(reader);
                        transfers.Add(t);
                    }
            }
            return transfers;
        }

        /// <summary>
        /// Given a specific transferID, return all details of that Transfer.
        /// </summary>
        public Transfer GetSpecificTransfer(int transferId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getSpecificTransferSqlString, conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return GetTransferFromReader(reader);
                }
                return null;
            }
        }

        /// <summary>
        /// Given a sending & recipient Member & an amount to transfer
        /// Transfer funds between the member Accounts and record a Transaction.
        /// </summary>
        public Transfer CompleteTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(transferSqlString, conn);
                cmd.Parameters.AddWithValue("@sending_account_id", transfer.Sender.AccountId);
                cmd.Parameters.AddWithValue("@receiving_account_id", transfer.Recipient.AccountId);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                transfer.TransferId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return transfer;
        }

        public void ApproveRequest(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(approveSqlString, conn);
                cmd.Parameters.AddWithValue("@sending_account_id", transfer.Sender.AccountId);
                cmd.Parameters.AddWithValue("@receiving_account_id", transfer.Recipient.AccountId);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                cmd.Parameters.AddWithValue("@transfer_status_id", 2001);
                cmd.ExecuteScalar();
            }
        }
        public void DenyRequest(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(denySqlString, conn);
                cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                cmd.Parameters.AddWithValue("@transfer_status_id", 2002);
                cmd.ExecuteScalar();
            }
        }
        public Transfer InitializeRequest(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(initializeSqlString, conn);
                cmd.Parameters.AddWithValue("@sending_account_id", transfer.Sender.AccountId);
                cmd.Parameters.AddWithValue("@receiving_account_id", transfer.Recipient.AccountId);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                transfer.TransferId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return transfer;
        }

        /// <summary>
        /// Build a Member
        /// </summary>
        private Member GetMemberFromReader(SqlDataReader reader)
        {
            return new Member()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
            };
        }

        /// <summary>
        /// Build an Account
        /// </summary>
        private Account GetAccountFromReader(SqlDataReader reader)
        {
            return new Account()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };
        }

        /// <summary>
        /// Build a Transfer
        /// </summary>
        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            return new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                Sender = new Member()
                {
                    AccountId = Convert.ToInt32(reader["account_from"]),
                    UserId = Convert.ToInt32(reader["sender_id"]),
                    Username = Convert.ToString(reader["sender_name"]),
                },
                Recipient = new Member()
                {

                    AccountId = Convert.ToInt32(reader["account_to"]),
                    UserId = Convert.ToInt32(reader["recipient_id"]),
                    Username = Convert.ToString(reader["recipient_name"]),
                },
                Amount = Convert.ToDecimal(reader["amount"]),
                Status = Convert.ToString(reader["transfer_status_desc"]),
                Type = Convert.ToString(reader["transfer_type_desc"]),
            };
        }
    }
}
