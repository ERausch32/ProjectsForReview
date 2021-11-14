using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {


        public Reservation(int space_Id, int reserveOccupants, DateTime reserveDate, int reserveDays, string reservationName, string spaceName, string venueName, decimal dailyRate)
        {
            this.Space_Id = space_Id;
            this.Number_Of_Attendees = reserveOccupants;
            this.Start_Date = reserveDate;
            this.End_Date = reserveDate.Date.AddDays(reserveDays - 1);
            this.Reserved_For = reservationName;
            this.Space_Name = spaceName;
            this.Venue_Name = venueName;
            this.Total_Cost = dailyRate * reserveDays;
        }

        public int Reservation_Id { get; private set; }

        public int Space_Id { get; }

        public int Number_Of_Attendees { get; }

        public DateTime Start_Date { get; }

        public DateTime End_Date { get; }

        public string Reserved_For { get; }

        public string Venue_Name { get; }

        public string Space_Name { get; }

        public decimal Total_Cost { get; }
    public void ConfirmReservation(int reservation_Id)
        {
            this.Reservation_Id = reservation_Id;
        }
        public override string ToString()
        {
            return $"Confirmation #: {Reservation_Id}\n" +
            $"Venue: {Venue_Name}\n" +
            $"Space: {Space_Name}\n" +
            $"Reserved For: {Reserved_For}\n" +
            $"Attendees: {Number_Of_Attendees}\n" +
            $"Arrival Date: {Start_Date.ToShortDateString()}\n" +
            $"Depart Date: {End_Date.ToShortDateString()}\n" +
            $"Total Cost {Total_Cost.ToString("C")}";
        }
    }
}
