using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; }

        public int Venue_Id { get; }

        public string Name { get; }

        public bool Is_Accessible { get; }

        public int Open_From_Month { get; }

        public int Open_To_Month { get; }

        public decimal Daily_Rate { get; }

        public int Max_Occupancy { get; }

        public Space(int spaceId, int venueId, string name, bool isAccessible, int openFromMonth, int openToMonth, decimal dailyRate, int maxOccupancy)
        {
            this.Id = spaceId;
            this.Venue_Id = venueId;
            this.Name = name;
            this.Is_Accessible = isAccessible;
            this.Open_From_Month = openFromMonth;
            this.Open_To_Month = openToMonth;
            this.Daily_Rate = dailyRate;
            this.Max_Occupancy = maxOccupancy;
        }

    }
}
