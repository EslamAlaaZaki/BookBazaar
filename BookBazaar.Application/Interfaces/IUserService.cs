using BookBazaar.Application.DTOs;

namespace BookBazaar.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(CreateUserDto dto);
        Task<string?> LoginUserAsync(LoginUserDto dto);
        Task<UserDto?> GetUserByIdAsync(Guid id, Guid userId);
        Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, Guid userId);
    }
}
