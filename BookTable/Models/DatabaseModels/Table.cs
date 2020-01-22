using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTable.Models.DatabaseModels
{
    public class Table
    {
        public int TableId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int Seats { get; set; }
        public bool Avaliable { get; set; }
    }
}