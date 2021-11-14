using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISpaceDAO
    {
        ICollection<Space> GetVenueSpaces(int venueId);

        ICollection<Space> GetAvailableVenueSpaces(int venueId, DateTime reserveDate, int reserveDays, int reserveOccupants, bool requiresAccessibility, int dailyBudget);

        Space GetSpace(int spaceId);

    }
}
