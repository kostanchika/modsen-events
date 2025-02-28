using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Interfaces;
using EventsAPI.BLL.Models;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.BLL.Services
{
    public class EventsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<CreateEventDTO> _creatingEventValidator;
        private readonly IValidator<ChangeEventDTO> _changingEventValidator;
        private readonly EmailSender _emailSender;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public EventsService(
            IUserRepository userRepository,
            IEventRepository eventRepository,
            IValidator<CreateEventDTO> creatingEventValidator,
            IValidator<ChangeEventDTO> changingEventValidator,
            EmailSender emailSender,
            ImageService imageService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _creatingEventValidator = creatingEventValidator;
            _changingEventValidator = changingEventValidator;
            _emailSender = emailSender;
            _imageService = imageService;
            _mapper = mapper;
        }

        public IEnumerable<EventDTO> GetAllEvents(EventFiltersModel filters)
        {
            var events = _eventRepository.GetAllEventsWithFilters(
                filters.Page,
                filters.PageSize,
                filters.Name,
                filters.DateFrom,
                filters.DateTo,
                filters.Location,
                filters.Category
            );

            return events.Select(_mapper.Map<EventDTO>);
        }

        public async Task<int> GetTotalPagesAsync(EventFiltersModel filters, CancellationToken ct = default)
        {
            return await _eventRepository.GetTotalPagesAsync(
                filters.PageSize,
                filters.Name,
                filters.DateFrom,
                filters.DateTo,
                filters.Location,
                filters.Category,
                ct
            );
        }

        public async Task<EventDTO> GetEventAsync(int id, CancellationToken ct = default)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id, ct)
                             ?? throw new NullReferenceException("Событие не найдено");

            return _mapper.Map<EventDTO>(eventItem);
        }

        public async Task<EventDTO> CreateEventAsync(CreateEventDTO creatingEvent, CancellationToken ct = default)
        {
            var validationResult = await _creatingEventValidator.ValidateAsync(creatingEvent, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var eventItem = _mapper.Map<Event>(creatingEvent);

            await UploadImageAsync(eventItem, creatingEvent.Image, ct);

            await _eventRepository.AddAsync(eventItem, ct);

            var eventDTO = _mapper.Map<EventDTO>(eventItem);

            return eventDTO;
        }

        public async Task<EventDTO> ChangeEventAsync(int id, ChangeEventDTO newData, CancellationToken ct = default)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id, ct)
                             ?? throw new NullReferenceException("Событие не найдено");

            var validationResult = await _changingEventValidator.ValidateAsync(newData, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (newData.MaximumParticipants < eventItem.Participants.Count)
            {
                throw new InvalidOperationException(
                    "Новое максимальное количество участников не может быть больше текущего количества"
                );
            }

            await UploadImageAsync(eventItem, newData.Image, ct);

            await UpdateEventAsync(eventItem, newData, ct);

            return _mapper.Map<EventDTO>(eventItem);
        }

        public async Task UpdateEventAsync(Event eventItem, ChangeEventDTO newData, CancellationToken ct = default)
        {
            bool isKeyFieldsChanged = eventItem.Location != newData.Location ||
                                      eventItem.EventDateTime != newData.EventDateTime;

            eventItem.Description = newData.Description;
            eventItem.EventDateTime = newData.EventDateTime;
            eventItem.Location = newData.Location;
            eventItem.Category = newData.Category;
            eventItem.MaximumParticipants = newData.MaximumParticipants;

            await _eventRepository.UpdateAsync(eventItem, ct);

            if (isKeyFieldsChanged)
            {
                await NotifyParticipantsAsync(eventItem, ct);
            }
        }

        public async Task RegisterUserForEventAsync(int eventId, string? userLogin, CancellationToken ct = default)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId, ct)
                ?? throw new NullReferenceException("Не удалось найти событие");

            if (userLogin == null)
            {
                throw new NullReferenceException("Не удалось найти пользователя с данным логином");
            }

            var user = await _userRepository.GetByLoginAsync(userLogin, ct)
                        ?? throw new NullReferenceException("Не удалось найти пользователя с данным логином");


            eventItem.Participants.Add(user);
            user.Events.Add(eventItem);

            await _eventRepository.UpdateAsync(eventItem, ct);
            await _userRepository.UpdateAsync(user, ct);
        }

        public async Task UnregisterUserFromEventAsync(int eventId, string? userLogin, CancellationToken ct = default)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId, ct)
                ?? throw new NullReferenceException("Не удалось найти событие");

            if (userLogin == null)
            {
                throw new NullReferenceException("Не удалось найти пользователя с данным логином");
            }

            var user = eventItem.Participants.FirstOrDefault(p => p.Login == userLogin)
                        ?? throw new NullReferenceException("Не удалось найти пользователя с данным логином");


            eventItem.Participants.Remove(user);
            user.Events.Remove(eventItem);

            await _eventRepository.UpdateAsync(eventItem, ct);
            await _userRepository.UpdateAsync(user, ct);
        }

        public async Task DeleteAsync(int eventId, CancellationToken ct = default)
        {
            await _eventRepository.DeleteAsync(eventId, ct);
        }

        public async Task<IEnumerable<EventDTO>> GetUserEventsAsync(string? login, CancellationToken ct = default)
        {
            if (login == null)
            {
                throw new NullReferenceException("Пользователь с таким логином не найден");
            }

            var userWithEvents = await _userRepository.GetByLoginIncludeEventsAsync(login, ct);
            if (userWithEvents == null)
            {
                throw new Exception("Не удалось получить список событий пользователя");
            }

            return userWithEvents.Events.Select(_mapper.Map<EventDTO>);
        }

        public async Task<IEnumerable<UserDTO>> GetEventParticipantsAsync(int eventId, CancellationToken ct = default)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId, ct);
            if (eventItem == null)
            {
                throw new NullReferenceException("Не удалось найти событие");
            }

            return eventItem.Participants.Select(_mapper.Map<UserDTO>);
        }

        public async Task<UserDTO> GetEventParticipantAsync(int eventId, int participantId, CancellationToken ct = default)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId, ct);
            if (eventItem == null)
            {
                throw new NullReferenceException("Не удалось найти событие");
            }

            var participant = eventItem.Participants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                throw new NullReferenceException("Не удалось найти участника события");
            }

            return _mapper.Map<UserDTO>(participant);
        }

        private async Task UploadImageAsync(Event eventItem, IImageFile? image, CancellationToken ct = default)
        {
            if (image != null && image.Length > 0)
            {
                eventItem.ImagePath = await _imageService.UploadImageAsync(image, ct);
            }
        }

        private async Task NotifyParticipantsAsync(Event eventItem, CancellationToken ct = default)
        {
            var message = $"Новая информация: {eventItem.Location}, {eventItem.EventDateTime:dd.MM.yyyy HH:mm}";
            foreach (var participant in eventItem.Participants)
            {
                await _emailSender.SendEmailAsync(
                    participant.Email,
                    $"Информация о месте проведения события {eventItem.Name} была обновлена",
                    message,
                    ct
                );
            }
        }
    }
}
