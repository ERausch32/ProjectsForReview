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
    public class SpaceSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetVenueSpacesShouldReturnAllSpaces()
        {
            SpaceSqlDAO dao = new SpaceSqlDAO(ConnectionString);

            // Act
            ICollection<Space> results = dao.GetVenueSpaces(1);

            // Assert

            Assert.IsNotNull(results);
            Assert.AreEqual(6, results.Count);
        }
        [TestMethod]

        [DataRow (1, 2021, 10, 01, 1, 10, 5)]
        [DataRow(1, 2021, 01, 01, 1, 10, 3)]
        [DataRow(1, 2021, 10, 01, 5, 10, 4)]
        [DataRow(1, 2021, 10, 03, 12, 10, 4)]
        [DataRow(1, 2021, 10, 06, 10, 10, 4)]
        [DataRow(1, 2021, 10, 01, 1, 100, 3)]
        public void GetAvailableVenueSpacesShouldReturnCorrectSpaces(int venueId, int year, int month, int day, int reserveDays,int reserveOccupants, int expectedSpaceCount)
        {
            SpaceSqlDAO dao = new SpaceSqlDAO(ConnectionString);
            DateTime reserveDate = new DateTime(year, month, day);

            // Act

            ICollection<Space> results = dao.GetAvailableVenueSpaces(venueId, reserveDate, reserveDays, reserveOccupants);

            // Assert

            Assert.IsNotNull(results);
            Assert.AreEqual(expectedSpaceCount, results.Count);
        }
        [TestMethod]
        public void GetSpaceShouldReturnCorrectSpace()
        {
            SpaceSqlDAO dao = new SpaceSqlDAO(ConnectionString);

            // Act
           Space results = dao.GetSpace(1);

            // Assert

            Assert.IsNotNull(results);
            Assert.AreEqual("Otter Offices", results.Name);
        }

    }
}
