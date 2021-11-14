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
        public void GetAllVenuesShouldReturnCorrectAmountOfVenues()
        {
            VenueSqlDAO dao = new VenueSqlDAO(ConnectionString);

            // Act
            IList<Venue> results = dao.GetAllVenues();

            // Assert

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
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
     
    }
    
}
