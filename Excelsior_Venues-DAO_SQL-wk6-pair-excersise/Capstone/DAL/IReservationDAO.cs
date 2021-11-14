using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        int MakeAReservation(Reservation reservation);

        ICollection<Reservation> GetVenueReservations(Venue venue);
    }

        

}


