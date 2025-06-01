using BookBazaar.Application.DTOs;
using BookBazaar.Application.Interfaces;
using BookBazaar.Domain.Entities;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BookBazaar.Application.Exceptions;
using BookBazaar.Application.Validators;
namespace BookBazaar.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }


        public async Task<UserDto> RegisterUserAsync(CreateUserDto dto)
        {
            _logger.LogInformation($"Start UserService.RegisterUserAsync");

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new InValidData("Email is already registered.");
            }

            if(UserValidator.RegisterUserValidation(dto, out string errorMessage) == false) 
            {
                throw new InValidData(errorMessage);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password), 
                Phone = dto.Phone,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                
            };

            await _userRepository.AddAsync(user);

            _logger.LogInformation($"End UserService.RegisterUserAsync");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        public async Task<string?> LoginUserAsync(LoginUserDto dto)
        {
            _logger.LogInformation($"Start UserService.LoginUserAsync");
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordHash != HashPassword(dto.Password)) 
            {
                throw new UnauthorizedAccessException("Authentication failed. Please check your username and password.");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };

            var token = _userRepository.GenerateToken(user);

            _logger.LogInformation($"End UserService.LoginUserAsync");

            return token;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id, Guid userId)
        {
            _logger.LogInformation($"Start UserService.GetUserByIdAsync");
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || id != userId) 
            {
                throw new UnauthorizedException("Invalid user");
            }

            _logger.LogInformation($"End UserService.GetUserByIdAsync");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        public async Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, Guid userId)
        {
            _logger.LogInformation($"Start UserService.UpdateUserAsync");
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid user");
            }

            if(UserValidator.UpdateUserValidation(dto, out string errorMessage) == false) 
            {
                throw new InValidData(errorMessage);
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                user.Phone = dto.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = HashPassword(dto.Password);

            user.UpdatedDateTime = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"End UserService.UpdateUserAsync");
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
