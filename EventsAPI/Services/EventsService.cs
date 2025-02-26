using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using EventsAPI.Models;

namespace EventsAPI.Services
{
    public class EventsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly EmailSender _emailSender;
        public EventsService(IUserRepository userRepository, IEventRepository eventRepository, EmailSender emailSender)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _emailSender = emailSender;
        }

        public async Task UpdateEventAsync(Event eventItem, ChangeEventModel newData)
        {
            bool isKeyFieldsChanged = false;
            if (
                eventItem.Location != newData.Location ||
                eventItem.EventDateTime != newData.EventDateTime
                )
            {
                isKeyFieldsChanged = true;
            }

            eventItem.Description = newData.Description;
            eventItem.EventDateTime = newData.EventDateTime;
            eventItem.Location = newData.Location;
            eventItem.Category = newData.Category;
            eventItem.MaximumParticipants = newData.MaximumParticipants;

            await _eventRepository.UpdateAsync(eventItem);

            if (isKeyFieldsChanged)
            {
                foreach (var participant in eventItem.Participants)
                {
                    await _emailSender.SendEmailAsync(
                        participant.Email,

                        $"Информация о месте проведения события {eventItem.Name} была обновлена",

                        $"Новая информация: {eventItem.Location}, " +
                        $"{eventItem.EventDateTime:dd.MM.yyyy HH:mm}"
                    );
                }
            }
        }

        public async Task RegisterUserForEventAsync(Event eventItem, string userLogin)
        {
            var user = await _userRepository.GetByLoginAsync(userLogin) ??
                throw new Exception("Не удалось найти пользователя с данным логином");
            eventItem.Participants.Add(user);
            user.Events.Add(eventItem);
            await _eventRepository.UpdateAsync(eventItem);
            await _userRepository.UpdateAsync(user);
        }

        public async Task UnregisterUserFromEventAsync(Event eventItem, string userLogin)
        {
            var user = eventItem.Participants.FirstOrDefault(p => p.Login == userLogin) ??
                throw new Exception("Не удалось найти пользователя с данным логином");
            eventItem.Participants.Remove(user);
            user.Events.Remove(eventItem);
            await _eventRepository.UpdateAsync(eventItem);
            await _userRepository.UpdateAsync(user);
        }
    }
}
