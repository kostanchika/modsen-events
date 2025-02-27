using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Models;
using EventsAPI.BLL.Services;
using EventsAPI.Models;
using EventsAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EventsService _eventsService;
        public EventsController(IMapper mapper, EventsService eventsService)
        {
            _mapper = mapper;
            _eventsService = eventsService;
        }

        [HttpGet]
        public IEnumerable<EventViewModel> GetAllEvents([FromQuery] EventFiltersModel filters)
        {
            Response.Headers["X-Page-Count"] = _eventsService.GetTotalPages(filters).ToString();

            return _eventsService.GetAllEvents(filters).Select(_mapper.Map<EventViewModel>);
        }

        [HttpGet("{id}")]
        public async Task<EventViewModel> GetSingleEvent(int id)
        {
            var eventItem = await _eventsService.GetEventAsync(id);

            return _mapper.Map<EventViewModel>(eventItem);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventModel creatingEvent)
        {
            var eventDTO = _mapper.Map<CreateEventDTO>(creatingEvent);
            var eventItem = await _eventsService.CreateEventAsync(eventDTO);
            var eventViewModel = _mapper.Map<EventViewModel>(eventItem);

            return CreatedAtAction(nameof(GetSingleEvent), new { id = eventItem.Id }, eventViewModel);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ChangeEvent(int id, [FromForm] ChangeEventModel updatingEvent)
        {
            var eventDTO = _mapper.Map<ChangeEventDTO>(updatingEvent);
            await _eventsService.ChangeEventAsync(id, eventDTO);

            return Ok();
        }

        [HttpPut("{id}/register")]
        [Authorize]
        public async Task<IActionResult> RegisterForEvent(int id)
        {
            var login = User?.Identity?.Name;

            await _eventsService.RegisterUserForEventAsync(id, login);

            return Ok();
        }

        [HttpPut("{id}/unregister")]
        [Authorize]
        public async Task<IActionResult> UnregisterForEvent(int id)
        {
            var login = User?.Identity?.Name;

            await _eventsService.UnregisterUserFromEventAsync(id, login);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventsService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IEnumerable<EventViewModel>> GetUserEvents()
        {
            var login = User?.Identity?.Name;

            var events = await _eventsService.GetUserEvents(login);

            return events.Select(_mapper.Map<EventViewModel>);
        }

        [HttpGet("{id}/participants")]
        public async Task<IEnumerable<UserViewModel>> GetEventParticipants(int id)
        {
            var participants = await _eventsService.GetEventParticipants(id);

            return participants.Select(_mapper.Map<UserViewModel>);
        }

        [HttpGet("{id}/participants/{participantId}")]
        public async Task<UserViewModel> GetEventParticipant(int id, int participantId)
        {
            var participant = await _eventsService.GetEventParticipant(id, participantId);

            return _mapper.Map<UserViewModel>(participant);
        }
    }
}
