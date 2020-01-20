using BookTable.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTable.Models.ViewModels
{
    public class TableInRestaurant
    {
        public Table table { get; set; }
        public int restaurantId { get; set; }
    }
}