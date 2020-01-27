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
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [MaxLength(100)]
        public String Description { get; set; }
        public Restaurant RestaurantId { get; set; }
        [Display(Name="Image")]
        public String ImageUrl { get; set; }
    }
}