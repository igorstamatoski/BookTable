using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookATable.Models.DataBaseModels
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public bool Approved { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }
        public String Description { get; set; }
        public int Rating { get; set; }
    }
}