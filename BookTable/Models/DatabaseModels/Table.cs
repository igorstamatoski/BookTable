using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookATable.Models.DataBaseModels
{
    public class Table
    {
        public int TableId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int Seats { get; set; }
        public bool Avaliable { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}