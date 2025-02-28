using EventsAPI.BLL.Interfaces;
using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.DTO
{
    public class CreateEventDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public string Location { get; set; }
        public EventCategories Category { get; set; }
        public int MaximumParticipants { get; set; }
        public IImageFile? Image { get; set; }
    }
}
