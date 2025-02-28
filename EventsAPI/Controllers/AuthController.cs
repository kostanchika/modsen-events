using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Services;
using EventsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registeringUser, CancellationToken ct)
        {
            var registerDTO = _mapper.Map<RegisterDTO>(registeringUser);
            var tokens = await _authService.RegisterAsync(registerDTO, ct);

            return Ok(new { tokens.AccessToken, tokens.RefreshToken });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel logginingInUser, CancellationToken ct)
        {
            var loginDTO = _mapper.Map<LoginDTO>(logginingInUser);
            var tokens = await _authService.LoginAsync(loginDTO, ct);

            return Ok(new { tokens.AccessToken, tokens.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokensModel tokensRequest, CancellationToken ct)
        {
            var tokensDTO = _mapper.Map<TokensDTO>(tokensRequest);
            var tokens = await _authService.RefreshAsync(tokensDTO, ct);

            return Ok(new { tokens.AccessToken, tokens.RefreshToken });
        }
    }
}
