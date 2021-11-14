using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private readonly string connectionString;

        private const string SqlInsertReservation = "INSERT INTO reservation (space_id, number_of_attendees, start_date, " +
            "end_date, reserved_for) VALUES (@space_id, @number_of_attendees, @start_date, @end_date, @reserved_for); SELECT @@IDENTITY;";

        private const string SqlSelectVenueReservations = " SELECT name, reserved_for, start_date, end_date, space_id, number_of_attendees, daily_rate from reservation " +
            "INNER JOIN space ON reservation.space_id = space.id WHERE space.venue_id = @venue_id AND (reservation.start_date >= GETDATE() AND reservation.start_date<GETDATE() + 30)";

        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Take in a reservation and insert into reservation table.
        /// </summary>
        public int MakeAReservation(Reservation reservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlInsertReservation, conn);
                    command.Parameters.AddWithValue("@space_id", reservation.Space_Id);
                    command.Parameters.AddWithValue("@number_of_attendees", reservation.Number_Of_Attendees);
                    command.Parameters.AddWithValue("@start_date", reservation.Start_Date);
                    command.Parameters.AddWithValue("@end_date", reservation.End_Date);
                    command.Parameters.AddWithValue("@reserved_for", reservation.Reserved_For);
                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return id;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An eror occured writing to database. ");
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        public ICollection<Reservation> GetVenueReservations(Venue venue)
        {

            List<Reservation> reservations = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectVenueReservations, conn);
                    command.Parameters.AddWithValue("@venue_id", venue.Id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int spaceId = Convert.ToInt32(reader["space_id"]);
                        int attendees = Convert.ToInt32(reader["number_of_attendees"]);
                        DateTime start = Convert.ToDateTime(reader["start_date"]);
                        DateTime end = Convert.ToDateTime(reader["end_date"]);
                        TimeSpan difference = end.Date - start.Date;
                        int reserveDays = (int)difference.TotalDays;
                        string reservedFor = Convert.ToString(reader["reserved_for"]);
                        string name = Convert.ToString(reader["name"]);
                        decimal rate = Convert.ToDecimal(reader["daily_rate"]);

                        Reservation reservation = new Reservation(spaceId, attendees, start, reserveDays, reservedFor, name, venue.Name, rate);
                        reservations.Add(reservation);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An eror occured reading database. ");
                Console.WriteLine(ex.Message);
            }
            return reservations;
        }
    }
}

