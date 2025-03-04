using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Mappers;
using EventsAPI.BLL.Services;
using EventsAPI.DAL.Data;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Repositories;
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
                new User { Name = "Dmitry", LastName = "Kudlacky", BirthDateTime = DateTime.Now.AddYears(-20), Email = "dm1try@gmail.com", Events = [], Login = "dmitry", PasswordHash="_", RefreshToken = "_", RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7), Role = "User"  },
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

        public static EventsService CreateService()
        {
            var db = CreateDatabase();
            var userRepository = new UserRepository(db);
            var eventRepository = new EventRepository(db);

            var emailSender = new EmailSender(null);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EventToEventDtoMapper>();
                cfg.AddProfile<UserToUserDtoMapper>();
            });
            var mapper = config.CreateMapper();

            return new EventsService(
                userRepository,
                eventRepository,
                null,
                null,
                emailSender,
                null,
                mapper
            );
        }

        public static string Login => "kostanchik";
        public static string Login2 => "dmitry";

        [Fact(DisplayName = "Пользователь должен зарегистрироваться на событие")]
        public async Task RegisterOnEvent()
        {
            var eventsService = CreateService();

            try
            {
                await eventsService.RegisterUserForEventAsync(1, Login);
            } catch { }


            var eventItem = await eventsService.GetEventAsync(1);

            Assert.Equal(1, eventItem.CurrentParticipants);
        }

        [Fact(DisplayName = "Пользователь должен зарегистрироваться на событие только один раз")]
        public async Task RegisterOnEventOnlyOneTime()
        {
            var eventsService = CreateService();

            try
            {
                await eventsService.RegisterUserForEventAsync(1, Login);
            } catch { }
            try
            {
                await eventsService.RegisterUserForEventAsync(1, Login);
            } catch { }

            var eventItem = await eventsService.GetEventAsync(1);

            Assert.Equal(1, eventItem.CurrentParticipants);
        }

        [Fact(DisplayName = "Пользователь должен зарегистрироваться и отписаться от события")]
        public async Task RegisterAndUnregisterEvent()
        {
            var eventsService = CreateService();
                       
            await eventsService.RegisterUserForEventAsync(1, Login);

            await eventsService.UnregisterUserFromEventAsync(1, Login);
            
            var eventItem = await eventsService.GetEventAsync(1);

            Assert.Equal(0, eventItem.CurrentParticipants);
        }

        [Fact(DisplayName = "Пользователь не должен подписаться на просроченное событие")]
        public async Task NotRegisterOnExpiredEvent()
        {
            var eventsService = CreateService();

            try
            {
                await eventsService.RegisterUserForEventAsync(2, Login);
            }
            catch { }

            var eventItem = await eventsService.GetEventAsync(1);

            Assert.Equal(0, eventItem.CurrentParticipants);
        }

        [Fact(DisplayName = "Вывод информации о участниках события")]
        public async Task GetEventParticipants()
        {
            var eventsService = CreateService();

            await eventsService.RegisterUserForEventAsync(1, Login);
            await eventsService.RegisterUserForEventAsync(1, Login2);

            var participants = await eventsService.GetEventParticipantsAsync(1);

            Assert.Equal(2, participants.Count());
        }

        [Fact(DisplayName = "Получение информации об участнике события")]
        public async Task GetEventParticipant()
        {
            var eventsService = CreateService();

            await eventsService.RegisterUserForEventAsync(1, Login);
            await eventsService.RegisterUserForEventAsync(1, Login2);

            var participant = await eventsService.GetEventParticipantAsync(1, 2);

            Assert.Equal(Login2, participant.Login);
        }

        [Fact(DisplayName = "Получение информации о несуществующем участнике события")]
        public async Task GetEventNullParticipant()
        {
            var eventsService = CreateService();

            await eventsService.RegisterUserForEventAsync(1, Login);

            UserDTO? participant = null;
            try
            {
                participant = await eventsService.GetEventParticipantAsync(1, 2);
            } catch { }

            Assert.Null(participant);
        }

        [Fact(DisplayName = "Получение информации о участнике несуществующего события")]
        public async Task GetNullEventParticipant()
        {
            var eventsService = CreateService();

            await eventsService.RegisterUserForEventAsync(1, Login);

            UserDTO? participant = null;
            try
            {
                participant = await eventsService.GetEventParticipantAsync(3, 1);
            }
            catch { }

            Assert.Null(participant);
        }
    }
}
