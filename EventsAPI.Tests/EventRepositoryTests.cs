using AutoMapper;
using EventsAPI.BLL.Mappers;
using EventsAPI.BLL.Models;
using EventsAPI.BLL.Validators;
using EventsAPI.DAL.Data;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Repositories;
using EventsAPI.Mappers;
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

            var filters = new EventFiltersModel("90", DateTime.UtcNow.AddDays(4), DateTime.UtcNow.AddYears(1), null, EventCategories.Music, Page: 1, PageSize: 10);
            Assert.Single(eventRepository.GetAllEventsWithFilters(
                filters.Page,
                filters.PageSize,
                filters.Name,
                filters.DateFrom,
                filters.DateTo,
                filters.Location,
                filters.Category)
            );
        }

        [Fact(DisplayName = "Создание корректного события")]
        public async Task CreateEvent()
        {
            var db = CreateDatabase();
            var eventRepository = new EventRepository(db);
            var creatingEvent = new BLL.DTO.CreateEventDTO
            {
                Name = "Игра в футбол",
                Description = "Надо иметь разряд по футболу",
                EventDateTime = DateTime.UtcNow.AddMonths(1),
                Location = "Стадион Динамо",
                Category = EventCategories.Sports,
                MaximumParticipants = 22,
                Image = null
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<EventToEventDtoMapper>());
            var mapper = config.CreateMapper();

            var creatingEventValidator = new CreateEventDTOValidator();
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
            var creatingEvent = new BLL.DTO.CreateEventDTO
            {
                Name = "Игра в футбол",
                Description = "Надо иметь разряд по футболу",
                EventDateTime = DateTime.UtcNow.AddMonths(1),
                Location = "Стадион Динамо",
                Category = EventCategories.Unspecified,
                MaximumParticipants = 22,
                Image = null
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile<EventDtoToEventViewModelMapper>());
            var mapper = config.CreateMapper();

            var creatingEventValidator = new CreateEventDTOValidator();
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