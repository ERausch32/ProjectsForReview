using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }

        public int Venue_Id { get; set; }

        public string Name { get; set; }

        public bool Is_Accessible { get; set; }

        public int Open_From_Month { get; set; } = 0;

        public int Open_To_Month { get; set; } = 0;

        public decimal Daily_Rate { get; set; }

        public int Max_Occupancy { get; set; }

    }
}
