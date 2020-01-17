using System;
using BookTable.Models;

namespace BookATable.Models.DataBaseModels
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public LoginViewModel User {get; set; }
        public DateTime Time { get; set; }
    }
}