using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IVenueDAO
    {
        IList<Venue> GetVenues(int categoryId);

        ICollection<Category> GetCategoriesByVenue(int venueId);

        ICollection<Category> GetCategories();
      
    }
}
