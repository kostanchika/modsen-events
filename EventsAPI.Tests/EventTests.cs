using EventsAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Tests
{
    public class EventTests
    {
        public static ApplicationContext CreateDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            List<User> users = [
                new User { Name = "Konstantin", LastName = "Baranchuk", BirthDateTime = DateTime.Now.AddYears(-19), Email = "kos5anchik@gmail.com", Events = [], Login = "kostanchik", PasswordHash="_", RefreshToken = "_", RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7), Role = "Admin"  },
                ];
            List<Event> events = [
                    new Event { Name = "Вечеринка у бассейна", EventDateTime = DateTime.UtcNow.AddDays(1), Location = "Бассейн у Влада", MaximumParticipants = 10, Participants = [], Category = EventCategories.Health, Description = "Поплаваем, отдохнём", ImagePath="/images/default.png" },
                    new Event { Name = "Дискотека 90-х", EventDateTime = DateTime.UtcNow.AddDays(-1), Location = "Ретро клуб", MaximumParticipants = 50, Participants = [], Category = EventCategories.Music, Description = "Танцуют все!", ImagePath="/images/default.png" },
                ];

            var context = new ApplicationContext(options);
            context.Set<User>().AddRange(users);
            context.Set<Event>().AddRange(events);
            context.SaveChanges();

            return context;
        }

        public static string Login => "kostanchik";

        [Fact(DisplayName = "Пользователь должен зарегистрироваться на событие")]
        public async Task RegisterOnEvent()
        {
            var db = CreateDatabase();
            var userRepository = new UserRepository(db);
            var eventRepository = new EventRepository(db);

            var eventItem = await eventRepository.GetByIdAsync(1);

            var emailSender = new EmailSender(null);

            var eventsService = new EventsService(userRepository, eventRepository, emailSender);

            if (!eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.Participants.Count < eventItem.MaximumParticipants)
                {
                    if (eventItem.EventDateTime >= DateTime.UtcNow)
                    {
                        await eventsService.RegisterUserForEventAsync(eventItem, Login);
                    }
                }
            }

            eventItem = await eventRepository.GetByIdAsync(1);

            Assert.Equal(1, eventItem.Participants.Count);
        }

        [Fact(DisplayName = "Пользователь должен зарегистрироваться на событие только один раз")]
        public async Task RegisterOnEventOnlyOneTime()
        {
            var db = CreateDatabase();
            var userRepository = new UserRepository(db);
            var eventRepository = new EventRepository(db);

            var eventItem = await eventRepository.GetByIdAsync(1);

            var emailSender = new EmailSender(null);

            var eventsService = new EventsService(userRepository, eventRepository, emailSender);

            if (!eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.Participants.Count < eventItem.MaximumParticipants)
                {
                    if (eventItem.EventDateTime >= DateTime.UtcNow)
                    {
                        await eventsService.RegisterUserForEventAsync(eventItem, Login);
                    }
                }
            }

            eventItem = await eventRepository.GetByIdAsync(1);

            if (!eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.Participants.Count < eventItem.MaximumParticipants)
                {
                    if (eventItem.EventDateTime >= DateTime.UtcNow)
                    {
                        await eventsService.RegisterUserForEventAsync(eventItem, Login);
                    }
                }
            }

            eventItem = await eventRepository.GetByIdAsync(1);

            Assert.Equal(1, eventItem.Participants.Count);
        }

        [Fact(DisplayName = "Пользователь должен зарегистрироваться и отписаться от события")]
        public async Task RegisterAndUnregisterEvent()
        {
            var db = CreateDatabase();
            var userRepository = new UserRepository(db);
            var eventRepository = new EventRepository(db);

            var eventItem = await eventRepository.GetByIdAsync(1);

            var emailSender = new EmailSender(null);

            var eventsService = new EventsService(userRepository, eventRepository, emailSender);

            if (!eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.Participants.Count < eventItem.MaximumParticipants)
                {
                    if (eventItem.EventDateTime >= DateTime.UtcNow)
                    {
                        await eventsService.RegisterUserForEventAsync(eventItem, Login);
                    }
                }
            }

            if (eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.EventDateTime >= DateTime.UtcNow)
                {
                    await eventsService.UnregisterUserFromEventAsync(eventItem, Login);
                }
            }

            eventItem = await eventRepository.GetByIdAsync(1);

            Assert.Equal(0, eventItem.Participants.Count);
        }

        [Fact(DisplayName = "Список событий пользователя должен вернуть 1 событие")]
        public async Task GetUserEvents()
        {
            var db = CreateDatabase();
            var userRepository = new UserRepository(db);
            var eventRepository = new EventRepository(db);

            var eventItem = await eventRepository.GetByIdAsync(1);

            var emailSender = new EmailSender(null);

            var eventsService = new EventsService(userRepository, eventRepository, emailSender);

            if (!eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.Participants.Count < eventItem.MaximumParticipants)
                {
                    if (eventItem.EventDateTime >= DateTime.UtcNow)
                    {
                        await eventsService.RegisterUserForEventAsync(eventItem, Login);
                    }
                }
            }

            var userEvents = await userRepository.GetByLoginIncludeEventsAsync(Login);

            Assert.Equal(1, userEvents.Events.Count);
        }

        [Fact(DisplayName = "Пользователь не должен подписаться на просроченное событие")]
        public async Task NotRegisterOnExpiredEvent()
        {
            var db = CreateDatabase();
            var userRepository = new UserRepository(db);
            var eventRepository = new EventRepository(db);

            var eventItem = await eventRepository.GetByIdAsync(2);

            var emailSender = new EmailSender(null);

            var eventsService = new EventsService(userRepository, eventRepository, emailSender);

            if (!eventItem.Participants.Select(u => u.Login).Contains(Login))
            {
                if (eventItem.Participants.Count < eventItem.MaximumParticipants)
                {
                    if (eventItem.EventDateTime >= DateTime.UtcNow)
                    {
                        await eventsService.RegisterUserForEventAsync(eventItem, Login);
                    }
                }
            }

            eventItem = await eventRepository.GetByIdAsync(1);

            Assert.Equal(0, eventItem.Participants.Count);
        }
    }
}
