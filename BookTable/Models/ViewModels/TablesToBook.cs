using BookTable.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTable.Models.ViewModels
{
    public class TablesToBook
    {
        public Restaurant restaurant { get; set; }
        public List<Table> tables { get; set; }
        public int bookedTableId { get; set; }
        public int eventId { get; set; } 

        public TablesToBook()
        {
            tables = new List<Table>();
        }
    }
}