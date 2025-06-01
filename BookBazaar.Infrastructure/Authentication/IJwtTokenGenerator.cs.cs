using BookBazaar.Domain.Entities;

namespace BookBazaar.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
