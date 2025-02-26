using AutoMapper;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using EventsAPI.Models;
using EventsAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterModel> _registeringUserValidator;
        private readonly IValidator<LoginModel> _loginingUserValidator;
        private readonly TokenService _tokenService;

        public AuthController(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<RegisterModel> registeringUserValidator,
            IValidator<LoginModel> loginingUserValidator,
            TokenService tokenService
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _registeringUserValidator = registeringUserValidator;
            _loginingUserValidator = loginingUserValidator;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registeringUser)
        {
            var validationResult = await _registeringUserValidator.ValidateAsync(registeringUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = _mapper.Map<User>(registeringUser);

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;

            try
            {
                user.BirthDateTime = user.BirthDateTime.ToUniversalTime();
                await _userRepository.AddAsync(user);
            }
            catch
            {
                return Conflict("Логин или почта уже занят(ы)");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            return Ok(new { accessToken, refreshToken.RefreshToken });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel logginingInUser)
        {
            var validationResult = await _loginingUserValidator.ValidateAsync(logginingInUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userRepository.GetByLoginAsync(logginingInUser.Login);
            if (user == null)
            {
                return NotFound("Пользователь с данным логином не найден");
            }

            if (!BCrypt.Net.BCrypt.Verify(logginingInUser.Password, user.PasswordHash))
            {
                return BadRequest("Неверный пароль");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;
            await _userRepository.UpdateAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user);

            return Ok(new { accessToken, refreshToken.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRequest tokenRequest)
        {
            ClaimsPrincipal? principal;
            try
            {
                principal = _tokenService.GetPrincipalFromToken(tokenRequest.AccessToken);
            }
            catch
            {
                return Unauthorized("Неверный access-токен");
            }

            var login = principal.Identity?.Name;
            if (login == null)
            {
                return Unauthorized("Неверный access-токен");
            }

            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
            {
                return NotFound("Не удалось найти пользователя");
            }

            if (user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized("Неверный refresh-токен");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;

            await _userRepository.UpdateAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user);
            return Ok(new { accessToken, refreshToken.RefreshToken });
        }
    }
}
