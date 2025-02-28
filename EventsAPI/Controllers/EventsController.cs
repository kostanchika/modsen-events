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
        public async Task<IEnumerable<EventViewModel>> GetAllEvents([FromQuery] EventFiltersModel filters, CancellationToken ct)
        {
            Response.Headers["X-Page-Count"] = (await _eventsService.GetTotalPagesAsync(filters, ct)).ToString();

            return _eventsService.GetAllEvents(filters).Select(_mapper.Map<EventViewModel>);
        }

        [HttpGet("{id}")]
        public async Task<EventViewModel> GetSingleEvent(int id, CancellationToken ct)
        {
            var eventItem = await _eventsService.GetEventAsync(id, ct);

            return _mapper.Map<EventViewModel>(eventItem);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventModel creatingEvent, CancellationToken ct)
        {
            var eventDTO = _mapper.Map<CreateEventDTO>(creatingEvent);
            var eventItem = await _eventsService.CreateEventAsync(eventDTO, ct);
            var eventViewModel = _mapper.Map<EventViewModel>(eventItem);

            return CreatedAtAction(nameof(GetSingleEvent), new { id = eventItem.Id }, eventViewModel);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ChangeEvent(int id, [FromForm] ChangeEventModel updatingEvent, CancellationToken ct)
        {
            var eventDTO = _mapper.Map<ChangeEventDTO>(updatingEvent);
            await _eventsService.ChangeEventAsync(id, eventDTO, ct);

            return Ok();
        }

        [HttpPut("{id}/register")]
        [Authorize]
        public async Task<IActionResult> RegisterForEvent(int id, CancellationToken ct)
        {
            var login = User?.Identity?.Name;

            await _eventsService.RegisterUserForEventAsync(id, login, ct);

            return Ok();
        }

        [HttpPut("{id}/unregister")]
        [Authorize]
        public async Task<IActionResult> UnregisterForEvent(int id, CancellationToken ct)
        {
            var login = User?.Identity?.Name;

            await _eventsService.UnregisterUserFromEventAsync(id, login, ct);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _eventsService.DeleteAsync(id, ct);

            return NoContent();
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IEnumerable<EventViewModel>> GetUserEvents(CancellationToken ct)
        {
            var login = User?.Identity?.Name;

            var events = await _eventsService.GetUserEventsAsync(login, ct);

            return events.Select(_mapper.Map<EventViewModel>);
        }

        [HttpGet("{id}/participants")]
        public async Task<IEnumerable<UserViewModel>> GetEventParticipants(int id, CancellationToken ct)
        {
            var participants = await _eventsService.GetEventParticipantsAsync(id, ct);

            return participants.Select(_mapper.Map<UserViewModel>);
        }

        [HttpGet("{id}/participants/{participantId}")]
        public async Task<UserViewModel> GetEventParticipant(int id, int participantId, CancellationToken ct)
        {
            var participant = await _eventsService.GetEventParticipantAsync(id, participantId, ct);

            return _mapper.Map<UserViewModel>(participant);
        }
    }
}
