using BookBazaar.Application.DTOs;
using BookBazaar.Application.Exceptions;
using BookBazaar.Application.Interfaces;
using BookBazaar.Application.Validators;
using BookBazaar.Domain.Entities;
using BookBazaar.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BookBazaar.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public BookService(IBookRepository bookRepository, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync(BookQueryDto? query = null)
        {
            _logger.LogInformation($"Start BookService.GetAllAsync");
            var books = await _bookRepository.GetAllAsync(query);
            _logger.LogInformation($"End BookService.GetAllAsync");
            return books.Select(b => BookToDto(b));
        }

        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            _logger.LogInformation($"Start BookService.GetBookByIdAsync");
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) 
            {
                throw new NotFoundException("The Book is Not Found");
            }
            var result = BookToDto(book);
            _logger.LogInformation($"End BookService.GetBookByIdAsync");
            return result;
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto dto, Guid SellerId)
        {
            _logger.LogInformation($"Start BookService.CreateBookAsync");

            if (BookValidator.CreateBookValidation(dto, out string errorMessage) == false)
            {
                throw new InValidData(errorMessage);
            }

            var book = CreateBookFromDto(dto,SellerId);
            await _bookRepository.AddAsync(book);

            _logger.LogInformation($"End BookService.CreateBookAsync");
            return BookToDto(book);
        }

        public async Task UpdateBookAsync(Guid bookId, Guid userId, UpdateBookDto dto)
        {
            _logger.LogInformation($"Start BookService.UpdateBookAsync");
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new NotFoundException("Book not found");

            if (book.IsSold)
                throw new ForbiddenOperationException("Cannot update sold book");

            if (book.SellerId != userId)
                throw new ForbiddenOperationException("Only the seller can update this book");

            if (BookValidator.UpdateBookValidation(dto, out string errorMessage) == false)
            {
                throw new InValidData(errorMessage);
            }

            if (dto.Title != null) book.Title = dto.Title;
            if (dto.Author != null) book.Author = dto.Author;
            if (dto.Description != null) book.Description = dto.Description;
            if (dto.Price.HasValue) book.Price = dto.Price.Value;
            if (dto.Condition != null) book.Condition = Enum.Parse<BookCondition>(dto.Condition);

            book.UpdatedDateTime = DateTime.UtcNow;

            await _bookRepository.UpdateAsync(book);

            _logger.LogInformation($"End BookService.UpdateBookAsync");
        }

        public async Task DeleteBookAsync(Guid bookId, Guid userId)
        {
            _logger.LogInformation($"Start DeleteBookAsync.CreateBookAsync");
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new NotFoundException("Book not found");

            if (book.IsSold)
                throw new ForbiddenOperationException("Cannot delete a sold book");

            if (book.SellerId != userId)
                throw new ForbiddenOperationException("Only the seller can delete this book");

            await _bookRepository.DeleteAsync(bookId);

            _logger.LogInformation($"End DeleteBookAsync.CreateBookAsync");
        }

        public async Task<BookDto> BuyBookAsync(Guid bookId, Guid buyerId)
        {
            _logger.LogInformation($"Start BuyBookAsync.CreateBookAsync");
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new NotFoundException("Book not found");

            if (book.IsSold)
                throw new ForbiddenOperationException("Book is already sold");

            if (book.SellerId == buyerId)
                throw new ForbiddenOperationException("Seller cannot buy their own book");

            // Update book details
            book.IsSold = true;
            book.BuyerId = buyerId;
            book.SoldDateTime = DateTime.UtcNow;
            book.UpdatedDateTime = DateTime.UtcNow;

            await _bookRepository.UpdateAsync(book);

            _logger.LogInformation($"End BuyBookAsync.CreateBookAsync");
            
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price,
                Condition = book.Condition.ToString(),
                IsSold = book.IsSold,
                SellerId = book.SellerId,
                SellerName = book.Seller?.Name ?? "",
                BuyerId = book.BuyerId,
                BuyerName = book.Buyer?.Name ?? "",
                CreatedDateTime = book.CreatedDateTime,
                UpdatedDateTime = book.UpdatedDateTime,
                SoldDateTime = book.SoldDateTime
            };
        }

        private static BookDto BookToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price,
                Condition = book.Condition.ToString(),
                IsSold = book.IsSold,
                SellerId = book.SellerId,
                SellerName = book.Seller?.Name ?? "",
                BuyerId = book.BuyerId,
                BuyerName = book.Buyer?.Name,
                CreatedDateTime = book.CreatedDateTime,
                SoldDateTime = book.SoldDateTime
            };
        }

        private static Book CreateBookFromDto(CreateBookDto dto, Guid SellerId)
        {
            if (!Enum.TryParse<BookCondition>(dto.Condition, ignoreCase: true, out var conditionEnum))
                throw new InValidData($"Invalid book condition value: {dto.Condition}");

            return new Book
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Price = dto.Price,
                Condition = conditionEnum,
                SellerId = SellerId,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                IsSold = false
            };
        }
    }
}
