using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Transactions;

namespace Capstone.IntegrationTests
{
    /// <summary>
    /// The base class containing convenience methods for managing transactions and querying the database.
    /// Tests inheriting from this class will automatically have their actions run inside of a transaction.
    /// Additionally, the contents of setup.sql will be run before each test in order to put the database into
    /// a known good state prior to testing.
    /// </summary>
    [TestClass]
    public abstract class IntegrationTestBase
    {
        private TransactionScope trans;

        /// <summary>
        /// Gets the connection string used to connect to the database
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return "Data Source=.\\sqlexpress;Initial Catalog=excelsior_venues;Integrated Security=True";
            }
        }

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope(); // BEGIN TRANSACTION

            // Get the SQL Script to run
            string sql = File.ReadAllText("setup.sql");

            // Execute the script
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Reset()
        {
            trans?.Dispose(); // ROLLBACK TRANSACTION
        }

        /// <summary>
        /// Gets the row count for a table.
        /// </summary>
        /// <param name="table">The name of the table to get the total number of rows for</param>
        /// <returns>The total number of rows in that table</returns>
        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
        }
    }
}
