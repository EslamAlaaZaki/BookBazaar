using BookBazaar.Application.DTOs;
using BookBazaar.Domain.Entities;
namespace BookBazaar.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(BookQueryDto? query = null);
        Task<Book?> GetByIdAsync(Guid id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
    }
}
