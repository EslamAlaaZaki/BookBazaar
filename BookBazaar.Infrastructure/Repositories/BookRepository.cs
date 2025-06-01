using BookBazaar.Application.DTOs;
using BookBazaar.Application.Interfaces;
using BookBazaar.Domain.Entities;
using BookBazaar.Domain.Enums;
using BookBazaar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookBazaar.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookBazaarDbContext _context;

        public BookRepository(BookBazaarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(BookQueryDto? query = null)
        {
            IQueryable<Book> books = _context.Books
                .Include(b => b.Seller)
                .Include(b => b.Buyer);

            if (query != null)
            {
                if (query.IsSold.HasValue)
                    books = books.Where(b => b.IsSold == query.IsSold.Value);

                if (!string.IsNullOrWhiteSpace(query.Title))
                    books = books.Where(b => b.Title.Contains(query.Title));

                if (!string.IsNullOrWhiteSpace(query.Author))
                    books = books.Where(b => b.Author.Contains(query.Author));

                if (!string.IsNullOrWhiteSpace(query.Condition))
                {
                    if (Enum.TryParse<BookCondition>(query.Condition, true, out var parsedCondition))
                    {
                        books = books.Where(b => b.Condition == parsedCondition);
                    }
                }

                if (query.MaxPrice.HasValue)
                    books = books.Where(b => b.Price <= query.MaxPrice.Value);

                if (query.MinPrice.HasValue)
                    books = books.Where(b => b.Price >= query.MinPrice.Value);
            }

            return await books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.Seller)
                .Include(b => b.Buyer)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
