using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTable.Models.DatabaseModels
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string OwnerId { get; set; }
        public bool Approved { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Category { get; set; }
        [MaxLength(100)]
        public String Description { get; set; }
        [Required]
        public String Address { get; set; }
    }
}