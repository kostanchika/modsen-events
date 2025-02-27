using EventsAPI.DAL.Entities;

namespace EventsAPI.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public string Location { get; set; }
        public EventCategories Category { get; set; }
        public int MaximumParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public string ImagePath { get; set; }
    }
}
