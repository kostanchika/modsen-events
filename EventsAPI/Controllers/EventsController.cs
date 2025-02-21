using AutoMapper;
using EventsAPI.Models;
using EventsAPI.Repository;
using EventsAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllEvents([FromQuery] GetEventsModel request)
        {
            return Ok(
                    new { 
                        Events = _eventRepository.GetAllEventsWithFilters(request),
                        Pages = _eventRepository.GetTotalPages(request.PageSize) 
                    }
                );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleEvent(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return BadRequest("Ќовое максимальное количество участников " +
                    "не может быть больше текущего количества");
            }

            if (updatingEvent.Image != null && updatingEvent.Image.Length > 0)
            {
                eventItem.ImagePath = await _imageService.UploadImageAsync(updatingEvent.Image);
            }

            await _eventService.UpdateEvent(eventItem, updatingEvent);

            return Ok();
        }

        [HttpPut("{id}/register")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

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

            try
            {
                await _eventService.RegisterUserForEvent(eventItem, login);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{id}/unregister")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            try
            {
                await _eventService.UnregisterUserFromEvent(eventItem, login);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("my")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            return Ok(userWithEvents.Events);
        }
    }
}
