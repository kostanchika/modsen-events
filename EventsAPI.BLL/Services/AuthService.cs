using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.DAL.Entities;
using EventsAPI.DAL.Interfaces;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EventsAPI.BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterDTO> _registeringUserValidator;
        private readonly IValidator<LoginDTO> _loginingUserValidator;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IValidator<RegisterDTO> registeringUserValidator,
            IValidator<LoginDTO> loginingUserValidator,
            TokenService tokenService,
            PasswordService passwordService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _registeringUserValidator = registeringUserValidator;
            _loginingUserValidator = loginingUserValidator;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<TokensDTO> RegisterAsync(RegisterDTO registeringUser)
        {
            var validationResult = await _registeringUserValidator.ValidateAsync(registeringUser);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
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
                throw new InvalidOperationException("Логин или почта уже занят(ы)");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            return new TokensDTO(accessToken, refreshToken.RefreshToken);
        }


        public async Task<TokensDTO> LoginAsync(LoginDTO logginingInUser)
        {
            var validationResult = await _loginingUserValidator.ValidateAsync(logginingInUser);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByLoginAsync(logginingInUser.Login);
            if (user == null)
            {
                throw new NullReferenceException("Пользователь не найден");
            }

            if (!_passwordService.ValidatePassword(logginingInUser.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Неверный пароль");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;
            await _userRepository.UpdateAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user);

            return new TokensDTO(accessToken, refreshToken.RefreshToken);
        }

        public async Task<TokensDTO> RefreshAsync(TokensDTO tokens)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _tokenService.GetPrincipalFromToken(tokens.AccessToken);
            }
            catch
            {
                throw new SecurityTokenException("Неверный access-token");
            }

            var login = principal.Identity?.Name;
            if (login == null)
            {
                throw new SecurityTokenException("Неверный access-token");
            }

            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
            {
                throw new NullReferenceException("Пользователь не найден");
            }

            if (user.RefreshToken != tokens.RefreshToken || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Неверный refresh-token");
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiresAt = refreshToken.ExpiresAt;

            await _userRepository.UpdateAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user);
            return new TokensDTO(accessToken, refreshToken.RefreshToken);
        }
    }
}
