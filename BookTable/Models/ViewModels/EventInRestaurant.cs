using BookTable.Models.DatabaseModels;

namespace BookTable.Models.ViewModels
{
    public class EventInRestaurant
    {
        public Event evnt { get; set; }
        public int restaurantId { get; set; }
    }
}