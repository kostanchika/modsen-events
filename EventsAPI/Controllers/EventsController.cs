using AutoMapper;
using EventsAPI.Models;
using EventsAPI.Repository;
using EventsAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace EventsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEventModel> _creatingEventValidator;
        private readonly IValidator<ChangeEventModel> _changingEventValidator;
        private readonly ImageService _imageService;
        private readonly EventsService _eventService;
        public EventsController(
            IEventRepository eventRepository, 
            IUserRepository userRepository,
            IMapper mapper, 
            IValidator<CreateEventModel> creatingEventValidator,
            IValidator<ChangeEventModel> changingEventValidator,
            ImageService imageService,
            EventsService eventService
        )
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _creatingEventValidator = creatingEventValidator;
            _changingEventValidator = changingEventValidator;
            _imageService = imageService;
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult GetAllEvents([FromQuery] GetEventsModel request)
        {
            Response.Headers["X-Page-Count"] = _eventRepository.GetTotalPages(request.PageSize).ToString();
            var events = _eventRepository.GetAllEventsWithFilters(request);
            var eventsResponse = events.Select(_mapper.Map<GetEventsResponse>);
            return Ok(eventsResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleEvent(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetEventsResponse>(eventItem));
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventModel creatingEvent)
        {
            var validationResult = await _creatingEventValidator.ValidateAsync(creatingEvent);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var eventItem = _mapper.Map<Event>(creatingEvent);
            if (eventItem.Category == EventCategories.Unspecified)
            {
                return BadRequest("Ќельз€ создать событие без категории");
            }
            if (creatingEvent.Image != null && creatingEvent.Image.Length > 0)
            {
                eventItem.ImagePath = await _imageService.UploadImageAsync(creatingEvent.Image);
            }

            await _eventRepository.AddAsync(eventItem);

            return CreatedAtAction(nameof(GetSingleEvent), new { id = eventItem.Id }, eventItem);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ChangeEvent(int id, [FromForm] ChangeEventModel updatingEvent)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var validationResult = await _changingEventValidator.ValidateAsync(updatingEvent);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (updatingEvent.Category == EventCategories.Unspecified)
            {
                return BadRequest("Ќельз€ создать событие без категории");
            }

            if (updatingEvent.MaximumParticipants < eventItem.Participants.Count)
            {
                return Conflict("Ќовое максимальное количество участников " +
                    "не может быть больше текущего количества");
            }

            if (updatingEvent.Image != null && updatingEvent.Image.Length > 0)
            {
                eventItem.ImagePath = await _imageService.UploadImageAsync(updatingEvent.Image);
            }

            await _eventService.UpdateEventAsync(eventItem, updatingEvent);

            return Ok();
        }

        [HttpPut("{id}/register")]
        [Authorize]
        public async Task<IActionResult> RegisterForEvent(int id)
        {
            var login = User?.Identity?.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            if (eventItem.Participants.Select(u => u.Login).Contains(login))
            {
                return BadRequest("¬ы уже зарегистрированы в данном событии");
            }

            if (eventItem.Participants.Count >= eventItem.MaximumParticipants)
            {
                return BadRequest("—вободные места на данное событие закончились");
            }

            if (eventItem.EventDateTime < DateTime.UtcNow)
            {
                return Conflict("—обытие уже завершено");
            }

            try
            {
                await _eventService.RegisterUserForEventAsync(eventItem, login);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{id}/unregister")]
        [Authorize]
        public async Task<IActionResult> UnregisterForEvent(int id)
        {
            var login = User?.Identity?.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            if (!eventItem.Participants.Select(u => u.Login).Contains(login))
            {
                return BadRequest("¬ы прежде не были зарегистрированы в данном событии");
            }

            if (eventItem.EventDateTime < DateTime.UtcNow)
            {
                return Conflict("—обытие уже завершено");
            }

            try
            {
                await _eventService.UnregisterUserFromEventAsync(eventItem, login);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetUserEvents()
        {
            var login = User?.Identity?.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var userWithEvents = await _userRepository.GetByLoginIncludeEventsAsync(login);
            if (userWithEvents == null)
            {
                return NotFound();
            }

            return Ok(userWithEvents.Events.Select(_mapper.Map<GetEventsResponse>));
        }
    }
}
