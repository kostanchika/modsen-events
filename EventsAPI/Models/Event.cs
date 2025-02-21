namespace EventsAPI.Models
{
    public enum EventCategories
    {
        Unspecified,
        Music,
        Sports,
        Education,
        Health,
        Art,
        Food,
        Business,
        Literature,
        Film,
        Theatre,
        Fashion,
        Science,
        Gaming
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public string Location { get; set; }
        public EventCategories Category { get; set; }
        public int MaximumParticipants { get; set; }
        public List<User> Participants { get; set; } = new List<User>();
        public string ImagePath { get; set; }
    }
}
