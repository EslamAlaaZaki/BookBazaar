using BookBazaar.Application.DTOs;
using BookBazaar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);  // Added for login and register checks
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        String GenerateToken(User user);
    }
}
