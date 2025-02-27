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
        public async Task<IActionResult> Register(RegisterModel registeringUser)
        {
            var registerDTO = _mapper.Map<RegisterDTO>(registeringUser);
            var tokens = await _authService.RegisterAsync(registerDTO);

            return Ok(new { tokens.AccessToken, tokens.RefreshToken });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel logginingInUser)
        {
            var loginDTO = _mapper.Map<LoginDTO>(logginingInUser);
            var tokens = await _authService.LoginAsync(loginDTO);

            return Ok(new { tokens.AccessToken, tokens.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokensModel tokensRequest)
        {
            var tokensDTO = _mapper.Map<TokensDTO>(tokensRequest);
            var tokens = await _authService.RefreshAsync(tokensDTO);

            return Ok(new { tokens.AccessToken, tokens.RefreshToken });
        }
    }
}
