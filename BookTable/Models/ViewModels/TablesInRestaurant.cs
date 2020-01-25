using BookTable.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTable.Models.ViewModels
{
    public class TablesInRestaurant
    {
        public List<Table> tables { get; set; }
        public int RestaurantId { get; set; }

        public TablesInRestaurant()
        {
            tables = new List<Table>();
        }
    }
}