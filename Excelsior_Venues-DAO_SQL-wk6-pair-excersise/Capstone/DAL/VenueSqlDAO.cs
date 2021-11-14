using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueSqlDAO : IVenueDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAllVenues = "Select venue.name AS venue_name, venue.id AS venue_id, venue.description, city.name AS city_name, city.state_abbreviation From venue INNER JOIN city ON venue.city_id = city.id ORDER BY venue_name";

        private const string SqlSelectCategoriesByVenue = "Select category.name From category_venue INNER JOIN category ON category.id = category_venue.category_id WHERE category_venue.venue_id = @venue_id";
        public VenueSqlDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Venue> GetAllVenues()
        {
            List<Venue> venues = new List<Venue>();

            try
            {
                using(SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAllVenues, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = new Venue();

                        venue.Id = Convert.ToInt32(reader["venue_id"]);
                        venue.Name = Convert.ToString(reader["venue_name"]);
                        venue.Description = Convert.ToString(reader["description"]);
                        venue.City_Name = Convert.ToString(reader["city_name"]);
                        venue.State_Abv = Convert.ToString(reader["state_abbreviation"]);
                        venues.Add(venue);
                    }
                }


            }
            catch(SqlException ex)
            {
                Console.WriteLine("An eror occured reading database. ");
                Console.WriteLine(ex.Message);
            }
            return venues;
        }

        public ICollection<Category> GetCategoriesByVenue(int venueId)
        {
            List<Category> categories = new List<Category>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectCategoriesByVenue, conn);
                    command.Parameters.AddWithValue("@venue_id", venueId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Category category = new Category();

                        category.Name = Convert.ToString(reader["name"]);

                        categories.Add(category);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An eror occured reading database. ");
                Console.WriteLine(ex.Message);
            }
            return categories;
        }
    }
}
