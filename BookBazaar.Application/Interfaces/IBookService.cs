using BookBazaar.Application.DTOs;
using BookBazaar.Domain.Entities;

namespace BookBazaar.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync(BookQueryDto? query = null);
        Task<BookDto?> GetBookByIdAsync(Guid id);
        Task<BookDto> CreateBookAsync(CreateBookDto dto, Guid SellerId);
        Task UpdateBookAsync(Guid bookId, Guid userId,UpdateBookDto dto);
        Task DeleteBookAsync(Guid id, Guid userId);
        Task<BookDto> BuyBookAsync(Guid bookId, Guid buyerId);
    }
}
