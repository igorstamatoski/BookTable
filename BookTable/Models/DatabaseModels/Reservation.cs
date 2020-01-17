using System;
using BookTable.Models;

namespace BookTable.Models.DatabaseModels
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public Event Event { get; set; }
        public Table Table { get; set; }
        public LoginViewModel User {get; set; }
        public DateTime CreatedAt { get; set; }
    }
}