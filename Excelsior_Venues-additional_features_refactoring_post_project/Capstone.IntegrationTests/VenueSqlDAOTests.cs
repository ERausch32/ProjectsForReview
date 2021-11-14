using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 1)]
        public void GetVenuesShouldReturnCorrectAmountOfVenues(int filter, int expectedRows)
        {
            VenueSqlDAO dao = new VenueSqlDAO(ConnectionString);

            // Act
            IList<Venue> results = dao.GetVenues(filter);

            // Assert

            Assert.IsNotNull(results);
            Assert.AreEqual(expectedRows, results.Count);
        }
        [TestMethod]
        public void GetCategoriesByVenueShouldReturnCorrectAmountOfVenues()
        {
            VenueSqlDAO dao = new VenueSqlDAO(ConnectionString);

            // Act
            ICollection<Category> results = dao.GetCategoriesByVenue(1);

            // Assert

            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void GetCategoriesShouldReturnAllCategories()
        {
            VenueSqlDAO dao = new VenueSqlDAO(ConnectionString);

            // Act
            ICollection<Category> results = dao.GetCategories();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(6, results.Count);
        }

    }

}
