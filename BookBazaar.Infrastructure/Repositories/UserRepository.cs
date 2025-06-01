using BookBazaar.Application.Interfaces;
using BookBazaar.Domain.Entities;
using BookBazaar.Infrastructure.Authentication;
using BookBazaar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookBazaar.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookBazaarDbContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserRepository(BookBazaarDbContext context, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<User?> GetByIdAsync(Guid id)
            => await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public String GenerateToken(User user) 
        {
            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}
