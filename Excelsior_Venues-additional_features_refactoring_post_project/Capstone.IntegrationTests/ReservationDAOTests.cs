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
    public class ReservationDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void MakeAReservationShouldMakeAReservation()
        {
            ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);
            DateTime reserveDate = new DateTime(2021, 09, 10);
            Reservation reservation = new Reservation(3, 20, reserveDate, 5, "tom", "space", "venue", 100M);

            // Act
            int results = dao.MakeAReservation(reservation);

            // Assert

            Assert.IsTrue(results > 3);
            Assert.AreEqual(4, GetRowCount("reservation"));
        }
    
        
    // This DAO method is based off of todays date.  (get reservations within the next 30 days.)
    // It will only be valid until 11/05/2021.   How do you code around something based
    // off of current date?   
    [TestMethod]
    public void GetVenueReservationsShouldReturnCorrectNumberOfReservations()
    {
        ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);
            Venue venue = new Venue();
            venue.Id = 1;
            venue.Name = "Test Venue";

        // Act
            ICollection<Reservation> reservations = dao.GetVenueReservations(venue);

        // Assert

        Assert.AreEqual(1, reservations.Count);
    }
    }
}
