using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTable.Models.DatabaseModels
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public String Description { get; set; }

        public Restaurant RestaurantId { get; set; }

        [Display(Name="Image")]
        public String ImageUrl { get; set; }
    }
}