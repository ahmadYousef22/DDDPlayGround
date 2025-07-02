using AutoMapper;
using DDDPlayGround.Application.Authentication.Dtos;
using DDDPlayGround.Application.Authentication.JwtToken;
using DDDPlayGround.Domain.Base;
using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Domain.Enums;
using DDDPlayGround.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace DDDPlayGround.Application.Authentication
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        private readonly ILogger<AuthenticationAppService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IValidator<RegisterRequestDto> _registerValidator;

        public AuthenticationAppService(
            IUnitOfWork unitOfWork,
            ILogger<AuthenticationAppService> logger,
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenService jwtService,
            IMapper mapper,
            IValidator<RegisterRequestDto> registerValidator,
            IValidator<LoginRequestDto> loginValidator)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var validationResult = await _loginValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Response<LoginResponseDto>.Failure(HttpStatusCodes.BadRequest, "Validation failed", errors);
                }

                var user = await _userRepository.GetByUsername(request.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: User not found for username {Username}", request.Username);
                    return Response<LoginResponseDto>.Failure(HttpStatusCodes.BadRequest, "Invalid username or password");
                }

                var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash.Value, request.Password);
                if (passwordVerification == PasswordVerificationResult.Failed)
                {
                    _logger.LogWarning("Login failed: Invalid password for username {Username}", request.Username);
                    return Response<LoginResponseDto>.Failure(HttpStatusCodes.BadRequest, "Invalid username or password");
                }

                var token = _jwtService.GenerateToken(user);

                var refreshTokenValue = GenerateSecureToken();
                var refreshToken = new UserToken(user.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7), null); 

                await _userTokenRepository.AddAsync(refreshToken);
                await _unitOfWork.SaveChangesAsync();

                var loginResponse = new LoginResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role.ToString()
                };

                return Response<LoginResponseDto>.SuccessResult(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for username {Username}", request.Username);
                return Response<LoginResponseDto>.Failure(HttpStatusCodes.InternalError, "An unexpected error occurred");
            }
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                var validationResult = await _registerValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Response<string>.Failure(HttpStatusCodes.BadRequest, "Validation failed", errors);
                }

                var existingUser = await _userRepository.GetByUsername(request.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Username {Username} already exists", request.Username);
                    return Response<string>.Failure(HttpStatusCodes.BadRequest, "Username already exists");
                }

                var newUser = _mapper.Map<User>(request);

                newUser.SetPassword(_passwordHasher.HashPassword(newUser, request.Password));

                newUser.Activate(); // this should be by email confirmation
                newUser.SetRole(UserRole.User); // this default role untill Admin change it 


                await _userRepository.Add(newUser);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("New user registered: {Username}", newUser.Username);

                return Response<string>.SuccessResult("User registered successfully", HttpStatusCodes.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for username {Username}", request.Username);
                return Response<string>.Failure(HttpStatusCodes.InternalError, "An unexpected error occurred");
            }
        }

        public async Task<Response<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _userTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || !storedToken.IsActive)
            {
                return Response<LoginResponseDto>.Failure(HttpStatusCodes.BadRequest, "Invalid or expired refresh token");
            }

            var user = storedToken.User;

            var newJwtToken = _jwtService.GenerateToken(user);
            var newRefreshTokenValue = GenerateSecureToken();

            storedToken.Revoke(null, newRefreshTokenValue);
            await _userTokenRepository.RevokeAsync(storedToken);

            var newRefreshToken = new UserToken(user.Id, newRefreshTokenValue, DateTime.UtcNow.AddDays(7), null);
            await _userTokenRepository.AddAsync(newRefreshToken);

            await _unitOfWork.SaveChangesAsync();

            var response = new LoginResponseDto
            {
                Token = newJwtToken,
                RefreshToken = newRefreshTokenValue,
                Username = user.Username,
                Role = user.Role.ToString()
            };

            return Response<LoginResponseDto>.SuccessResult(response);
        }

 
        public async Task LogoutAsync(string refreshToken)
        {
            var storedToken = await _userTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || !storedToken.IsActive)
            {
                return;
            }
            await _userTokenRepository.RevokeAsync(storedToken, revokedByIp: null, replacedByToken: null);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LogoutAllSessionsAsync(Guid userId)
        {
            await _userTokenRepository.RevokeAllTokensAsync(userId);
            await _unitOfWork.SaveChangesAsync();
        }

        #region Helper 
        private string GenerateSecureToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        #endregion
    }
}
