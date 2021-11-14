using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IVenueDAO
    {
        IList<Venue> GetAllVenues();

        ICollection<Category> GetCategoriesByVenue(int venueId);
      
    }
}
