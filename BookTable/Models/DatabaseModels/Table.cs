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
        [Range(1,15,ErrorMessage ="Table seats must be between 1 and 15")]
        public int Seats { get; set; }
        public bool Avaliable { get; set; }
    }
}