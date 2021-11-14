using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int City_Id { get; set; }

        public string Description { get; set; }

        public string City_Name { get; set; }

        public string State_Abv { get; set; }

    }
}
