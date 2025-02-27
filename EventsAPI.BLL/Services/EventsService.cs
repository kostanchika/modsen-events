using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Interfaces;
using EventsAPI.BLL.Models;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using FluentValidation;

namespace EventsAPI.BLL.Services
{
    public class EventsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly EmailSender _emailSender;
        private readonly IValidator<CreateEventDTO> _creatingEventValidator;
        private readonly IValidator<ChangeEventDTO> _changingEventValidator;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public EventsService(
            IUserRepository userRepository,
            IEventRepository eventRepository,
            EmailSender emailSender,
            ImageService imageService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
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

        public int GetTotalPages(EventFiltersModel filters)
        {
            return _eventRepository.GetTotalPages(
                filters.PageSize,
                filters.Name,
                filters.DateFrom,
                filters.DateTo,
                filters.Location,
                filters.Category
            );
        }

        public async Task<EventDTO> GetEventAsync(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id)
                             ?? throw new NullReferenceException("Событие не найдено");

            return _mapper.Map<EventDTO>(eventItem);
        }

        public async Task<EventDTO> CreateEventAsync(CreateEventDTO creatingEvent)
        {
            var validationResult = await _creatingEventValidator.ValidateAsync(creatingEvent);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var eventItem = _mapper.Map<Event>(creatingEvent);

            await UploadImageAsync(eventItem, creatingEvent.Image);

            await _eventRepository.AddAsync(eventItem);

            var eventDTO = _mapper.Map<EventDTO>(eventItem);

            return eventDTO;
        }

        public async Task<EventDTO> ChangeEventAsync(int id, ChangeEventDTO newData)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id)
                             ?? throw new NullReferenceException("Событие не найдено");

            var validationResult = await _changingEventValidator.ValidateAsync(newData);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (newData.MaximumParticipants < eventItem.Participants.Count)
            {
                throw new InvalidOperationException("Новое максимальное количество участников не может быть больше текущего количества");
            }

            await UploadImageAsync(eventItem, newData.Image);

            await UpdateEventAsync(eventItem, newData);

            return _mapper.Map<EventDTO>(eventItem);
        }

        public async Task UpdateEventAsync(Event eventItem, ChangeEventDTO newData)
        {
            bool isKeyFieldsChanged = eventItem.Location != newData.Location ||
                                      eventItem.EventDateTime != newData.EventDateTime;

            eventItem.Description = newData.Description;
            eventItem.EventDateTime = newData.EventDateTime;
            eventItem.Location = newData.Location;
            eventItem.Category = newData.Category;
            eventItem.MaximumParticipants = newData.MaximumParticipants;

            await _eventRepository.UpdateAsync(eventItem);

            if (isKeyFieldsChanged)
            {
                await NotifyParticipantsAsync(eventItem);
            }
        }

        public async Task RegisterUserForEventAsync(int eventId, string? userLogin)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId)
                ?? throw new NullReferenceException("Не удалось найти событие");

            if (userLogin == null)
            {
                throw new NullReferenceException("Не удалось найти пользователя с данным логином");
            }

            var user = await _userRepository.GetByLoginAsync(userLogin)
                        ?? throw new NullReferenceException("Не удалось найти пользователя с данным логином");

            eventItem.Participants.Add(user);
            user.Events.Add(eventItem);

            await _eventRepository.UpdateAsync(eventItem);
            await _userRepository.UpdateAsync(user);
        }

        public async Task UnregisterUserFromEventAsync(int eventId, string? userLogin)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId)
                ?? throw new NullReferenceException("Не удалось найти событие");

            if (userLogin == null)
            {
                throw new NullReferenceException("Не удалось найти пользователя с данным логином");
            }

            var user = eventItem.Participants.FirstOrDefault(p => p.Login == userLogin)
                        ?? throw new NullReferenceException("Не удалось найти пользователя с данным логином");

            eventItem.Participants.Remove(user);
            user.Events.Remove(eventItem);

            await _eventRepository.UpdateAsync(eventItem);
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int eventId)
        {
            await _eventRepository.DeleteAsync(eventId);
        }

        public async Task<IEnumerable<EventDTO>> GetUserEvents(string? login)
        {
            if (login == null)
            {
                throw new NullReferenceException("Пользователь с таким логином не найден");
            }

            var userWithEvents = await _userRepository.GetByLoginIncludeEventsAsync(login);
            if (userWithEvents == null)
            {
                throw new Exception("Не удалось получить список событий пользователя");
            }

            return userWithEvents.Events.Select(_mapper.Map<EventDTO>);
        }

        public async Task<IEnumerable<UserDTO>> GetEventParticipants(int eventId)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
            if (eventItem == null)
            {
                throw new NullReferenceException("Не удалось найти событие");
            }

            return eventItem.Participants.Select(_mapper.Map<UserDTO>);
        }

        public async Task<UserDTO> GetEventParticipant(int eventId, int participantId)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
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

        private async Task UploadImageAsync(Event eventItem, IImageFile? image)
        {
            if (image != null && image.Length > 0)
            {
                eventItem.ImagePath = await _imageService.UploadImageAsync(image);
            }
        }

        private async Task NotifyParticipantsAsync(Event eventItem)
        {
            var message = $"Новая информация: {eventItem.Location}, {eventItem.EventDateTime:dd.MM.yyyy HH:mm}";
            foreach (var participant in eventItem.Participants)
            {
                await _emailSender.SendEmailAsync(
                    participant.Email,
                    $"Информация о месте проведения события {eventItem.Name} была обновлена",
                    message
                );
            }
        }
    }
}
