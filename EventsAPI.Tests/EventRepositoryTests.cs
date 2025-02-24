using AutoMapper;
using EventsAPI.Data;
using EventsAPI.Mappers;
using EventsAPI.Models;
using EventsAPI.Repository;
using EventsAPI.Services;
using EventsAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Tests
{
    public class EventRepositoryTests
    {
        public static ApplicationContext CreateDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            List<Event> events = [
                    new Event { Name = "Вечеринка у бассейна", EventDateTime = DateTime.UtcNow.AddDays(1), Location = "Бассейн у Влада", MaximumParticipants = 10, Participants = [], Category = EventCategories.Health, Description = "Поплаваем, отдохнём", ImagePath="/images/default.png" },
                    new Event { Name = "Дискотека 90-х", EventDateTime = DateTime.UtcNow.AddDays(5), Location = "Ретро клуб", MaximumParticipants = 50, Participants = [], Category = EventCategories.Music, Description = "Танцуют все!", ImagePath="/images/default.png" },
                ];

            var context = new ApplicationContext(options);
            context.Set<Event>().AddRange(events);
            context.SaveChanges();

            return context;
        }

        [Fact(DisplayName = "Получение всех событий")]
        public async Task GetAllEvents()
        {
            var db = CreateDatabase();
            var eventRepository = new EventRepository(db);

            Assert.Equal(2, (await eventRepository.GetAllAsync()).Count());
        }

        [Fact(DisplayName = "Получение отфильтрованных событий")]
        public void GetFilteredEvents()
        {
            var db = CreateDatabase();
            var eventRepository = new EventRepository(db);

            var request = new GetEventsModel("90", DateTime.UtcNow.AddDays(4), DateTime.UtcNow.AddYears(1), null, EventCategories.Music, Page: 1, PageSize: 10);
            Assert.Single(eventRepository.GetAllEventsWithFilters(request));
        }

        [Fact(DisplayName = "Создание корректного события")]
        public async Task CreateEvent()
        {
            var db = CreateDatabase();
            var eventRepository = new EventRepository(db);
            var creatingEvent = new CreateEventModel(
                "Игра в футбол",
                "Надо иметь разряд по футболу",
                DateTime.UtcNow.AddMonths(1),
                "Стадион Динамо",
                EventCategories.Sports,
                22,
                null
            );

            var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMapper>());
            var mapper = config.CreateMapper();

            var creatingEventValidator = new CreateEventValidator();
            var result = creatingEventValidator.Validate(creatingEvent);

            if (result.IsValid)
            {
                var eventItem = mapper.Map<Event>(creatingEvent);
                eventItem.ImagePath = "/images/default.png";
                await eventRepository.AddAsync(eventItem);
            }

            Assert.Equal(3, (await eventRepository.GetAllAsync()).Count());
        }

        [Fact(DisplayName = "Создание некорректного события")]
        public async Task CreateBadEvent()
        {
            var db = CreateDatabase();
            var eventRepository = new EventRepository(db);
            var creatingEvent = new CreateEventModel(
                "Игра в футбол",
                "Надо иметь разряд по футболу",
                DateTime.UtcNow.AddMonths(1),
                "Стадион Динамо",
                EventCategories.Unspecified,
                22,
                null
            );

            var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMapper>());
            var mapper = config.CreateMapper();

            var creatingEventValidator = new CreateEventValidator();
            var result = creatingEventValidator.Validate(creatingEvent);

            if (result.IsValid)
            {
                var eventItem = mapper.Map<Event>(creatingEvent);
                eventItem.ImagePath = "/images/default.png";
                await eventRepository.AddAsync(eventItem);
            }

            Assert.Equal(2, (await eventRepository.GetAllAsync()).Count());
        }
    }
}