using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceSqlDAO : ISpaceDAO
    {
        private readonly string connectionString;

        private const string SqlSelectVenueSpaces = "SELECT id, venue_id,name, is_accessible, open_from, open_to, daily_rate, max_occupancy FROM space WHERE venue_id = @venue_id";

        private const string SqlSelectAvailableVenueSpaces = "SELECT Top 5 space.id, space.name, space.is_accessible, space.open_from, space.open_to, "+
            "space.daily_rate, space.max_occupancy FROM space WHERE venue_id = @venue_id AND max_occupancy >= @max_occupancy AND "+
            "((open_from <= @reservation_from_month AND open_to >= @reservation_to_month) OR (open_from IS NULL AND open_to IS NULL)) AND " +
            "space.id NOT IN (SELECT reservation.space_id FROM reservation WHERE (@startDate > reservation.start_date AND @startDate<reservation.end_date) OR" +
            "(@endDate > reservation.start_date AND @endDate<reservation.end_date))";

        private const string SqlSelectSpaceBySpaceId = "Select name, daily_rate From Space WHERE id = @id";
        public SpaceSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Given a venue, retrieve all associated spaces and return as a collection.
        /// </summary>
        public ICollection<Space> GetVenueSpaces(int venueId)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectVenueSpaces, conn);
                    command.Parameters.AddWithValue("@venue_id", venueId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Venue_Id = Convert.ToInt32(reader["venue_id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.Is_Accessible = Convert.ToBoolean(reader["is_accessible"]);
                        if (reader["open_from"] != DBNull.Value)
                        {
                            space.Open_From_Month = Convert.ToInt32(reader["open_from"]);
                        }
                        if (reader["open_to"] != DBNull.Value)
                        {
                            space.Open_To_Month = Convert.ToInt32(reader["open_to"]);
                        }
                        space.Daily_Rate = Convert.ToDecimal(reader["daily_rate"]);
                        space.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        spaces.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An eror occured reading database. ");
                Console.WriteLine(ex.Message);
            }
            return spaces;
        }

        /// <summary>
        /// Given user requirements, retrieve TOP 5 matching spaces and return as a collection.
        /// </summary>
        public ICollection<Space> GetAvailableVenueSpaces(int venueId, DateTime reserveDate, int reserveDays, int reserveOccupants)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAvailableVenueSpaces, conn);
                    command.Parameters.AddWithValue("@venue_id", venueId);
                    command.Parameters.AddWithValue("@max_occupancy", reserveOccupants);
                    command.Parameters.AddWithValue("@startDate", reserveDate);
                    command.Parameters.AddWithValue("@endDate", reserveDate.AddDays(reserveDays - 1));
                    command.Parameters.AddWithValue("@reservation_from_month", (int)reserveDate.Month);
                    command.Parameters.AddWithValue("@reservation_to_month", (int)reserveDate.AddDays(reserveDays - 1).Month);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.Is_Accessible = Convert.ToBoolean(reader["is_accessible"]);
                        if (reader["open_from"] != DBNull.Value)
                        {
                            space.Open_From_Month = Convert.ToInt32(reader["open_from"]);
                        }
                        if (reader["open_to"] != DBNull.Value)
                        {
                            space.Open_To_Month = Convert.ToInt32(reader["open_to"]);
                        }
                        space.Daily_Rate = Convert.ToDecimal(reader["daily_rate"]);
                        space.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        spaces.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An eror occured reading database. ");
                Console.WriteLine(ex.Message);
            }
            return spaces;
        }

        /// <summary>
        /// Given a space, retrieve name & daily rate
        /// </summary>
        public Space GetSpace(int spaceId)
        {
            Space space = new Space();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectSpaceBySpaceId, conn);
                    command.Parameters.AddWithValue("@id", spaceId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        space.Name = Convert.ToString(reader["name"]);
                        space.Daily_Rate = Convert.ToDecimal(reader["daily_rate"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An eror occured reading database. ");
                Console.WriteLine(ex.Message);
            }
            return space;
        }
    }
}
