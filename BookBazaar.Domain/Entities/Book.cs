using BookBazaar.Domain.Enums;
namespace BookBazaar.Domain.Entities 
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public BookCondition Condition { get; set; }
        public bool IsSold { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? SoldDateTime { get; set; }

        public Guid SellerId { get; set; }
        public User Seller { get; set; } = null!;

        public Guid? BuyerId { get; set; }
        public User? Buyer { get; set; }
    }
}

